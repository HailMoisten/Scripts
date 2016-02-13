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
