using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QPath;
using System.Linq;
using System;

public class Unit : IQPathUnit{

    //Values that will not be changed
    [SerializeField] private int carryCapacity { get; set; } //Carry Capacity = Strenght 2x
    [SerializeField] private int maxHealth;
    [SerializeField] private int maxSanity;
    //Information so that it can be changes in unity
    [SerializeField] private string NAME;
    [SerializeField] private string BIO;
    [SerializeField] private int STRENGTH;
    [SerializeField] private int SPEED;
    [SerializeField] private int INTELLIGENCE;
    [SerializeField] private int CHARISMA;

    [SerializeField] public string[] chClassSkills = new string[6];
    [SerializeField] private string[] chSkillNames = new string[6];//Max size for now(SkillsList)
    [SerializeField] public string[] mapSkills = new string[6];
    [SerializeField] public int[] extraDice = new int[] { 0, 0, 0, 0 };  //Events can Increment this numbers


    //Character Information
    public string characterName  { get; set; }
    public string description { get; set; }
    [SerializeField] public Dictionary<string, string> challengeSkills = new Dictionary<string, string>(); //Have to match the name of the skill class 

    //Charactes Stats
    public float speed = 4;
    public float ap=5;


    public int strenght { get; set; }
   // public int speed { get; set; }
    public int intelligence { get; set; }
    public int charisma { get; set; }
    public int health { get; set; }
    public int sanity { get; set; }

    //public int ap { get; set; }         //AP = to speed but changes every turn
    //Character Items
    public int food { get; set; }
    public int resources { get; set; }
    public int meds { get; set; }



    void Start()
    {
        food = 0;
        resources = 0;
        meds = 0;

        Debug.Log(ap);
    }

    //Return an array with the values in the order:
    // [Health,Sanity, Strenght, Speed, Intelligence, Social]
    public int[] getStatArray()
    {
        return new int[] { health, sanity, strenght,
            //speed,
            intelligence, charisma };
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
        if (health <= 0)
        {
            return true;
        }
        return false;
    }

    //Put AP back to max at the end of the turn
    public void RefreshMovement()
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

    List<Hex> hexPath;

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

    public void ClearHexPath()
    {
        this.hexPath = new List<Hex>();
    }
    public void SetHexPath(Hex[] hexArray)
    {
        this.hexPath = new List<Hex>(hexArray);
        /*
        if (hexPath.Count > 0)
        {
            this.hexPath.Dequeue(); // First hex is the one we're standing in, so throw it out.
        }*/
    }

    public Hex[] GetHexPath()
    {
        return (this.hexPath == null) ? null : this.hexPath.ToArray();
    }

    public static void MyDelay(int seconds)
    {
        DateTime dt = DateTime.Now + TimeSpan.FromSeconds(seconds);

        do { } while (DateTime.Now < dt);
    }

    public bool UnitWaitingForOrders()
    {
        if(ap > 0 && (hexPath == null || hexPath.Count == 0))
        {
            return true;
        }
        return false;
    }
    
    public bool DoMove()
    {
        // do queued move?
        Debug.Log("Do turn");
        
        if(ap <= 0)
        {
            return false;
        }

        if(hexPath == null || hexPath.Count == 0)
        {
            return false;
        }

        Hex hexWeAreLeaving = hexPath[0];
        Hex newHex = hexPath[1];



        int costToEnter = MovementCostToEnterHex(newHex, false);

        if(costToEnter > ap && MOVEMENT_RULSE_LIKE_CIV6)
        {
            return false;
        }

        hexPath.RemoveAt(0);

        if (hexPath.Count == 1)
        {
            // only 1 more hex left, no more path to follow
            hexPath = null;
        }

        SetHex(newHex);
        ap = Mathf.Max(ap - costToEnter, 0); 
        return hexPath != null && ap > 0 ;

        // Grab first hex from queue 

    }

    public int MovementCostToEnterHex(Hex hex, bool isScout)
    {
        if (isScout)
        {
            if (hex.ElevationType == Hex.ELEVATION_TYPE.SWAMP)
            {
                return 3;
            }

            if (hex.ElevationType == Hex.ELEVATION_TYPE.MOUNTAIN || hex.ElevationType == Hex.ELEVATION_TYPE.RIVER)
            {
                return 8;
            }

            if (hex.ElevationType == Hex.ELEVATION_TYPE.DESERT)
            {
                return 5;
            }
            return 1;
        }
        return hex.BaseMovementCost();

    }

    public float AggregateTurnsToEnterHex(Hex hex, float turnsToDate)
    {
        float baseTurnsToEnterHex = MovementCostToEnterHex(hex, false) / speed; //Ex: Entering grass "1" turn

        if(baseTurnsToEnterHex < 0)
        {
            // Impassable
            Debug.Log("Impassible terrain at: " + hex.ToString());
            return -9999f;
        }

        float turnsRemaining = ap / speed; //Ex: if at 1/2 move, we have .5 turns left

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
