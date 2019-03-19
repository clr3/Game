using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitSelectionPanel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        mouseController = GameObject.FindObjectOfType<MouseController>();

    }

    public Text Title;
    public Text Movement;
    public Text MovePath;

    MouseController mouseController;
    // Update is called once per frame
    void Update()
    {
        if(mouseController.SelectedUnit != null)
        {
            Unit unit = mouseController.SelectedUnit;
            Title.text = "Selected Unit: " + unit.characterName;
            Movement.text = string.Format("AP: {0}/{1}", unit.ap.ToString(), unit.speed.ToString());
            //Title.text = mouseController.SelectedUnit.characterName;
        }

        else
        {
         
        }
    }
}
