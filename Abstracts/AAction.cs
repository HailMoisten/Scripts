using UnityEngine;
using System.Collections;
using IconAndErrorType;

// Actions what LifeSeeds have.
/*    public interface IAction
    {
        int GetActionCode();
        void Action(LifeSeed target, int actioncode);
    }
*/
public abstract class AAction : AIcon
{
    public override void Awake()
    {
        base.Awake();
        IconType = (int)IconTypeList.Action;
        gameObject.tag = "Action";
    }

    protected int actioncode = 0;
    protected float duration = 0.0f; public float Duration { get { return duration; } }
    protected int hpCost = 0; public int HPCost { get { return hpCost; } }
    protected int spCost = 0; public int SPCost { get { return spCost; } }
    protected int hppercentCost = 0; public int HPPercentCost { get { return hppercentCost; } }
    protected int sppercentCost = 0; public int SPPercentCost { get { return sppercentCost; } }

    // have to definite ACTIONCODE, DURATION and NAME at inheriting constracter
    /// <summary>
    /// 0: I can do it.
    /// others: ErrorNumber
    /// </summary>
    /// <param name="myself"></param>
    /// <returns></returns>
    public abstract int CanDoAction(AAnimal myself);
    protected int CanDoActionAboutHPSP(AAnimal myself)
    {
        float hp = myself.HP; float sp = myself.SP;
        float[] subs = myself.GetSubStatus();
        hp = hp - hpCost; sp = sp - spCost;
        hp = hp - Mathf.RoundToInt(myself.MaxHP * ((float)hppercentCost / 100));
        sp = sp - Mathf.RoundToInt(myself.MaxSP * ((float)sppercentCost / 100));
        if (hp >= 0 && sp >= 0) { return (int)ErrorTypeList.Nothing; }// Break_The_Limit
        return (int)ErrorTypeList.HPSP;
    }
    public abstract void Action(AAnimal myself);
    protected void SetMotionAndDurationAndUseHPSP(AAnimal myself)
    {
        myself.UseHPSP(hpCost, spCost, hppercentCost, sppercentCost);
        myself.GetAnimator().SetInteger("ActionCode", actioncode);
        myself.StartCoroutine(myself.DoingAction(duration));
    }

    protected bool canSelectPosition = false;
    public bool CanSelectPosition { get { return canSelectPosition; } }
    protected Vector3 skillPOSVector = Vector3.zero;
    public Vector3 SkillPOSVector { get { return skillPOSVector; } set { skillPOSVector = value; } }
    protected bool canResize = false;
    public bool CanResize { get { return canResize; } }
    protected Vector3 skillScaleVector = Vector3.one;
    public Vector3 SkillScaleVector { get { return skillScaleVector; } set { skillScaleVector = value; } }
    protected float skillScale = 1.0f;
    public float SkillScale { get { return skillScale; } }
    protected int skillScaleOneSideLimit = 1;
    public int SkillScaleOneSideLimit { get { return skillScaleOneSideLimit; } }
    protected int skillRange = 1;
    public int SkillRange { get { return skillRange; } }
    protected bool isChargeSkill = false;
    public bool IsChargeSkill { get { return isChargeSkill; } }
    protected bool charged = false;
    public bool Charged { get { return charged; } }
    protected int chargeCount = 1; // Inclement a times per casttime/movementspeed.
    public int ChargeCount { get { return chargeCount; } set { chargeCount = value; } }
    protected int chargeSpan = 1;
    public int ChargeSpan { get { return chargeSpan; } }
    protected int chargeLimit = 1;
    public int ChargeLimit { get { return chargeLimit; } }
    protected bool isCharged = false;
    protected IEnumerator chargedCD(float time)
    {
        isCharged = true;
        yield return new WaitForSeconds(time);
        isCharged = false;
    }
    public void Charge(AAnimal myself)
    {
        if (isCharged) { }
        else
        {
            chargeCount++;
            myself.GetAnimator().SetInteger("ActionCode", actioncode);
            if (ChargeCount >= ChargeLimit)
            {
                charged = true;
                GameObject ef = (GameObject)Instantiate(Resources.Load("Prefabs/Effects/Utilities/Charged"), myself.nextPOS + Vector3.up, Quaternion.identity);
                ef.GetComponent<EffectManager>().Go();
                Debug.Log("Charged!");
            }
            else if (ChargeCount % ChargeSpan == 0) { ChargeAction(myself); }
            myself.UseHPSP(hpCost, spCost, hppercentCost, sppercentCost);

            StartCoroutine(chargedCD(CastTime));
        }
    }
    protected virtual void ChargeAction(AAnimal myself)
    {
        GameObject ef = (GameObject)Instantiate(Resources.Load("Prefabs/Effects/Utilities/Charging"), myself.nextPOS + Vector3.up, Quaternion.identity);
        ef.GetComponent<EffectManager>().Go();
    }
    protected float castTime;
    public float CastTime { get { return castTime; } }
    protected float damageDuration = 0.10f;
    public float DamageDuration { get { return damageDuration; } }
    protected GameObject damageEffect = null;
    public GameObject DamageEffect { get { return damageEffect; } }
    protected GameObject fieldBuff = null;
    public GameObject FieldBuff { get { return fieldBuff; } }

