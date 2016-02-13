using System;
using UnityEngine;
using UnityEngine.UI;

public class MageMind : AMind {

    public override void Start()
    {
        _name = "Mage"; // const
        type = "Mind"; // const
        flavor = "Mage is a Nuker.";
        icon = GetComponent<Image>().sprite;
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
        public override void Start()
        {
            actioncode = 5;
            _name = "Pressure";
            flavor = "Give 1.0*MD damage as magicdamage.";
            icon = GetComponent<Image>().sprite;
            damageEffect = (GameObject)Resources.Load("Prefabs/Effects/Minds/MageMind/Pressure_Eff_Burst_2_oneshot");
            spCost = 3;
            canUseAnyTargetPOS = true;
            canUseAnyTargetScale = true;
            skillRange = 8;
            skillScaleOneSideLimit = 10;
            castTime = 2.0f;
            duration = 2.0f;
        }
        public override bool CanDoAction(AAnimal target)
        {
            return CanDoActionAboutHPSP(target);
        }
        public override void Action(AAnimal target)
        {
            skillScale = (float)Math.Sqrt(SkillScaleVector.x * SkillScaleVector.y * SkillScaleVector.z);
            spCost = Mathf.RoundToInt(SPCost * skillScale);
            Debug.Log(skillScale); Debug.Log(SkillScaleVector);
            GameObject damagefield = (GameObject)Instantiate(Resources.Load("Prefabs/Utilities/CubeDamageField"), Vector3.zero, Quaternion.identity);
            damagefield.GetComponent<ADamageField>().SetMainParam(DamageEffect, SkillScaleVector, Buff, 0, target.MD, DamageDuration, CastTime, target.targetPOS);
            damagefield.GetComponent<CubeDamageField>().SetAndAwake();
            SetMotionAndDurationAndUseHPSP(target);
        }
    }

    public class Break_The_Limit : AMindSkill
    {
        public override void Start()
        {
            actioncode = 5;
            _name = "Break The Limit";
            flavor = "Give buff of -Break The Limit- to you.";
            icon = GetComponent<Image>().sprite;
            castTime = 5.0f;
            duration = 5.0f;
            buff = (GameObject)Resources.Load("Prefabs/Buffs/Break_The_Limit");
        }
        public override bool CanDoAction(AAnimal target)
        {
            return canUseAnyTargetPOS;
        }
        public override void Action(AAnimal target)
        {
            //Give a Buff about this.
            GameObject damagefield = (GameObject)Instantiate(Resources.Load("Prefabs/Utilities/CubeDamageField"), Vector3.zero, Quaternion.identity);
            damagefield.GetComponent<ADamageField>().SetMainParam(DamageEffect, SkillScaleVector, Buff, 0, 0, DamageDuration, CastTime, target.nextPOS);
            damagefield.GetComponent<CubeDamageField>().SetAndAwake();
            SetMotionAndDurationAndUseHPSP(target);
        }
    }

}
