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
        actiondummy = new GameObject("actiondummy");
        actiondummy.transform.SetParent(transform);
        skills[1] = actiondummy.AddComponent<Pressure>();

        initSkills();
    }

    private void initSkills()
    {
        // get from savedata
        PROFICIENCY = 0;
        MINDLEVEL = 1 + (PROFICIENCY/100);
    }

    public class Pressure : AMindSkill
    {
        public Pressure()
        {
            ACTIONCODE = 5;
            NAME = "Pressure";
            duration = 1.0f;
            isPassive = false;

            spCost = 5;
        }
        public override bool CanDoAction(AAnimal target)
        {
            return CanDoActionAboutHPSP(target);
        }
        public override void Action(AAnimal target)
        {
            castTime = (1.0f / target.MovementSpeed);
            duration = 0.15f;
            GameObject damagefield = Instantiate((GameObject)Resources.Load("Prefabs/Utilities/CubeDamageField"));
            damagefield.GetComponent<ADamageField>().SetMainParam(0, target.MD, duration, castTime, target.nextnextPOS + new Vector3(0, 1, 0), 1);
            damagefield.GetComponent<CubeDamageField>().SetAndAwake();
            Debug.Log(NAME + "!");
            SetMotionAndDurationAndUseHPSP(target);
        }
    }

    public class Break_The_Limit : AMindSkill
    {
        public Break_The_Limit()
        {
            ACTIONCODE = 0;
            NAME = "Break The Limit";
            duration = 1.0f;
            isPassive = true;
        }
        public override bool CanDoAction(AAnimal target)
        {
            return isPassive;
        }
        public override void Action(AAnimal target)
        {
            //Give a Buff about this.

            Debug.Log(NAME + "!");
            SetMotionAndDurationAndUseHPSP(target);
        }
    }

}