    public override ACanvasManager Clicked(Vector3 clickedpos)
    {
        GameObject inst = Instantiate((GameObject)Resources.Load("Prefabs/GUI/PopUpActionCanvas"));
        inst.transform.GetChild(0).GetComponent<RectTransform>().localPosition = clickedpos + new Vector3(60, 60, 0);
        PopUpIconCanvasManager ptcm = inst.GetComponent<PopUpIconCanvasManager>();
        ptcm.Title = Name;
        ptcm.Icon = Icon;
        ptcm.Content = "Resize " + CanResize + ", OneSideLimit " + SkillScaleOneSideLimit + "\n" +
            "Position " + CanSelectPosition + ", Range " + SkillRange + "\n" +
            "Charge " + IsChargeSkill + ", Limit " + ChargeLimit + "\n" + 
            "Duration " + Duration + " sec";
        ptcm.Flavor = Flavor;
        return ptcm;
    }

}

// Main Actions
public class Idle : AAction
{
    public override void Awake()
    {
        base.Awake();
        actioncode = 0;
        _name = "Idle";
        duration = 0.0f;
    }

    public override int CanDoAction(AAnimal myself)
    {
        return (int)ErrorTypeList.Nothing;
    }
    public override void Action(AAnimal myself)
    {
        duration = myself.GCD;
        SetMotionAndDurationAndUseHPSP(myself);
    }

}
public class Walk : AAction
{
    public override void Awake()
    {
        base.Awake();
        actioncode = 1;
        _name = "Walk";
        duration = 1.0f;
        spCost = 1;
    }
    public override int CanDoAction(AAnimal myself)
    {
        Vector3 dir2 = new Vector3(myself.nextnextPOS.x - myself.nextPOS.x, 0, myself.nextnextPOS.z - myself.nextPOS.z);
        dir2 = myself.RoundToIntVector3XZ(dir2);
        float maxd = 1.0f;
        if (dir2.x != 0 && dir2.z != 0) { maxd = 1.5f; }
        RaycastHit hitFront; RaycastHit hitDown;
        Ray rayFront = new Ray(myself.nextPOS + new Vector3(0, 1.5f, 0), dir2);
        Ray rayDown = new Ray(myself.nextnextPOS + new Vector3(0, 1.5f, 0), new Vector3(0, -1, 0));
        if (Physics.Raycast(rayFront, out hitFront, maxd))
        {
            //            Debug.Log("hitsFront:" + hitFront.distance);
            if (hitFront.collider.gameObject.layer == LayerMask.NameToLayer("Terrain") ||
                hitFront.collider.gameObject.layer == LayerMask.NameToLayer("Environment") ||
                hitFront.collider.gameObject.layer == LayerMask.NameToLayer("Animal"))
            {
                return (int)ErrorTypeList.Move;
            }
        }
        if (Physics.Raycast(rayDown, out hitDown, 3.0f))
        {
            //            Debug.Log("hitDown:" + hitDown.distance);
            if (hitDown.collider.gameObject.layer == LayerMask.NameToLayer("Environment") ||
                hitDown.collider.gameObject.layer == LayerMask.NameToLayer("Animal"))
            {
                myself.nextnextPOS = myself.nextnextPOS + new Vector3(0, 1.5f - hitDown.distance, 0);
            }
            else if (hitDown.collider.gameObject.layer == LayerMask.NameToLayer("Terrain"))
            {
                myself.nextnextPOS = myself.nextnextPOS + new Vector3(0, Terrain.activeTerrain.SampleHeight(myself.nextnextPOS) - myself.nextnextPOS.y, 0);
            }
            return (int)ErrorTypeList.Nothing;
        }
        return (int)ErrorTypeList.Move;
    }
    public override void Action(AAnimal myself)
    {
        float diag = 1.0f;
        if (Mathf.Abs(myself.nextPOS.x - myself.nextnextPOS.x) != 0 &&
             Mathf.Abs(myself.nextPOS.z - myself.nextnextPOS.z) != 0) { diag = 1.5f; }
        else { diag = 1.0f; }
        if (myself.MovementSpeed == 0) { }
        else { duration = (diag) / myself.MovementSpeed; }//time
        myself.POS = myself.nextPOS;
        myself.nextPOS = myself.nextnextPOS;
        iTween.MoveTo(myself.gameObject,
            iTween.Hash("position", myself.nextPOS,
            "time", duration,
            "easetype", "linear"
            ));
        SetMotionAndDurationAndUseHPSP(myself);
    }

}
public class Run : AAction
{
    public override void Awake()
    {
        base.Awake();
        actioncode = 2;
        _name = "Run";
        duration = 1.0f;
        spCost = 2;
    }
    public override int CanDoAction(AAnimal myself)
    {
        // Run fix
        float[] subs = myself.GetSubStatus();
        Vector3 dir2 = new Vector3(myself.nextnextPOS.x - myself.nextPOS.x, 0, myself.nextnextPOS.z - myself.nextPOS.z);
        dir2 = myself.RoundToIntVector3XZ(dir2);
//        dir2 = dir2 * Mathf.RoundToInt(target.RunRatio);
//        target.nextnextPOS = target.nextPOS + dir2;

        float maxd = 1.0f;
        if (dir2.x != 0 && dir2.z != 0) { maxd = 1.5f; }
        RaycastHit hitFront; RaycastHit hitDown;
        Ray rayFront = new Ray(myself.nextPOS + new Vector3(0, 1.5f, 0), dir2);
        Ray rayDown = new Ray(myself.nextnextPOS + new Vector3(0, 1.5f, 0), new Vector3(0, -1, 0));
        if (Physics.Raycast(rayFront, out hitFront, maxd))
        {
            //            Debug.Log("hitsFront:" + hitFront.distance);
            if (hitFront.collider.gameObject.layer == LayerMask.NameToLayer("Terrain") ||
                hitFront.collider.gameObject.layer == LayerMask.NameToLayer("Environment") ||
                hitFront.collider.gameObject.layer == LayerMask.NameToLayer("Animal"))
            {
                return (int)ErrorTypeList.Move;
            }
        }
        if (Physics.Raycast(rayDown, out hitDown, 3.0f))
        {
            //            Debug.Log("hitDown:" + hitDown.distance);
            if (hitDown.collider.gameObject.layer == LayerMask.NameToLayer("Environment") ||
                hitDown.collider.gameObject.layer == LayerMask.NameToLayer("Animal"))
            {
                myself.nextnextPOS = myself.nextnextPOS + new Vector3(0, 1.5f - hitDown.distance, 0);
            }
            else if (hitDown.collider.gameObject.layer == LayerMask.NameToLayer("Terrain"))
            {
                myself.nextnextPOS = myself.nextnextPOS + new Vector3(0, Terrain.activeTerrain.SampleHeight(myself.nextnextPOS) - myself.nextnextPOS.y, 0);
            }
            return (int)ErrorTypeList.Nothing;
        }
        return (int)ErrorTypeList.Move;
    }
    public override void Action(AAnimal myself)
    {
        // almost same to WalkAction.Action()
        float diag = 1.0f;
        if (Mathf.Abs(myself.nextPOS.x - myself.nextnextPOS.x) != 0 &&
             Mathf.Abs(myself.nextPOS.z - myself.nextnextPOS.z) != 0) { diag = 1.5f; }
        else { diag = 1.0f; }
        float[] subs = myself.GetSubStatus();
        if (myself.MovementSpeed * myself.RunRatio == 0) { }
        else { duration = (diag) / (myself.MovementSpeed * myself.RunRatio); }
        myself.POS = myself.nextPOS;
        myself.nextPOS = myself.nextnextPOS;
        iTween.MoveTo(myself.gameObject,
            iTween.Hash("position", myself.nextPOS,
            "time", duration,
            "easetype", "linear"
            ));
        SetMotionAndDurationAndUseHPSP(myself);
    }

}
public class Attack : AAction
{
    public override void Awake()
    {
        base.Awake();
        actioncode = 4;
        _name = "Attack";
        duration = 1.0f;
        spCost = 2;
    }

