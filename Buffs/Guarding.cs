using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Guarding : ABuff
{

    public override void Awake()
    {
        base.Awake();
        objectPass = "Prefabs/Buffs/Guarding";
        IsToggle = true;
        Toggle = true;
        _name = "Guarding";
        flavor = "You are guarding." + "\n" +
            "Gain 67% of your AR and MR.";
    }
    public override float[] BuffToSubStatus(float[] subs)
    {
        subs[2] = subs[2] * 5 / 3;
        subs[3] = subs[3] * 5 / 3;
        return subs;
    }

}
