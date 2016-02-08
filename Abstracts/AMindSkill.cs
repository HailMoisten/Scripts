using UnityEngine;
using System.Collections;

public abstract class AMindSkill : AAction {
    protected bool isPassive;
    public bool IsPassive { get { return isPassive; } }
    protected float castTime;
    public float CastTime { get { return castTime; } }

    // Please definite isPassive at inheriting constracter in addition ACTIONCODE, DURATION and NAME.

}
