using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Character : MonoBehaviour
{
        //Character Informarion
    [SerializeField] public string characterName {get;set;}
    [SerializeField] public string description {get;set;}
    [SerializeField] public Dictionary<String,String> challengeSkills = new Dictionary<String,String>(); //Have to match the name of the skill class 
    public string[] mapSkills = new string[6];
    public int[] extraDice = new int[]{0,0,0,0};  //Events can Increment this numbers

        //Charactes Stats
    [SerializeField]public int strenght { get; set; } 
    [SerializeField]public int speed{ get; set; }
    [SerializeField]public int intelligence { get; set; }
    [SerializeField]public int charisma { get; set; }
    [SerializeField]public int health { get; set; }
    [SerializeField]public int sanity { get; set; }
        //CharacterAP
    private int ap{ get; set; }         //AP = to speed but changes every turn
        //Character Items
    public int food {get; set;}
    public int resources{get; set;}
    public int meds{get;set;}
        //Values that will not be changed
    [SerializeField] private int carryCapacity{get; set;} //Carry Capacity = Strenght 2x
    [SerializeField] private int maxHealth;
    [SerializeField] private int maxSanity;
    // Start is called before the first frame update
    //No items are being carried
    void Start()
    {
        food = 0;
        resources = 0;
        meds = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //Return an array with the values in the order:
    // [Health,Sanity, Strenght, Speed, Intelligence, Social]
    public int[] getStatArray(){
        return new int[health, sanity, strenght, speed, intelligence, charisma];
    }
    public void createCharacterAtributes(int str, int spd, int inte, int ch, int heal, int san){
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

    public void createCharacterBio(string name, string bio){
        characterName = name;
        description = bio;
    }

    public void addChallengeSkill(string skillClass, string skillName){
        challengeSkills.Add(skillClass, skillName);
    }
    //Returns true when the health of the hero = 0
    public bool noHealth(){
        if(health == 0){

        }
    }
    //Put AP back to max at the end of the turn
    public void restoreAP(){
        ap = speed;
    }
    //Returns true if there is points to remove
    public bool removeAP(int piointsToRemove){
        if(actionPointsLeft()){
            if(piointsToRemove > ap) {
                ap = 0;
            }else{
                ap = ap - piointsToRemove;
            }
            return true;
        }
        else return false;
        
    }
    //Returns true when there are still action points in the turn
    public bool actionPointsLeft(){
        if(ap == 0 ) return false;
        return true;
    }


}
