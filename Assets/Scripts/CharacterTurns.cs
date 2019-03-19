using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterTurns : MonoBehaviour
{

    //Create as many characters as ther will be in the game
    //if we only have the dwarf the make just one charachter here..?
    private int noOfCharacters = 5;
    [SerializeField] public Unit charater1; 
    [SerializeField] public Unit charater2;
    [SerializeField] public Unit charater3; 
    [SerializeField] public Unit charater4; 
    [SerializeField] public Unit charater5; 
    public Unit[] characters;
    private int counter = 0;

    // Start is called before the first frame update
    void Start()
    {
        characters = new Unit[](charater1,charater2,charater3,charater4,charater5);
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
