using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Break_The_Limit : ABuff {

    public override void Awake()
    {
        base.Awake();
        Duration = 60.0f;
        _name = "Break_The_Limit";
        icon = GetComponent<Image>().sprite;
        flavor = "Break your limit." + "\n" + "Gain a half your MaxSP and SP for a while.";
    }
    public override int[] BuffToMainStatus(int[] mains)
    {
        return mains;
    }
    public override float[] BuffToSubStatus(float[] subs)
    {
        subs[12] = subs[12] * 1.5f;
        return subs;
    }
    public override float BuffToHP(float hp) { return hp; }
    public override float BuffToHPOnlyOnce(float hp) { return hp; }
    public override float BuffToSP(float sp) { return sp; }
    public override float BuffToSPOnlyOnce(float sp) { return sp; }

}
