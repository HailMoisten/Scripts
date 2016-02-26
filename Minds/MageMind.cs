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
            actioncode = (int)ActionCodeList.MagicAttack;
            _name = "Pressure";
            flavor = "Give 1.0*MD damage as magicdamage.";
            spCost = 3;
            canSelectPosition = true;
            canResize = true;
            skillRange = 8;
            skillScaleOneSideLimit = 10;
            isChargeSkill = true;
            chargeLimit = 2;

            damageEffect = (GameObject)Resources.Load("Prefabs/Effects/Minds/MageMind/Pressure_Eff_Burst_2_oneshot");
            castTime = 2.0f;
            duration = 2.0f;
        }
        public override int CanDoAction(AAnimal myself)
        {
            if (myself.BattleReady) { }
            else { return (int)ErrorTypeList.BattleReady; }
            return CanDoActionAboutHPSP(myself);
        }
        public override void SetParamsNeedAnimal(AAnimal myself)
        {
            castTime = 2.0f / myself.MovementSpeed;
            duration = CastTime;
            skillScale = (float)Math.Sqrt(SkillScaleVector.x * SkillScaleVector.y * SkillScaleVector.z);
            spCost = Mathf.RoundToInt(3 * skillScale);
        }
        protected override void ChargingAction(AAnimal myself)
        {
            base.ChargingAction(myself);
        }
        public override void Action(AAnimal myself)
        {
            spCost = 0;
            if (Charged) { CreateCubeDamageField(myself, 0, myself.MD * 2, myself.targetPOS); }
            else { CreateCubeDamageField(myself, 0, myself.MD, myself.targetPOS); }
            base.Action(myself);
            SetMotionAndDurationAndUseHPSP(myself);
        }

    }

    public class Break_The_Limit : AAction
    {
        public override void Awake()
        {
            base.Awake();
            actioncode = (int)ActionCodeList.MagicAttack;
            _name = "Break The Limit";
            flavor = "Give buff of -Break The Limit- to you.";
            castTime = 5.0f;
            duration = 5.0f;
            sppercentCost = 10;
        }
        public override int CanDoAction(AAnimal myself)
        {
            return CanDoActionAboutHPSP(myself);
        }
        public override void SetParamsNeedAnimal(AAnimal myself)
        {
            castTime = 5.0f / myself.MovementSpeed;
            duration = CastTime;
        }
        public override void Action(AAnimal myself)
        {
            myself.TakeBuff("Break_The_Limit");
            SetMotionAndDurationAndUseHPSP(myself);
        }
    }

}
