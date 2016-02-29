using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Chilled : ABuff
{
    public override void Awake()
    {
        _name = "Chilled";
        base.Awake();
        objectPass = "Prefabs/Buffs/Chilled";
        Duration = 5.0f;
        flavor = "You got chilled." + "\n" +
            "Down your SPRegen for 5 seconds.";
    }
    public override float[] BuffToSubStatus(float[] subs)
    {
        subs[14] = subs[14] * -1;
        return subs;
    }

}
