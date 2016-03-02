using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MagicChargeIII : ABuff
{
    public override void Awake()
    {
        _name = "MagicChargeIII";
        base.Awake();
        objectPass = "Prefabs/Buffs/MagicChargeIII";
        Duration = 20.0f;
        flavor = "Elements will help your magic." + "\n" +
            "Gain a half of your MD for 20 seconds.";
    }
    public override float[] BuffToSubStatus(float[] subs)
    {
        subs[1] = subs[1] * 1.5f;
        return subs;
    }
}