    public override int CanDoAction(AAnimal myself)
    {
        if (myself.BattleReady) { }
        else { return (int)ErrorTypeList.BattleReady; }
        return (int)ErrorTypeList.Nothing;
    }
    public override void Action(AAnimal myself)
    {
        duration = 1.0f / myself.MovementSpeed;
        GameObject damagefield = (GameObject)Instantiate(Resources.Load("Prefabs/Utilities/CubeDamageField"), Vector3.zero, Quaternion.identity);
        damagefield.GetComponent<ADamageField>().SetMainParam(myself, DamageEffect, SkillScaleVector, FieldBuff, myself.AD, 0, DamageDuration, CastTime, myself.targetPOS);
        damagefield.GetComponent<CubeDamageField>().SetAndAwake();
        SetMotionAndDurationAndUseHPSP(myself); SetMotionAndDurationAndUseHPSP(myself);
    }
}
public class Guard : AAction
{
    public override void Awake()
    {
        base.Awake();
        actioncode = 5;
        _name = "Guard";
        castTime = 1.0f;
        duration = 1.0f;
        spCost = 2;
        isChargeSkill = true;
        chargeSpan = 1;
        chargeLimit = 64;
    }
    public override int CanDoAction(AAnimal myself)
    {
        castTime = 1.0f / myself.MovementSpeed;
        duration = CastTime;
        return (int)ErrorTypeList.Nothing;
    }
    protected override void ChargeAction(AAnimal myself)
    {
        castTime = 1.0f / myself.MovementSpeed;
        duration = CastTime;
        myself.TakeBuff("Guarding");
    }
    public override void Action(AAnimal myself)
    {
        myself.RemoveBuff("Guarding");
        SetMotionAndDurationAndUseHPSP(myself);
    }
}

