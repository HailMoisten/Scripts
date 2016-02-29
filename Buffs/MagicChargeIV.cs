using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MagicChargeIV : ABuff
{
    public override void Awake()
    {
        _name = "MagicChargeIV";
        base.Awake();
        objectPass = "Prefabs/Buffs/MagicChargeIV";
        Duration = 40.0f;
        flavor = "Elements will help your magic." + "\n" +
            "Gain a half of your MD for 40 seconds.";
    }
    public override float[] BuffToSubStatus(float[] subs)
    {
        subs[1] = subs[1] * 1.5f;
        return subs;
    }
}