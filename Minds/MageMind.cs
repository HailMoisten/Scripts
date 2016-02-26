using System;
using UnityEngine;
using UnityEngine.UI;
using IconAndErrorType;

public class MageMind : AMind {

    public override void Awake()
    {
        base.Awake();
        _name = "Mage"; // const
        flavor = "Mage is a Nuker.";
        prefabPass = "Prefabs/Minds/Mage";
        GrowProficiency(900);
        initSkills();
    }

    private void initSkills()
    {
        // get from savedata
//        GrowProficiency();

        transform.GetChild(0).gameObject.AddComponent<Idle>();
        transform.GetChild(1).gameObject.AddComponent<Pressure>();
        transform.GetChild(2).gameObject.AddComponent<Idle>();
        transform.GetChild(3).gameObject.AddComponent<Idle>();
        transform.GetChild(4).gameObject.AddComponent<Idle>();
        transform.GetChild(5).gameObject.AddComponent<Idle>();
        transform.GetChild(6).gameObject.AddComponent<Idle>();
        transform.GetChild(7).gameObject.AddComponent<Idle>();
        transform.GetChild(8).gameObject.AddComponent<Idle>();
        transform.GetChild(9).gameObject.AddComponent<Idle>();
        transform.GetChild(10).gameObject.AddComponent<Break_The_Limit>();

    }

    public class Pressure : AAction
    {
        public override void Awake()
        {
            base.Awake();
            actioncode = 6;
            _name = "Pressure";
            flavor = "Give 1.0*MD damage as magicdamage.";
            damageEffect = (GameObject)Resources.Load("Prefabs/Effects/Minds/MageMind/Pressure_Eff_Burst_2_oneshot");
            spCost = 3;
            canSelectPosition = true;
            canResize = true;
            skillRange = 8;
            skillScaleOneSideLimit = 10;
            castTime = 2.0f;
            duration = 2.0f;
            isChargeSkill = true;
            chargeLimit = 1;
        }
        public override int CanDoAction(AAnimal myself)
        {
            if (myself.BattleReady) { }
            else { return (int)ErrorTypeList.BattleReady; }
            return CanDoActionAboutHPSP(myself);
        }
        protected override void ChargeAction(AAnimal myself)
        {
            castTime = 2.0f / myself.MovementSpeed;
            duration = CastTime;
            skillScale = (float)Math.Sqrt(SkillScaleVector.x * SkillScaleVector.y * SkillScaleVector.z);
            spCost = Mathf.RoundToInt(3 * skillScale);
            base.ChargeAction(myself);
        }
        public override void Action(AAnimal myself)
        {
            castTime = 2.0f / myself.MovementSpeed;
            duration = CastTime;
            skillScale = (float)Math.Sqrt(SkillScaleVector.x * SkillScaleVector.y * SkillScaleVector.z);
            spCost = Mathf.RoundToInt(3 * skillScale);
            GameObject damagefield = (GameObject)Instantiate(Resources.Load("Prefabs/Utilities/CubeDamageField"), Vector3.zero, Quaternion.identity);
            if (Charged) { damagefield.GetComponent<ADamageField>().SetMainParam(myself, DamageEffect, SkillScaleVector, FieldBuff, 0, myself.MD * 2, DamageDuration, CastTime, myself.targetPOS); }
            else { damagefield.GetComponent<ADamageField>().SetMainParam(myself, DamageEffect, SkillScaleVector, FieldBuff, 0, myself.MD, DamageDuration, CastTime, myself.targetPOS); }
            if (IsChargeSkill) { chargeCount = 0; charged = false; } // Need this. if (IsChargeSkill)
            damagefield.GetComponent<CubeDamageField>().SetAndAwake();
            SetMotionAndDurationAndUseHPSP(myself);
        }

    }

    public class Break_The_Limit : AAction
    {
        public override void Awake()
        {
            base.Awake();
            actioncode = 6;
            _name = "Break The Limit";
            flavor = "Give buff of -Break The Limit- to you.";
            castTime = 5.0f;
            duration = 5.0f;
            sppercentCost = 10;
        }
        public override int CanDoAction(AAnimal myself)
        {
            castTime = 5.0f / myself.MovementSpeed;
            duration = CastTime;
            return CanDoActionAboutHPSP(myself);
        }
        public override void Action(AAnimal myself)
        {
            myself.TakeBuff("Break_The_Limit");
            SetMotionAndDurationAndUseHPSP(myself);
        }
    }

}
