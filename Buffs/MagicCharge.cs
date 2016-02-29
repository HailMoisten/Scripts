using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MagicCharge : ABuff
{
    public override void Awake()
    {
        _name = "MagicCharge";
        base.Awake();
        objectPass = "Prefabs/Buffs/MagicCharge";
        Duration = 10.0f;
        flavor = "Elements will help your magic." + "\n" +
            "Gain a half of your MD for 10 seconds.";
    }
    public override float[] BuffToSubStatus(float[] subs)
    {
        subs[1] = subs[1] * 1.5f;
        return subs;
    }
}