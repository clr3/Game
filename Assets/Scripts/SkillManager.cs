using TMPro;
using UnityEngine;
using System;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour {

    [SerializeField] private SceneControl controllorSetter;
    [SerializeField] private GameObject skillTemp; //template for an entry in the skill panel
    [SerializeField] private GameObject skillsPanel; //available skills
    [SerializeField] public Sprite[] skillIcons; //skill icon reference
    static public SceneControl controllor;

    void Start() {
        controllor = controllorSetter;
        int[] playerExtraDicePlaceholder = { 2, 1, 1, 1 };

        //Adding the basic extra dice skills
        if (playerExtraDicePlaceholder[0] > 0) {
            AddSkill("AddStrengthDice");
        }
        if (playerExtraDicePlaceholder[1] > 0) {
            AddSkill("AddSpeedDice");
        }
        if (playerExtraDicePlaceholder[2] > 0) {
            AddSkill("AddIntelligenceDice");
        }
        if (playerExtraDicePlaceholder[3] > 0) {
            AddSkill("AddSocialDice");
        }
        AddSkill("SelectiveReroll");
        AddSkill("Reroll");
        AddSkill("FragileReality");
        AddSkill("CowboyDiplomacy");
        AddSkill("ImpatientForager");
        AddSkill("Resourceful");
        AddSkill("FieldMedicine");

        //Add character-specific skills
        //
    }

    void Update() {

    }

    /// <summary>
    /// Adds a skill to the skill panel.
    /// </summary>
    /// <param name="skillClassName">Specifies the particular skill to add.</param>
    public void AddSkill(string skillClassName) {

        GameObject skillButton = Instantiate(skillTemp); //creates new entry in the skills panel
        skillButton.GetComponent<Selectable>().skill = true;
        skillButton.gameObject.AddComponent(Type.GetType(skillClassName)); //set skill

        Skill skill = (Skill)skillButton.GetComponent(Type.GetType(skillClassName));
        skillButton.transform.Find("SkillText/SkillName").GetComponent<TextMeshProUGUI>().text = skill.skillName;
        skillButton.transform.Find("SkillText/SkillDesc").GetComponent<TextMeshProUGUI>().text = skill.skillDesc;

        if (skill.skillIcon != null) {
            skillButton.transform.Find("SkillIcon").GetComponent<Image>().sprite = skill.skillIcon;
        } else {
            skillButton.transform.Find("SkillIcon").GetComponent<Image>().sprite = skillIcons[0]; //use placeholder icon if no icon set
        }

        skillButton.transform.SetParent(skillsPanel.transform, false);
    }

}
