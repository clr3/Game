using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour{

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

    [SerializeField]public string[] chClassSkills = new string[6];
    [SerializeField]private string[] chSkillNames = new string[6];//Max size for now(SkillsList)
    [SerializeField]public string[] mapSkills = new string[6];
    [SerializeField]public int[] extraDice = new int[] { 0, 0, 0, 0 };  //Events can Increment this numbers


    //Character Information
    public string characterName { get; set; }
    public string description { get; set; }
    [SerializeField]public Dictionary<string, string> challengeSkills = new Dictionary<string, string>(); //Have to match the name of the skill class 
   
    //Charactes Stats
    public int strenght { get; set; }
    public int speed { get; set; }
    public int intelligence { get; set; }
    public int charisma { get; set; }
    public int health { get; set; }
    public int sanity { get; set; }

    private int ap { get; set; }         //AP = to speed but changes every turn
    //Character Items
    public int food { get; set; }
    public int resources { get; set; }
    public int meds { get; set; }

  



    public Hex Hex { get; protected set; }

    void Start()
    {
        food = 0;
        resources = 0;
        meds = 0;
    }

    void create(){
        createCharacterBio(NAME, BIO);
        createCharacterAtributes(STRENGTH, SPEED, INTELLIGENCE, CHARISMA, maxHealth, maxSanity);
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

    public void addChallengeSkills(string skillClass, string skillName)
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

    public void setHex(Hex hex)
    {
        if(hex != null)
        {
            Hex.RemoveUnit(this);
        }

        Hex = hex;

        Hex.AddUnit(this);

    }

    public void DoTurn()
    {
        // do queued move?

        //Testing: Move us one tile to the right

        Hex oldHex = Hex;
        Hex newHex = oldHex.HexMap.GetHexAt(oldHex.C + 1, oldHex.R);

        setHex(newHex);
    }
}
