using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Ablaze : ABuff
{
    public override void Awake()
    {
        _name = "Ablaze";
        base.Awake();
        objectPass = "Prefabs/Buffs/Ablaze";
        Duration = 5.0f;
        flavor = "You got ablaze." + "\n" +
            "Reduce 10% of your AD and MD for 5 seconds.";
    }
    public override float[] BuffToSubStatus(float[] subs)
    {
        subs[0] = subs[0] * 0.9f;
        subs[1] = subs[1] * 0.9f;
        return subs;
    }

}
