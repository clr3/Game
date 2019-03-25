using UnityEngine;
public abstract class Skill : MonoBehaviour {
    public SceneControl controllor = SkillManager.controllor;
    public SkillManager manager = SceneControl.manager;
    public string skillName { get; protected set; }
    public string skillDesc { get; protected set; }
    public Sprite skillIcon { get; protected set; }

    public abstract void Activate();
}


/* SKILLS */


public class AddStrengthDice : Skill {
    void Awake() {
        this.skillName = "Strength+";
        this.skillDesc = "Reduce strength requirement by one";
        this.skillIcon = manager.skillIcons[1];
    }

    public override void Activate() {
        SkillManager.controllor.AlterGoal("Strength", -1);
        SkillManager.controllor.OpenSkillPanel();
    }
}

public class AddSpeedDice : Skill {
    void Awake() {
        this.skillName = "Speed+";
        this.skillDesc = "Reduce speed requirement by one";
        this.skillIcon = manager.skillIcons[2];
    }

    public override void Activate() {
        SkillManager.controllor.AlterGoal("Speed", -1);
        SkillManager.controllor.OpenSkillPanel();
    }
}

public class AddIntelligenceDice : Skill {
    void Awake() {
        this.skillName = "Intelligence+";
        this.skillDesc = "Reduce intelligence requirement by one";
        this.skillIcon = manager.skillIcons[3];
    }

    public override void Activate() {
        SkillManager.controllor.AlterGoal("Intelligence", -1);
        SkillManager.controllor.OpenSkillPanel();
    }
}

public class AddSocialDice : Skill {
    void Awake() {
        this.skillName = "Social+";
        this.skillDesc = "Reduce social requirement by one";
        this.skillIcon = manager.skillIcons[4];
    }

    public override void Activate() {
        SkillManager.controllor.AlterGoal("Social", -1);
        SkillManager.controllor.OpenSkillPanel();
    }
}

public class Reroll : Skill {
    void Awake() {
        this.skillName = "Mulligan Wizard";
        this.skillDesc = "I was just pretending to fail! Get a reroll.";
        this.skillIcon = manager.skillIcons[5];
    }

    public override void Activate() {
        SkillManager.controllor.Reroll();
        SkillManager.controllor.OpenSkillPanel();
    }
}

public class SelectiveReroll : Skill {
    void Awake() {
        this.skillName = "Planning";
        this.skillDesc = "When things don't go according to plan, plan according to how things go. Rerolls up to three selected dice.";
        this.skillIcon = manager.skillIcons[6];
    }

    public override void Activate() {

        SkillManager.controllor.RerollDie(4);
        SkillManager.controllor.OpenSkillPanel();
    }

}

public class FieldMedicine : Skill {
    void Awake() {
        this.skillName = "Field Medicine";
        this.skillDesc = "Patched up, good as new...ish. Heal for 10hp but a random increases by one.";
        this.skillIcon = manager.skillIcons[7];
    }

    public override void Activate() {
        controllor.RandomAlterGoal(-1);
        //char.alterHealth(10);//////////////////////
        SkillManager.controllor.OpenSkillPanel();
    }

}

public class Resourceful : Skill {
    void Awake() {
        this.skillName = "Resourceful";
        this.skillDesc = "When you don’t have resources, you become resourceful. Add one die to the dice pool";
        this.skillIcon = manager.skillIcons[8];
    }

    public override void Activate() {
        SkillManager.controllor.AddDie();
        SkillManager.controllor.OpenSkillPanel();
    }
}

public class ImpatientForager : Skill {
    void Awake() {
        this.skillName = "Impatient Forager";
        this.skillIcon = manager.skillIcons[9];
        this.skillDesc = "Danger can wait, there's resources to gather!. Gain a random resource but removes two dice from the dice pool";
    }

    public override void Activate() {
        //char.food++;////////////////////////
        controllor.RemoveDie();
        controllor.RemoveDie();
        SkillManager.controllor.OpenSkillPanel();
    }
}

public class CowboyDiplomacy : Skill {
    void Awake() {
        this.skillName = "Cowboy Diplomacy";
        this.skillDesc = "The best diplomat is the one with the bigger stick. Social checks become Strength ones";
        this.skillIcon = manager.skillIcons[10];
    }

    public override void Activate() {
        int temp = PlayerPrefs.GetInt("Social");
        controllor.AlterGoal("Social", -temp);
        controllor.AlterGoal("Strength", temp);
        SkillManager.controllor.OpenSkillPanel();
    }
}

public class FragileReality : Skill {
    void Awake() {
        this.skillName = "Fragile Reality";
        this.skillDesc = "Gaze blackly into the penguin-fringed abyss and embrace madness. Sacrifice your mind to save your body";
        this.skillIcon = manager.skillIcons[11];
    }

    public override void Activate() {
        controllor.Roll();
        controllor.AlterGoal("Strength", Random.Range(-2,1));
        controllor.AlterGoal("Speed", Random.Range(-2, 1));
        controllor.AlterGoal("Intelligence", Random.Range(-2, 1));
        controllor.AlterGoal("Social", -Random.Range(-2, 1));
 
        SkillManager.controllor.OpenSkillPanel();
    }


}

