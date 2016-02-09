using UnityEngine;
using System.Collections;

public class Exhausted : ABuff {

    public override int[] BuffToMainStatus(int[] mains)
    {
        return mains;
    }
    public override float[] BuffToSubStatus(float[] subs)
    {
        subs[5] = subs[5] / 2;
        IsUsed = true;
        return subs;
    }
    public Exhausted()
    {
        duration = 10.0f;
        _name = "Exhausted";
        type = "Buff";
        flavor = "You are exhausted." + "\n" + "Your MovementSpeed is half of original.";
    }

}
