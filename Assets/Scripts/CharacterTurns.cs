using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterTurns : MonoBehaviour
{

    //Create as many characters as ther will be in the game
    private int noOfCharacters = 5;
    [SerializeField] public Unit charater1; 
    [SerializeField] public Unit charater2;
    [SerializeField] public Unit charater3; 
    [SerializeField] public Unit charater4; 
    [SerializeField] public Unit charater5; 
    public Unit[] characters = new Unit[]();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //
    public void startCharacterTurns(){
        //Check for the BASE stats to see of the game is still going
        //Do this loop every time the characters 
        for(int i = 0; i < noOfCharacters; i++){

        }
    }





}
