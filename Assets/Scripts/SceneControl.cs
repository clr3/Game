using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SceneControl : MonoBehaviour {
    [SerializeField] private Sprite[] diceSymbols;
    [SerializeField] private Dice originalDice;
    [SerializeField] private GameObject dicePoolPanel;
    [SerializeField] private Button rollButton;
    [SerializeField] private Button resolveButton;
    [SerializeField] private SkillManager managerSetter;
    [SerializeField] private GameObject skillPanel;
    [SerializeField] private EventManager eventManager;
    [SerializeField] private Canvas challengeUI;
    
    static public SkillManager manager;
    public Unit challenger;

    private List<Dice> diceSet = new List<Dice>();
    private List<Dice> inactiveDice = new List<Dice>();
    private List<Dice> clonesDice = new List<Dice>();
    private string[] results;
    private Vector3 startPos;

    void Start() {
        manager = managerSetter;
        originalDice.GetComponent<EventTrigger>().enabled = false;
        originalDice.GetComponent<Selectable>().enabled = false; //prevent dice selection until at least one roll has happened
        resolveButton.interactable = false; //prevent challenge completion until at least one roll has happened
        startPos = originalDice.transform.position; //screen position of the original die
        rollButton.interactable = false;
    }

    public void Initialise(Unit activeUnit, int[] goals) { //CHARCTER, CHALLENGE GOALS,
        challengeUI.gameObject.SetActive(true);
        challenger = activeUnit;

        foreach (Dice clone in clonesDice) {
            Destroy(clone.gameObject);
        }

        diceSet.Clear();
        inactiveDice.Clear();
        clonesDice.Clear();
        originalDice.GetComponent<EventTrigger>().enabled = false;
        originalDice.GetComponent<Selectable>().enabled = false;
        resolveButton.interactable = false;

        SetChallenge(goals); 
    }


    /// <summary>
    /// Open/close skill panel.vv
    /// </summary>
    public void OpenSkillPanel() {
        skillPanel.SetActive(!skillPanel.activeSelf);
    }

    /// <summary>
    /// Builds the dice pool.
    /// </summary>
    /// <param name="pool">Number of dice to generate</param>
    void GenerateDice(int pool) {

        //Disable the original die if 0 dice are to be generated
        if (pool < 1) {
            originalDice.ChangeDiceState();
        }

        originalDice.diceSides = PopulateDice(challenger.getStatArray()); //set the dice sides

        //Dice grid layout variables - currently allows for a maximum of 16 dice
        const int gridRows = 2;
        const int gridCols = 8;
        const float offsetX = 1.8f;
        const float offsetY = -1.5f;

        //Fills in the layout with dice objects
        for (int i = 0; i < gridRows; i++) {
            for (int j = 0; j < gridCols; j++) {
                Dice die;
                if (i == 0 && j == 0) {
                    die = originalDice;
                } else {
                    die = Instantiate(originalDice) as Dice;
                    clonesDice.Add(die);
                    die.transform.SetParent(dicePoolPanel.transform, false);
                }

                //Position the dice on-screen
                float posX = (offsetX * j) + startPos.x;
                float posY = (offsetY * i) + startPos.y;
                die.transform.position = new Vector3(posX, posY, startPos.z);

                //Only show dice that are apart of the generated dice pool
                if (pool > 0) {
                    diceSet.Add(die);
                    pool--;
                } else {
                    die.ChangeDiceState();
                    inactiveDice.Add(die);
                }
            }
        }

    }

    /// <summary>
    /// Roll the dice.
    /// </summary>
    public void Roll() {
        diceSet.ForEach(x => { x.GetComponent<EventTrigger>().enabled = false; x.GetComponent<Selectable>().enabled = false; });
        rollButton.interactable = false; //disable re-roll
        foreach (Dice die in diceSet) {
            StartCoroutine("RollHelper", die);
        }
    }

    /// <summary>
    /// Roll the dice with slower face changes as dice approaches its result.
    /// </summary>
    /// <returns></returns>
    public IEnumerator RollHelper(Dice die) {
        resolveButton.interactable = false;
        float rollTimer = 0.1f; //initial time between face changes

        while (rollTimer < 0.7f) {
            yield return new WaitForSeconds(rollTimer);
            die.RollDice();
            rollTimer *= 1.1f; //slow dice roll over time
        }

        die.GetComponent<Selectable>().enabled = true;
        die.GetComponent<EventTrigger>().enabled = true;
        resolveButton.interactable = true;
    }


    /// <summary>
    /// Create a new dice configuration.
    /// </summary>
    /// <param name="stats"></param>
    /// <returns></returns>
    private Sprite[] PopulateDice(int[] stats) {
        Sprite[] dice = new Sprite[12]; //12-sided dice

        int index = 0;
        int symbolIndex = 0;
        Sprite symbol;

        foreach (int stat in stats) {
            symbol = diceSymbols[symbolIndex];
            for (int i = 0; i < stat; i++) {
                dice[index] = symbol;
                index++;
            }
            symbolIndex++;
        }
        return dice;
    }

    /// <summary>
    /// Set the goals of the challenge and the initial dice pool size.
    /// </summary>
    /// <param name="baseNumOfDice">Initial dice pool size</param>
    /// <param name="goal">Goals of the challenge</param>
    public void SetChallenge(int[] goal) {
        PlayerPrefs.SetInt("Strength", goal[0]);
        PlayerPrefs.SetInt("Speed", goal[1]);
        PlayerPrefs.SetInt("Intelligence", goal[2]);
        PlayerPrefs.SetInt("Social", goal[3]);
        GenerateDice(goal[4]);
        rollButton.interactable = true;
    }

    /// <summary>
    /// Determine challenge success or failure.
    /// </summary>
    public void ResolveChallenge() {

        int strength = 0;
        int speed = 0;
        int intelligence = 0;
        int social = 0;
        bool success = true;

        foreach (Dice die in diceSet) {
            if (die.gameObject.GetComponent<Selectable>().selected) {
                switch (die.id) {
                    case "strength":
                        strength++;
                        break;
                    case "speed":
                        speed++;
                        break;
                    case "intelligence":
                        intelligence++;
                        break;
                    case "social":
                        social++;
                        break;
                }
            }
        }

        if (strength < PlayerPrefs.GetInt("Strength")) {
            //Debug.Log("Strength Failure ");
            success = false;
        }
        if (speed < PlayerPrefs.GetInt("Speed")) {
            //Debug.Log("Speed Failure");
            success = false;
        }
        if (intelligence < PlayerPrefs.GetInt("Intelligence")) {
            //Debug.Log("Intelligence Failure");
            success = false;
        }
        if (social < PlayerPrefs.GetInt("Social")) {
            //Debug.Log("Social Failure");
            success = false;
        }
        //Debug.Log("Challenge Success = " + success);

        challengeUI.gameObject.SetActive(false);

        if (success) {
            eventManager.HandleSucess();
        } else {
            eventManager.HandleFailure();
        }
    }

    /* SKILL FUNCTIONS*/

    /// <summary>
    /// Add a die to the dice pool.
    /// </summary>
    public void AddDie() {
        if (inactiveDice.Count > 0) {
            Dice die = inactiveDice[0];
            inactiveDice.Remove(die);
            die.ChangeDiceState();
            diceSet.Add(die);
        }
    }

    /// <summary>
    /// Remove a die from the dice pool.
    /// </summary>
    public void RemoveDie() {
        if (diceSet.Count > 0) {
            Dice die = diceSet[diceSet.Count - 1];
            diceSet.Remove(die);
            die.ChangeDiceState();
            inactiveDice.Insert(0, die);
        }
    }

    /// <summary>
    /// Alter the challenge goals.
    /// </summary>
    /// <param name="targetGoal">The goal that is to be altered.</param>
    /// <param name="value">Amount to alter the goal by.</param>
    public void AlterGoal(string targetGoal, int value) {
        int current = PlayerPrefs.GetInt(targetGoal);
        PlayerPrefs.SetInt(targetGoal, current + value);
    }

    public void Reroll() {
        rollButton.interactable = true;
        resolveButton.interactable = false;
        rollButton.GetComponentInChildren<Text>().text = "Reroll";
    }

    public void RerollDie(int numOfDice) {
        int count = 0;

        foreach (Dice die in diceSet) {
            Selectable selectDie = die.gameObject.GetComponent<Selectable>();
            if (selectDie.selected && count < numOfDice) {
                StartCoroutine("RollHelper", die);
                count++;
            }
            selectDie.selected = false;
            selectDie.PointerExit();
        }
    }

    public void RandomAlterGoal(int value) {

        int target = Random.Range(0, 4);
        int current;

        switch (target) {
            case 0:
                current = PlayerPrefs.GetInt("Strength");
                PlayerPrefs.SetInt("Strength", current + value);
                break;
            case 1:
                current = PlayerPrefs.GetInt("Speed");
                PlayerPrefs.SetInt("Speed", current + value);
                break;
            case 2:
                current = PlayerPrefs.GetInt("Intelligence");
                PlayerPrefs.SetInt("Intelligence", current + value);
                break;
            case 3:
                current = PlayerPrefs.GetInt("Social");
                PlayerPrefs.SetInt("Social", current + value);
                break;
        }

    }

}

