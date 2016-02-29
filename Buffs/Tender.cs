using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Tender : ABuff
{
    public override void Awake()
    {
        _name = "Tender";
        base.Awake();
        objectPass = "Prefabs/Buffs/Tender";
        Duration = 5.0f;
        flavor = "You got Tender." + "\n" +
            "Reduce 10% of your AR and MR for 5 seconds.";
    }
    public override float[] BuffToSubStatus(float[] subs)
    {
        subs[2] = subs[2] * 0.9f;
        subs[3] = subs[3] * 0.9f;
        return subs;
    }

}
