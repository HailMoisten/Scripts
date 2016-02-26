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
    public virtual int CanDoAction(AAnimal myself)
    {
        return (int)ErrorTypeList.Nothing;
    }
    public abstract void SetParamsNeedAnimal(AAnimal myself);
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
    public virtual void Action(AAnimal myself)
    {
        if (IsChargeSkill)
        {
            chargeCount = 0;
            charged = false;
        }
    }
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
        SetParamsNeedAnimal(myself);
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
            else if (ChargeCount % ChargeSpan == 0) { ChargingAction(myself); }
            myself.UseHPSP(hpCost, spCost, hppercentCost, sppercentCost);

            StartCoroutine(chargedCD(CastTime));
        }
    }
    protected virtual void ChargingAction(AAnimal myself)
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
    public GameObject Buff { get { return fieldBuff; } }
    protected void CreateCubeDamageField(AAnimal myself, int ad, int md, Vector3 pos)
    {
        GameObject cubedamagefield = (GameObject)Instantiate(Resources.Load("Prefabs/Utilities/CubeDamageField"), Vector3.zero, Quaternion.identity);
        cubedamagefield.GetComponent<ADamageField>().SetMainParam(
            myself,
            ad, md, pos,
            DamageEffect,
            SkillScaleVector,
            Buff,
            DamageDuration,
            CastTime);
        cubedamagefield.GetComponent<CubeDamageField>().SetAndAwake();
    }

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