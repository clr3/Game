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
public class DiceLord : Skill {
    void Awake() {
        this.skillName = "Dice Lord";
        this.skillDesc = "I am lord of all I can roll. Add one die to the dice pool";
    }

    public override void Activate() {
        SkillManager.controllor.AddDie();
    }
}

public class DiceDunce : Skill {
    void Awake() {
        this.skillName = "Dice Dunce";
        this.skillDesc = "i got da picture one!. Remove one die from the dice pool";
    }

    public override void Activate() {
        controllor.RemoveDie();
    }
}
