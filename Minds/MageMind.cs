using System;
using UnityEngine;
using UnityEngine.UI;
using IconAndErrorType;

public class MageMind : AMind {

    public override void Awake()
    {
        _name = "Mage"; // const
        base.Awake();
        flavor = "Mage is a Nuker.";
        prefabPass = "Prefabs/Minds/Mage";
        initSkills();
    }

    private void initSkills()
    {
        transform.GetChild(0).gameObject.AddComponent<Idle>();
        transform.GetChild(1).gameObject.AddComponent<Pressure>();
        transform.GetChild(2).gameObject.AddComponent<MagicCharge>();
        transform.GetChild(3).gameObject.AddComponent<Burn>();
        transform.GetChild(4).gameObject.AddComponent<MagicChargeII>();
        transform.GetChild(5).gameObject.AddComponent<Freeze>();
        transform.GetChild(6).gameObject.AddComponent<MagicChargeIII>();
        transform.GetChild(7).gameObject.AddComponent<Shock>();
        transform.GetChild(8).gameObject.AddComponent<MagicChargeIV>();
        transform.GetChild(9).gameObject.AddComponent<Explosion>();
        transform.GetChild(10).gameObject.AddComponent<Break_The_Limit>();
    }

    private class Pressure : AAction
    {
        private AItem catalyst = null;
        public override void Awake()
        {
            _name = "Pressure";
            base.Awake();
            actioncode = 6;
            flavor = "Give 1.0*MD damage as magicdamage.";
            spCost = 3;
            mindName = "Mage";
            canSelectPosition = true;
            canResize = true;
            skillRange = 8;
            skillScaleOneSideLimit = 10;
            isChargeSkill = true;
            chargeLimit = 2;

            damageEffect = (GameObject)Resources.Load("Prefabs/Effects/Minds/MageMind/Pressure_Eff_Burst_2_oneshot");
        }
        public override int CanDoAction(AAnimal myself)
        {
            if (myself.BattleReady) { }
            else { return (int)ErrorTypeList.BattleReady; }
            if (catalyst != null)
            {
                if (catalyst.Number >= 1) { }
                else { return (int)ErrorTypeList.Number; }
            }
            else { return (int)ErrorTypeList.Catalyst; }
            return CanDoActionAboutHPSP(myself);
        }
        public override void SetParamsNeedAnimal(AAnimal myself)
        {
            profPoint = 1;
            castTime = 2.0f / myself.MovementSpeed;
            duration = CastTime;
            skillScale = (float)Math.Sqrt(SkillScaleVector.x * SkillScaleVector.y * SkillScaleVector.z);
            spCost = Mathf.RoundToInt(3 * skillScale);
            profPoint += Mathf.FloorToInt(SkillScale / 10);
            if (catalyst != null) { }
            else
            {
                if (myself.ItemBag.FindChild("AirShard"))
                {
                    catalyst = myself.ItemBag.FindChild("AirShard").GetComponent<AItem>();
                }
            }
            SkillPOSFix = myself.EyeLevel;
        }
        protected override void ChargingAction(AAnimal myself)
        {
            base.ChargingAction(myself);
        }
        public override void Action(AAnimal myself)
        {
            if (myself.Buffs.FindChild("MagicChargeIV")) { profPoint += 4; }
            else if (myself.Buffs.FindChild("MagicChargeIII")) { profPoint += 3; }
            else if (myself.Buffs.FindChild("MagicChargeII")) { profPoint += 2; }
            else if (myself.Buffs.FindChild("MagicCharge")) { profPoint += 1; }

            if (catalyst.Number <= 0) { }
            else
            {
                catalyst.Materialize(myself.targetPOS);
                if (Charged) { profPoint++; CreateCubeDamageField(myself, 0, Mathf.RoundToInt(myself.MD * 2.0f), myself.targetPOS + SkillPOSFix); }
                else { CreateCubeDamageField(myself, 0, myself.MD, myself.targetPOS + SkillPOSFix); }
                profPoint = 1;
                if (myself.Buffs.FindChild("MagicCharge")) { Destroy(myself.Buffs.FindChild("MagicCharge").gameObject); }
                if (myself.Buffs.FindChild("MagicChargeII")) { Destroy(myself.Buffs.FindChild("MagicChargeII").gameObject); }
                if (myself.Buffs.FindChild("MagicChargeIII")) { Destroy(myself.Buffs.FindChild("MagicChargeIII").gameObject); }
                if (myself.Buffs.FindChild("MagicChargeIV")) { Destroy(myself.Buffs.FindChild("MagicChargeIV").gameObject); }
            }
            base.Action(myself);
            SetMotionAndDurationAndUseHPSP(myself);
        }

    }
    public class MagicCharge : AAction
    {
        public override void Awake()
        {
            _name = "MagicCharge";
            base.Awake();
            actioncode = 6;
            flavor = "You can take buff of -MagicCharge-.";
            sppercentCost = 20;
            mindName = "Mage";
            profPoint = 1;
            castTime = 2.0f;
            duration = 2.0f;
        }
        public override int CanDoAction(AAnimal myself)
        {
            return CanDoActionAboutHPSP(myself);
        }
        public override void SetParamsNeedAnimal(AAnimal myself) { }
        public override void Action(AAnimal myself)
        {
            string buffnum = "";
            if (myself.Mind.FindChild("Mage"))
            {
                AMind mage = myself.Mind.FindChild("Mage").GetComponent<AMind>();
                if (mage.MindLevel >= 4 && myself.Buffs.FindChild("MagicCharge")) { buffnum = "II"; }
                if (mage.MindLevel >= 6 && myself.Buffs.FindChild("MagicChargeII")) { buffnum = "III"; }
                if (mage.MindLevel >= 8 && myself.Buffs.FindChild("MagicChargeIII")) { buffnum = "IV"; }
            }
            else { }
            myself.TakeBuff("MagicCharge" + buffnum);
            GiveProficiency(myself);
            SetMotionAndDurationAndUseHPSP(myself);
        }

    }
    private class Burn : AAction
    {
        private AItem catalyst = null;
        public override void Awake()
        {
            _name = "Burn";
            base.Awake();
            actioncode = 6;
            flavor = "Give 1.0 * MD damage as magicdamage."+"\n"+
                "Give a buff of -Ablaze- to target.";
            spCost = 10;
            mindName = "Mage";
            canSelectPosition = true;
            canResize = true;
            skillRange = 8;
            skillScaleOneSideLimit = 10;

            damageEffect = (GameObject)Resources.Load("Prefabs/Effects/Minds/MageMind/Burn_L_Rose_fire");
            fieldBuffName = "Ablaze";
        }
        public override int CanDoAction(AAnimal myself)
        {
            if (myself.BattleReady) { }
            else { return (int)ErrorTypeList.BattleReady; }
            if (catalyst != null)
            {
                if (catalyst.Number >= 1) { }
                else { return (int)ErrorTypeList.Number; }
            }
            else { return (int)ErrorTypeList.Catalyst; }
            return CanDoActionAboutHPSP(myself);
        }
        public override void SetParamsNeedAnimal(AAnimal myself)
        {
            profPoint = 2;
            castTime = 2.0f / myself.MovementSpeed;
            duration = CastTime;
            skillScale = (float)Math.Sqrt(SkillScaleVector.x * SkillScaleVector.y * SkillScaleVector.z);
            spCost = Mathf.RoundToInt(3 * skillScale);
            profPoint += Mathf.FloorToInt(SkillScale / 10);
            if (myself.ItemBag.FindChild("FireShard"))
            {
                catalyst = myself.ItemBag.FindChild("FireShard").GetComponent<AItem>();
            }
            SkillPOSFix = myself.EyeLevel;
        }
        public override void Action(AAnimal myself)
        {
            if (myself.Buffs.FindChild("MagicChargeIV")) { fieldBuffName += "IV"; profPoint += 4; }
            else if (myself.Buffs.FindChild("MagicChargeIII")) { fieldBuffName += "III"; profPoint += 3; }
            else if (myself.Buffs.FindChild("MagicChargeII")) { fieldBuffName += "II"; profPoint += 2; }
            else if (myself.Buffs.FindChild("MagicCharge")) { profPoint += 1; }

            if (catalyst.Number <= 0) { }
            else
            {
                catalyst.Materialize(myself.targetPOS);
                CreateCubeDamageField(myself, 0, myself.MD, myself.targetPOS + SkillPOSFix);
                if (myself.Buffs.FindChild("MagicCharge")) { Destroy(myself.Buffs.FindChild("MagicCharge").gameObject); }
                if (myself.Buffs.FindChild("MagicChargeII")) { Destroy(myself.Buffs.FindChild("MagicChargeII").gameObject); }
                if (myself.Buffs.FindChild("MagicChargeIII")) { Destroy(myself.Buffs.FindChild("MagicChargeIII").gameObject); }
                if (myself.Buffs.FindChild("MagicChargeIV")) { Destroy(myself.Buffs.FindChild("MagicChargeIV").gameObject); }
            }
            base.Action(myself);
            SetMotionAndDurationAndUseHPSP(myself);
        }

    }

