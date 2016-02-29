using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MagicChargeII : ABuff
{
    public override void Awake()
    {
        _name = "MagicChargeII";
        base.Awake();
        objectPass = "Prefabs/Buffs/MagicChargeII";
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