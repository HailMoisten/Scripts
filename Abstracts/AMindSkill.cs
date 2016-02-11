using UnityEngine;
using System.Collections;

public abstract class AMindSkill : AAction {
    protected bool isPassive;
    public bool IsPassive { get { return isPassive; } }
    protected float castTime;
    public float CastTime { get { return castTime; } }
    protected float damageDuration = 0.10f;
    public float DamageDuration { get { return damageDuration; } }
    protected GameObject damageEffect = null;
    public GameObject DamageEffect { get { return damageEffect; } }
    protected float damageEffectDuration = 2.0f;
    public float DamageEffectDuration { get { return damageEffectDuration; } }
    protected GameObject buff = null;
    public GameObject Buff { get { return buff; } }

    // Please definite isPassive at inheriting constracter in addition ACTIONCODE, DURATION and NAME.

    public override ACanvasManager Clicked(Vector3 clickedpos)
    {
        GameObject inst = Instantiate((GameObject)Resources.Load("Prefabs/GUI/PopUpTextCanvas"));
        inst.transform.GetChild(0).GetComponent<RectTransform>().localPosition = clickedpos + new Vector3(64, 64, 0);
        PopUpTextCanvasManager ptcm = inst.GetComponent<PopUpTextCanvasManager>();
        ptcm.Title = Name;
        ptcm.Content = "Duration " + Duration + " sec\n" +
            "CastTime " + CastTime + " sec\n" +
            Flavor;
        return ptcm;
    }

}
