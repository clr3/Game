using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterTurns : MonoBehaviour
{

    //Create as many characters as ther will be in the game
    //if we only have the dwarf the make just one charachter here..?
    private int noOfCharacters = 5;
    [SerializeField] public Unit character1= new Unit(); 
    [SerializeField] public Unit character2= new Unit();
    [SerializeField] public Unit character3= new Unit(); 
    [SerializeField] public Unit character4= new Unit(); 
    [SerializeField] public Unit character5= new Unit(); 
    public Unit[] characters = new Unit[noOfCharacters]{};
    private int counter = 0;

    // Start is called before the first frame update
    void Start()
    {
        character1.createCharacterBio("Captain - Jack","Jack is a young captain, with a very high charima which he uses to be an ambassador. He is a strong leader and has his head on his feet.");
        //(High Charima)He can talk swiftly to manage his crew. The face of the ship.  He is a strong leader with high Sanity and Health
        character1.addSkill("Reroll","Mulligan Wizard");
        character2.createCharacterBio("Marine - John","After serving in Earth for a long time, John decided it was time to embark into space.");
        //(High Strength) In Charge of the security of the ship. The one with the highest health but low sanity due to all the things he has endured.
        
        character3.createCharacterBio("Explorer - Casey","He can move fast on the different terrains.");
        //(High speed) Can scout areas easily(skill?) Relatively high health and sanity.
        character4.createCharacterBio("Trader - Myles","An old man with the lowest health and sanity.");
        //(Lowest health but his¡ghest Sanity) 
        character5.createCharacterBio("Doctor - Bones","Highly intelligent, Bones gets a bonus when healing any character.");
        //High intelligence)(High Charisma) Bonus when healing allies.


        characters = {character1,character2,character3,character4,character5};
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Get the next character for moving
    //Should be used after clicking the next Turn Button
    public Unit nextCharacter(){
        if(count = noOfCharacters){
            count = 0;
            return characters[count];
        }else{
            count++;
            return characters[count];
        } 
    }



}
