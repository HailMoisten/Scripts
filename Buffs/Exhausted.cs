using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Exhausted : ABuff
{
    public override void Awake()
    {
        base.Awake();
        objectPass = "Prefabs/Buffs/Exhausted";
        Duration = 5.0f;
        Sands = 5.0f;
        _name = "Exhausted";
        flavor = "You are exhausted." + "\n" + "Your MovementSpeed is half of original.";
    }
    public override float[] BuffToSubStatus(float[] subs)
    {
        subs[5] = subs[5] / 2;
        return subs;
    }

}
