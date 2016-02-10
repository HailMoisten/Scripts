using System;
using UnityEngine;
using UnityEngine.UI;

public class MageMind : AMind {

    protected override void Start()
    {
        _name = "Mage"; // const
        type = "Mind"; // const
        flavor = "Mage is a Nuker.";
        icon = GetComponent<Image>().sprite; // const
        proficiency = 0;

        initSkills();
    }

    private void initSkills()
    {
        // get from savedata
        proficiency = 0;
        mindlevel = 1 + (proficiency/100);

        transform.GetChild(0).gameObject.AddComponent<IdleAction>();
        transform.GetChild(1).gameObject.AddComponent<Pressure>();
        transform.GetChild(2).gameObject.AddComponent<IdleAction>();
        transform.GetChild(3).gameObject.AddComponent<IdleAction>();
        transform.GetChild(4).gameObject.AddComponent<IdleAction>();
        transform.GetChild(5).gameObject.AddComponent<IdleAction>();
        transform.GetChild(6).gameObject.AddComponent<IdleAction>();
        transform.GetChild(7).gameObject.AddComponent<IdleAction>();
        transform.GetChild(8).gameObject.AddComponent<IdleAction>();
        transform.GetChild(9).gameObject.AddComponent<IdleAction>();
        transform.GetChild(10).gameObject.AddComponent<Break_The_Limit>();

    }

    public class Pressure : AMindSkill
    {
        public Pressure()
        {
            actioncode = 5;
            _name = "Pressure";
            flavor = "Give 1.0*MD damage as magicdamage.";
            icon = GetComponent<Image>().sprite;

            castTime = 3.0f;
            damageDuration = 0.15f;
            duration = castTime + damageDuration;
            isPassive = false;

            damageEffect = (GameObject)Resources.Load("Prefabs/Effects/Minds/MageMind/Pressure_Eff_Burst_2_oneshot");
            damageEffectDuration = 2.0f;

            spCost = 5;
        }
        public override bool CanDoAction(AAnimal target)
        {
            return CanDoActionAboutHPSP(target);
        }
        public override void Action(AAnimal target)
        {
            castTime = (2.0f / target.MovementSpeed);
            damageDuration = 0.15f;
            duration = castTime;
            GameObject damagefield = Instantiate((GameObject)Resources.Load("Prefabs/Utilities/CubeDamageField"));
            damagefield.GetComponent<ADamageField>().SetMainParam(DamageEffect, DamageEffectDuration,0, target.MD, damageDuration, castTime, target.targetPOS, 1);
            damagefield.GetComponent<CubeDamageField>().SetAndAwake();
            SetMotionAndDurationAndUseHPSP(target);

        }
    }

    public class Break_The_Limit : AMindSkill
    {
        public Break_The_Limit()
        {
            actioncode = 0;
            _name = "Break The Limit";
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

            Debug.Log(_name + "!");
            SetMotionAndDurationAndUseHPSP(target);
        }
    }

}
