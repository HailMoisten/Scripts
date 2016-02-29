using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Break_The_Limit : ABuff {

    public override void Awake()
    {
        _name = "Break_The_Limit";
        base.Awake();
        objectPass = "Prefabs/Buffs/Break_The_Limit";
        Duration = 50.0f;
        flavor = "Break your limit." + "\n" +
            "Gain a half your MaxSP and SP for a while.";
    }
    public override float[] BuffToSubStatus(float[] subs)
    {
        halfofmaxsp = subs[12] * 0.5f;
        subs[12] = subs[12] * 1.5f;
        return subs;
    }
    private float halfofmaxsp;
    public override float BuffToSPFirst(float sp) { return sp + halfofmaxsp; }
    public override float BuffToSPLast(float sp) { return sp - halfofmaxsp; }

}
