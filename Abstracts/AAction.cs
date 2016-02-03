using UnityEngine;
using System.Collections;

// Actions what LifeSeeds have.
/*    public interface IAction
    {
        int GetActionCode();
        void Action(LifeSeed target, int actioncode);
    }
*/
public abstract class AAction/* : IAction */
{
    protected int ACTIONCODE = 0; // const
    protected float duration = 0.0f; // const
    protected string NAME = "";
    // have to definite ACTIONCODE, DURATION and NAME at inheriting constracter
    protected AAction() { }
    public abstract void Action(AAnimal target);
    protected void SetMotionAndDuration(AAnimal target)
    {
        target.GetAnimator().SetInteger("ActionCode", ACTIONCODE);
        target.StartCoroutine(target.DoingAction(duration));
    }
}
public class IdleAction : AAction
{
    public IdleAction()
    {
        ACTIONCODE = 0;
        NAME = "Idle";
        duration = 0.0f;
    }
    public override void Action(AAnimal target)
    {
        SetMotionAndDuration(target);
    }
}
public class WalkAction : AAction
{
    public WalkAction()
    {
        ACTIONCODE = 1;
        NAME = "Walk";
        duration = 1.0f;
    }
    public override void Action(AAnimal target)
    {
        float diag = 1.0f;
        if (Mathf.Abs(target.nextPOS.x - target.nextnextPOS.x) != 0 &&
             Mathf.Abs(target.nextPOS.z - target.nextnextPOS.z) != 0) { diag = 1.5f; }
        else { diag = 1.0f; }
        float[] subs = target.GetSubStatus();
        if (subs[5] == 0) { }
        else { duration = (diag) / (subs[5]); }//time
        target.nextPOS = target.nextnextPOS;
        iTween.MoveTo(target.gameObject,
            iTween.Hash("position", target.nextPOS,
            "time", duration,
            "easetype", "linear"
            ));
        SetMotionAndDuration(target);
    }
}
public class RunAction : AAction
{
    public RunAction()
    {
        ACTIONCODE = 2;
        NAME = "Run";
        duration = 1.0f;
    }
    public override void Action(AAnimal target)
    {
        // almost same to WalkAction.Action()
        float diag = 1.0f;
        if (Mathf.Abs(target.nextPOS.x - target.nextnextPOS.x) != 0 &&
             Mathf.Abs(target.nextPOS.z - target.nextnextPOS.z) != 0) { diag = 1.5f; }
        else { diag = 1.0f; }
        float[] subs = target.GetSubStatus();
        if (subs[5] * subs[6] == 0) { }// *(1/RunRatio)
        else { duration = (diag) / (subs[5]/* * target.RunRatio */); }//time
        target.nextPOS = target.nextnextPOS;
        iTween.MoveTo(target.gameObject,
            iTween.Hash("position", target.nextPOS,
            "time", duration,
            "easetype", "linear"
            ));
        SetMotionAndDuration(target);
    }
}
