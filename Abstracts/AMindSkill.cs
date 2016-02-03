using UnityEngine;
using System.Collections;

public abstract class AMindSkill : AAction {
    protected bool isPassive;
    public bool IsPassive { get { return isPassive; } }

    // Please definite isPassive at inheriting constracter in addition ACTIONCODE, DURATION and NAME.

}