public class PickUp : AAction
{
    public AItem TargetItem { get; set; }
    public override void Awake()
    {
        base.Awake();
        actioncode = -2;
        _name = "PickUp";
        duration = 0.5f;
    }

    public override int CanDoAction(AAnimal myself)
    {
        Vector3 diff = myself.nextPOS - TargetItem.transform.position;
        if (Mathf.Abs(diff.x) < 1.5f && Mathf.Abs(diff.y) < 1.5f && Mathf.Abs(diff.z) < 1.5f)
        { return (int)ErrorTypeList.Nothing; }
        return -1;
    }
    public override void Action(AAnimal myself)
    {
        if (myself.ItemBag.FindChild(TargetItem.Name))
        {
            Transform t = myself.ItemBag.FindChild(TargetItem.Name);
            if (t.GetComponent<AItem>().CanTogether)
            {
                t.GetComponent<AItem>().Number += TargetItem.Number;
            }
        }
        else
        {
            GameObject item = new GameObject(TargetItem.Name);
            item.AddComponent(TargetItem.GetType());
            item.transform.SetParent(myself.ItemBag.transform);
        }
        TargetItem.PickUp();
        duration = 0.5f / myself.MovementSpeed;
        TargetItem = null;
        SetMotionAndDurationAndUseHPSP(myself);
    }

}

public class Stunned : AAction
{
    public override void Awake()
    {
        base.Awake();
        actioncode = -1;
        _name = "Stunned";
        duration = 1.0f;
    }

    public override int CanDoAction(AAnimal myself)
    {
        return (int)ErrorTypeList.Nothing;
    }
    public override void Action(AAnimal myself)
    {
        duration = 1.0f / myself.MovementSpeed;
        SetMotionAndDurationAndUseHPSP(myself);
        myself.Interrupting = false;
    }

}