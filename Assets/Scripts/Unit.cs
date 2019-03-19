using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QPath;
using System.Linq;

public class Unit : IQPathUnit{
   
    //Character Informarion
    [SerializeField] public string characterName { get; set; }
    [SerializeField] public string description { get; set; }
    [SerializeField] public Dictionary<string, string> challengeSkills = new Dictionary<string, string>(); //Have to match the name of the skill class 
    public string[] mapSkills = new string[6];
    public int[] extraDice = new int[] { 0, 0, 0, 0 };  //Events can Increment this numbers

    //Charactes Stats
    [SerializeField] public int strenght { get; set; }
    [SerializeField] public int speed { get; set; }
    [SerializeField] public int intelligence { get; set; }
    [SerializeField] public int charisma { get; set; }
    [SerializeField] public int health { get; set; }
    [SerializeField] public int sanity { get; set; }

    private int ap { get; set; }         //AP = to speed but changes every turn
    //Character Items
    public int food { get; set; }
    public int resources { get; set; }
    public int meds { get; set; }

    //Values that will not be changed
    [SerializeField] private int carryCapacity { get; set; } //Carry Capacity = Strenght 2x
    [SerializeField] private int maxHealth;
    [SerializeField] private int maxSanity;


    

    void Start()
    {
        food = 0;
        resources = 0;
        meds = 0;
    }

    //Return an array with the values in the order:
    // [Health,Sanity, Strenght, Speed, Intelligence, Social]
    public int[] getStatArray()
    {
        return new int[] { health, sanity, strenght, speed, intelligence, charisma };
    }

    public void createCharacterAtributes(int str, int spd, int inte, int ch, int heal, int san)
    {
        strenght = str;
        speed = spd;
        intelligence = inte;
        charisma = ch;
        health = heal;
        sanity = san;
        //PresetValues that won't Change
        carryCapacity = 2 * str;
        maxHealth = heal;
        maxSanity = san;
        ap = speed;
    }

    public void createCharacterBio(string name, string bio)
    {
        characterName = name;
        description = bio;
    }

    public void addChallengeSkill(string skillClass, string skillName)
    {
        challengeSkills.Add(skillClass, skillName);
    }

    //Returns true when the health of the hero = 0
    public bool Dead()
    {
        if (health == 0)
        {
            return true;
        }
        return false;
    }

    //Put AP back to max at the end of the turn
    public void restoreAP()
    {
        ap = speed;
    }

    //Returns true if there is points to remove
    public bool removeAP(int cost)
    {
        if (actionPointsLeft(cost))
        {
            ap -= cost;
            return true;
        }
        else
        {
            Debug.Log("not enouch ap left");
            return false;
        }


    }
    //Returns true when there are still action points in the turn
    public bool actionPointsLeft(int cost)
    {
        if (cost > ap) return false;
        return true;
    }

    public Hex Hex { get; protected set; }

    public delegate void UnitMovedDelegate(Hex oldHex, Hex newHex);
    public event UnitMovedDelegate OnUnitMoved;

    Queue<Hex> hexPath;

    //TODO: Should be moved to central config
    const bool MOVEMENT_RULSE_LIKE_CIV6 = false;

    public void SetHex(Hex newHex)
    {
        Hex oldHex = Hex;
        if (Hex != null)
        {
            Hex.RemoveUnit(this);
        }

        Hex = newHex;

        Hex.AddUnit(this);

        if(OnUnitMoved != null)
        {
            OnUnitMoved(oldHex, newHex);
        }
    }

    public void DUMMY_PATHING_FUNCTION()
    {

        Hex[] pathHexes = QPath.QPath.FindPath<Hex>(
            Hex.HexMap,
            this, 
            Hex, 
            Hex.HexMap.GetHexAt(Hex.C + 3, Hex.R - 6), 
            Hex.CostEstimate
            );

        Debug.Log("Got pathfinding path of length " + pathHexes.Length);
        SetHexPath(pathHexes);

    }

    public void ClearHexPath()
    {
        this.hexPath = new Queue<Hex>();
    }
    public void SetHexPath(Hex[] hexArray)
    {
        this.hexPath = new Queue<Hex>(hexArray);

        if (hexPath.Count > 0)
        {
            this.hexPath.Dequeue(); // First hex is the one we're standing in, so throw it out.
        }
    }

    public void DoTurn()
    {
        // do queued move?
        Debug.Log("Do turn");
        //Testing: Move us one tile to the right

        if(hexPath == null || hexPath.Count == 0)
        {
            return;
        }
        // Grab first hex from queue 
        Hex newHex = hexPath.Dequeue();

        SetHex(newHex);
    }

    public int MovementCostToEnterHex(Hex hex)
    {
        //TODO: Override base movement cost based on movement mode + tile type;
        return hex.BaseMovementCost();
    }
    float test_speed = 2;
    float test_ap = 1;

    public float AggregateTurnsToEnterHex(Hex hex, float turnsToDate)
    {
        float baseTurnsToEnterHex = MovementCostToEnterHex(hex) / test_speed; //Ex: Entering grass "1" turn

        if(baseTurnsToEnterHex < 0)
        {
            // Impassable
            Debug.Log("Impassible terrain at: " + hex.ToString());
            return -9999f;
        }

        float turnsRemaining = test_ap / speed; //Ex: if at 1/2 move, we have .5 turns left

        float turnsToDateWhole = Mathf.Floor(turnsToDate); // 4.33 => 4
        float turnsToDateFraction = turnsToDate - turnsToDateWhole; // 4.33 => 0.33

        if (turnsToDateFraction > 0 && turnsToDateFraction < 0.01f || turnsToDateFraction > 0.99f){
            Debug.LogError("Floating point drift");

            if (turnsToDateFraction < 0.01f) turnsToDateFraction = 0;
            if (turnsToDateFraction > 0.99f)
            {
                turnsToDateWhole += 1;
                turnsToDateFraction = 0;
            }

        }

        float turnsUsedAfterThismove = turnsToDateFraction + baseTurnsToEnterHex; //Ex: 0.33 + 1

        if (turnsUsedAfterThismove > 1)
        {
            // Not enough movement to complete move
            if (MOVEMENT_RULSE_LIKE_CIV6)
            {
                //// Can't enter tile, 
                if (turnsToDateFraction == 0)
                {
                    // we have full movement, but this isnt enough to enter tile
                }
                else
                {
                    // Not on a fresh turn
                    // sit idle for remainder of turn.
                    turnsToDateWhole += 1;
                    turnsToDateFraction = 0;
                }
                turnsUsedAfterThismove = baseTurnsToEnterHex;
                if (turnsUsedAfterThismove > 1) turnsUsedAfterThismove = 1;
            }
        }
        else
        {
            //Civ5 style movement state, we can always enter movement tile
            turnsUsedAfterThismove = 1;
        }
        return turnsToDateWhole + turnsUsedAfterThismove;
       
    }

    /// <summary>
    /// Turn cost to enter a hex (0.5 turns if movement cost is 1 and we have 2 max movement)
    /// </summary>

    public float CostToEnterHex(IQPathTile sourceTile, IQPathTile destinationTile)
    {
        return 1;
    }

}
