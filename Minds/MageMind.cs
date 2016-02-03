using System;
using UnityEngine;
using UnityEngine.UI;

public class MageMind : AMind {

    protected override void Start()
    {
        NAME = "Mage"; // const
        TYPE = "Mind"; // const
        FLAVOR = "Mage is a Nuker.";
        ICON = GetComponent<Image>().sprite; // const
        PROFICIENCY = 0;
        NUMofMindSkills = 10+1;
        skills = new AMindSkill[NUMofMindSkills];
        skills[1] = new Pressure();

        initSkills();
    }

    private void initSkills()
    {
        // get from savedata
        PROFICIENCY = 0;
        MINDLEVEL = 1 + (PROFICIENCY/100);
    }

    public override AMindSkill GetMindSkill(int index)
    {
        if (MINDLEVEL >= index) { return skills[index]; }
        else { Debug.Log("Need more MindLevel."); }
        return null;
    }

    protected class Pressure : AMindSkill
    {
        public Pressure()
        {
            ACTIONCODE = 5;
            NAME = "Pressure";
            duration = 0.0f;
            isPassive = false;
        }
        public override void Action(AAnimal target)
        {
            Debug.Log("Pressure!");
            SetMotionAndDuration(target);
        }
    }

}