    public class MagicChargeII : AAction
    {
        public override void Awake()
        {
            isPassive = true;
            _name = "MagicChargeII";
            base.Awake();
            actioncode = 6;
            flavor = "You can take buff of -MagicCharge II- after -MagicCharge-.";
            sppercentCost = 20;
            mindName = "Mage";
        }
        public override int CanDoAction(AAnimal myself)
        {
            return (int)ErrorTypeList.IsPassive;
        }
        public override void SetParamsNeedAnimal(AAnimal myself) { }
        public override void Action(AAnimal myself)
        {
            myself.TakeBuff("MagicChargeII");
            SetMotionAndDurationAndUseHPSP(myself);
        }

    }
    public class Freeze : AAction
    {
        private AItem catalyst = null;
        public override void Awake()
        {
            _name = "Freeze";
            base.Awake();
            actioncode = 6;
            flavor = "Give 1.0 * MD damage as magicdamage."+"\n"+
                "Give a buff of -Chilled- to target.";
            spCost = 10;
            mindName = "Mage";
            canSelectPosition = true;
            canResize = true;
            skillRange = 8;
            skillScaleOneSideLimit = 10;

            damageEffect = (GameObject)Resources.Load("Prefabs/Effects/Minds/MageMind/Freeze_Eff_Aura_1_loop");
            fieldBuffName = "Chilled";
        }
        public override int CanDoAction(AAnimal myself)
        {
            if (myself.BattleReady) { }
            else { return (int)ErrorTypeList.BattleReady; }
            if (catalyst != null)
            {
                if (catalyst.Number >= 1) { }
                else { return (int)ErrorTypeList.Number; }
            }
            else { return (int)ErrorTypeList.Catalyst; }
            return CanDoActionAboutHPSP(myself);
        }
        public override void SetParamsNeedAnimal(AAnimal myself)
        {
            profPoint = 3;
            castTime = 2.0f / myself.MovementSpeed;
            duration = CastTime;
            skillScale = (float)Math.Sqrt(SkillScaleVector.x * SkillScaleVector.y * SkillScaleVector.z);
            spCost = Mathf.RoundToInt(3 * skillScale);
            profPoint += Mathf.FloorToInt(SkillScale / 10);
            if (myself.ItemBag.FindChild("IceShard"))
            {
                catalyst = myself.ItemBag.FindChild("IceShard").GetComponent<AItem>();
            }
            SkillPOSFix = myself.EyeLevel;
        }
        public override void Action(AAnimal myself)
        {
            if (myself.Buffs.FindChild("MagicChargeIV")) { fieldBuffName += "IV"; profPoint += 4; }
            else if (myself.Buffs.FindChild("MagicChargeIII")) { fieldBuffName += "III"; profPoint += 3; }
            else if (myself.Buffs.FindChild("MagicChargeII")) { fieldBuffName += "II"; profPoint += 2; }
            else if (myself.Buffs.FindChild("MagicCharge")) { profPoint += 1; }

            if (catalyst.Number <= 0) { }
            else
            {
                catalyst.Materialize(myself.targetPOS);
                CreateCubeDamageField(myself, 0, myself.MD, myself.targetPOS + SkillPOSFix);
                if (myself.Buffs.FindChild("MagicCharge")) { Destroy(myself.Buffs.FindChild("MagicCharge").gameObject); }
                if (myself.Buffs.FindChild("MagicChargeII")) { Destroy(myself.Buffs.FindChild("MagicChargeII").gameObject); }
                if (myself.Buffs.FindChild("MagicChargeIII")) { Destroy(myself.Buffs.FindChild("MagicChargeIII").gameObject); }
                if (myself.Buffs.FindChild("MagicChargeIV")) { Destroy(myself.Buffs.FindChild("MagicChargeIV").gameObject); }
            }
            base.Action(myself);
            SetMotionAndDurationAndUseHPSP(myself);
        }

    }

