using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Exhausted : ABuff {

    public override void Awake()
    {
        Duration = 10.0f;
        Sands = 10.0f;
        _name = "Exhausted";
        icon = GetComponent<Image>().sprite;
        typestring = "Buff";
        flavor = "You are exhausted." + "\n" + "Your MovementSpeed is half of original.";
    }
    public override int[] BuffToMainStatus(int[] mains)
    {
        return mains;
    }
    public override float[] BuffToSubStatus(float[] subs)
    {
        subs[5] = subs[5] / 2;
        return subs;
    }
    public override float BuffToHP(float hp) { return hp; }
    public override float BuffToHPOnlyOnce(float hp) { return hp; }
    public override float BuffToSP(float sp) { return sp; }
    public override float BuffToSPOnlyOnce(float sp) { return sp; }

}
