using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dice : MonoBehaviour
{
    [SerializeField] private SceneControl controller;
    public Sprite[] diceSides;
    [SerializeField] public Sprite startingSymbol;
    private Color startcolor;
    public string id { get; private set; }

    void Start() {
        this.gameObject.GetComponent<Image>().sprite = startingSymbol;
    }

    void Update() {
        
    }

    /// <summary>
    /// Change 
    /// </summary>
    /// <param name="diceType"></param>
    /// <param name="image"></param> 
    public void SetDieFace(int diceType, Sprite image) {

        switch (diceType) {
            case 0:
                this.id = "strength";
                break;
            case 1:
                this.id = "speed";
                break;
            case 2:
                this.id = "intelligence";
                break;
            case 3:
                this.id = "social";
                break;
        }

        this.gameObject.GetComponent<Image>().sprite = image;
    }

    public void RollDice() {
        int side = Random.Range(0, diceSides.Length-1);
        switch (diceSides[side].name) {
            case "diceSymbol_Strength":
                this.id = "strength";
                break;
            case "diceSymbol_Speed":
                this.id = "speed";
                break;
            case "diceSymbol_Intelligence":
                this.id = "intelligence";
                break;
            case "diceSymbol_Social":
                this.id = "social";
                break;
        }
        this.SetDieFace(4, diceSides[side]);

    }

    /// <summary>
    /// Enable/disable die.
    /// </summary>
    public void ChangeDiceState() {
        this.gameObject.SetActive(!this.gameObject.activeSelf);
    }
    
}