    public class MagicChargeIII : AAction
    {
        public override void Awake()
        {
            isPassive = true;
            _name = "MagicChargeIII";
            base.Awake();
            actioncode = 6;
            flavor = "You can take buff of -MagicCharge III- after -MagicCharge II-.";
            sppercentCost = 20;
            mindName = "Mage";
        }
        public override int CanDoAction(AAnimal myself)
        {
            return (int)ErrorTypeList.IsPassive;
        }
        public override void SetParamsNeedAnimal(AAnimal myself) { }
        public override void Action(AAnimal myself)
        {
            myself.TakeBuff("MagicChargeIII");
            SetMotionAndDurationAndUseHPSP(myself);
        }

    }
    public class Shock : AAction
    {
        private AItem catalyst = null;
        public override void Awake()
        {
            _name = "Shock";
            base.Awake();
            actioncode = 6;
            flavor = "Give 1.0 * MD damage as magicdamage." + "\n" +
                "Give a buff of -Tender- to target.";
            spCost = 10;
            mindName = "Mage";
            canSelectPosition = true;
            canResize = true;
            skillRange = 8;
            skillScaleOneSideLimit = 10;

            damageEffect = (GameObject)Resources.Load("Prefabs/Effects/Minds/MageMind/Shock_Spark");
            fieldBuffName = "Tender";
        }
        public override int CanDoAction(AAnimal myself)
        {
            if (myself.BattleReady) { }
            else { return (int)ErrorTypeList.BattleReady; }
            if (catalyst != null)
            {
                if (catalyst.Number >= 1) { }
                else { return (int)ErrorTypeList.Number; }
            }
            else { return (int)ErrorTypeList.Catalyst; }
            return CanDoActionAboutHPSP(myself);
        }
        public override void SetParamsNeedAnimal(AAnimal myself)
        {
            profPoint = 3;
            castTime = 2.0f / myself.MovementSpeed;
            duration = CastTime;
            skillScale = (float)Math.Sqrt(SkillScaleVector.x * SkillScaleVector.y * SkillScaleVector.z);
            spCost = Mathf.RoundToInt(3 * skillScale);
            profPoint += Mathf.FloorToInt(SkillScale / 10);
            if (myself.ItemBag.FindChild("LightningShard"))
            {
                catalyst = myself.ItemBag.FindChild("LightningShard").GetComponent<AItem>();
            }
            SkillPOSFix = myself.EyeLevel;
        }
        public override void Action(AAnimal myself)
        {
            if (myself.Buffs.FindChild("MagicChargeIV")) { fieldBuffName += "IV"; profPoint += 4; }
            else if (myself.Buffs.FindChild("MagicChargeIII")) { fieldBuffName += "III"; profPoint += 3; }
            else if (myself.Buffs.FindChild("MagicChargeII")) { fieldBuffName += "II"; profPoint += 2; }
            else if (myself.Buffs.FindChild("MagicCharge")) { profPoint += 1; }

            if (catalyst.Number <= 0) { }
            else
            {
                catalyst.Materialize(myself.targetPOS);
                CreateCubeDamageField(myself, 0, myself.MD, myself.targetPOS + SkillPOSFix);
                if (myself.Buffs.FindChild("MagicCharge")) { Destroy(myself.Buffs.FindChild("MagicCharge").gameObject); }
                if (myself.Buffs.FindChild("MagicChargeII")) { Destroy(myself.Buffs.FindChild("MagicChargeII").gameObject); }
                if (myself.Buffs.FindChild("MagicChargeIII")) { Destroy(myself.Buffs.FindChild("MagicChargeIII").gameObject); }
                if (myself.Buffs.FindChild("MagicChargeIV")) { Destroy(myself.Buffs.FindChild("MagicChargeIV").gameObject); }
            }
            base.Action(myself);
            SetMotionAndDurationAndUseHPSP(myself);
        }

    }
    public class MagicChargeIV : AAction
    {
        public override void Awake()
        {
            isPassive = true;
            _name = "MagicChargeIV";
            base.Awake();
            actioncode = 6;
            flavor = "You can take buff of -MagicCharge IV- after -MagicCharge III-.";
            sppercentCost = 20;
            mindName = "Mage";
        }
        public override int CanDoAction(AAnimal myself)
        {
            return (int)ErrorTypeList.IsPassive;
        }
        public override void SetParamsNeedAnimal(AAnimal myself) { }
        public override void Action(AAnimal myself)
        {
            myself.TakeBuff("MagicChargeIV");
            SetMotionAndDurationAndUseHPSP(myself);
        }

    }
    public class Explosion : AAction
    {
        private AItem catalyst = null;
        public override void Awake()
        {
            _name = "Explosion";
            base.Awake();
            actioncode = 6;
            flavor = "Give 5.0 * MD damage as magicdamage.";
            sppercentCost = 50;
            mindName = "Mage";
            canSelectPosition = true;
            canResize = true;
            skillRange = 8;
            skillScaleOneSideLimit = 20;

            damageEffect = (GameObject)Resources.Load("Prefabs/Effects/Minds/MageMind/Explosion_Eff_Burst_1_oneShot");
        }
        public override int CanDoAction(AAnimal myself)
        {
            if (myself.BattleReady) { }
            else { return (int)ErrorTypeList.BattleReady; }
            if (catalyst != null)
            {
                if (catalyst.Number >= 1) { }
                else { return (int)ErrorTypeList.Number; }
            }
            else { return (int)ErrorTypeList.Catalyst; }
            return CanDoActionAboutHPSP(myself);
        }
        public override void SetParamsNeedAnimal(AAnimal myself)
        {
            profPoint = 5;
            castTime = 2.0f / myself.MovementSpeed;
            duration = CastTime;
            skillScale = (float)Math.Sqrt(SkillScaleVector.x * SkillScaleVector.y * SkillScaleVector.z);
            if (SkillScale >= 50) { skillScale = 50; }
            sppercentCost = 50 + Mathf.RoundToInt(skillScale);
            profPoint += Mathf.FloorToInt(SkillScale / 10);
            if (myself.ItemBag.FindChild("EnergyShard"))
            {
                catalyst = myself.ItemBag.FindChild("EnergyShard").GetComponent<AItem>();
            }
            SkillPOSFix = myself.EyeLevel;
        }
        public override void Action(AAnimal myself)
        {
            if (myself.Buffs.FindChild("MagicChargeIV")) { profPoint += 4; }
            else if (myself.Buffs.FindChild("MagicChargeIII")) { profPoint += 3; }
            else if (myself.Buffs.FindChild("MagicChargeII")) { profPoint += 2; }
            else if (myself.Buffs.FindChild("MagicCharge")) { profPoint += 1; }

            if (catalyst.Number <= 0) { }
            else
            {
                catalyst.Materialize(myself.targetPOS);
                CreateCubeDamageField(myself, 0, myself.MD * 5, myself.targetPOS + SkillPOSFix);
                if (myself.Buffs.FindChild("MagicCharge")) { Destroy(myself.Buffs.FindChild("MagicCharge").gameObject); }
                if (myself.Buffs.FindChild("MagicChargeII")) { Destroy(myself.Buffs.FindChild("MagicChargeII").gameObject); }
                if (myself.Buffs.FindChild("MagicChargeIII")) { Destroy(myself.Buffs.FindChild("MagicChargeIII").gameObject); }
                if (myself.Buffs.FindChild("MagicChargeIV")) { Destroy(myself.Buffs.FindChild("MagicChargeIV").gameObject); }
            }
            base.Action(myself);
            SetMotionAndDurationAndUseHPSP(myself);
        }

    }
    public class Break_The_Limit : AAction
    {
        public override void Awake()
        {
            _name = "Break The Limit";
            base.Awake();
            actioncode = 6;
            flavor = "Give a buff of -Break The Limit- to you.";
            mindName = "Mage";
            castTime = 5.0f;
            duration = 5.0f;
            sppercentCost = 25;
        }
        public override int CanDoAction(AAnimal myself)
        {
            return CanDoActionAboutHPSP(myself);
        }
        public override void SetParamsNeedAnimal(AAnimal myself)
        {
        }
        public override void Action(AAnimal myself)
        {
            myself.TakeBuff("Break_The_Limit");
            SetMotionAndDurationAndUseHPSP(myself);
        }
    }

}
