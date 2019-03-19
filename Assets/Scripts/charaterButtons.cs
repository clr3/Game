using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class charaterButtons : MonoBehaviour
{   
    //Next character button will 
    //Get next character and update the card
    //start listener for a click
    //MakeMovements -> Manage evenets and challenges
    //End turn
    [SerializeField] private Text speedText = ;
    [SerializeField] private Text strengthText;
    [SerializeField] private Text charismaText;
    [SerializeField] private Text intelligenceText;
    
    [SerializeField] private Text bio;
    [SerializeField] private Text name;

    [SerializeField] private Unit character;
    [SerializeField] private CharacterTurns turn;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //CharacterVard button will display the card
    //MakeMovements -> Manage evenets and challenges
    //End turn
    public void startNextTurn(){
        character = turn.snextCharacter);
        updateCardDetails();
    }    

    public void updateCardDetails(){
        name = character.characterName;
        bio = character.description;

        speedText = character.speed;
        strengthText = character.strength;
        charismaText = character.charisma;
        intelligenceText = character.intelligence;
    
    }
}
