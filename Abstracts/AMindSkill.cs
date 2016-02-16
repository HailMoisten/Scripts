using UnityEngine;
using System.Collections;

public abstract class AMindSkill : AAction {
    protected bool canUseAnyTargetPOS = false;
    public bool CanUseAnyTargetPOS { get { return canUseAnyTargetPOS; } }
    protected bool canUseAnyTargetScale = false;
    public bool CanUseAnyTargetScale { get { return canUseAnyTargetScale; } }
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
    protected int chargeCount = 1; // Inclement a times per second.
    public int ChargeCount { get { return chargeCount; } set { chargeCount = value; } }
    protected int chargeLimit = 1;
    public int ChargeLimit { get { return chargeLimit; } }
    protected bool isCharged = false;
    protected IEnumerator chargedCD(float time)
    {
        isCharged = true;
        yield return new WaitForSeconds(time);
        isCharged = false;
    }
    public void Charge(AAnimal target)
    {
        if (isCharged) { }
        else
        {
            if (ChargeCount >= ChargeLimit)
            {
                charged = true;
                GameObject ef = (GameObject)Instantiate(Resources.Load("Prefabs/Effects/Utilities/Charged"), target.nextPOS + Vector3.up, Quaternion.identity);
                ef.GetComponent<EffectManager>().Go();
                Debug.Log("Charged!");
            }
            else { chargeCount++; target.UseHPSP(0, SPCost, 0, 0); }
            StartCoroutine(chargedCD(CastTime / target.MovementSpeed));
        }
    }

    protected float castTime;
    public float CastTime { get { return castTime; } }
    protected float damageDuration = 0.10f;
    public float DamageDuration { get { return damageDuration; } }
    protected GameObject damageEffect = null;
    public GameObject DamageEffect { get { return damageEffect; } }
    protected GameObject buff = null;
    public GameObject Buff { get { return buff; } }

    // Please definite ACTIONCODE, DURATION and NAME and more at Start.
    public override ACanvasManager Clicked(Vector3 clickedpos)
    {
        GameObject inst = (GameObject)Instantiate(Resources.Load("Prefabs/GUI/PopUpTextCanvas"), Vector3.zero, Quaternion.identity);
        inst.transform.GetChild(0).GetComponent<RectTransform>().localPosition = clickedpos + new Vector3(64, 64, 0);
        PopUpTextCanvasManager ptcm = inst.GetComponent<PopUpTextCanvasManager>();
        ptcm.Title = Name;
        ptcm.Content = "Duration " + Duration + " sec\n" +
            "CastTime " + CastTime + " sec\n" +
            Flavor;
        return ptcm;
    }

}
