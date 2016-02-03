using UnityEngine;
using System.Collections;

// Actions what LifeSeeds have.
/*    public interface IAction
    {
        int GetActionCode();
        void Action(LifeSeed target, int actioncode);
    }
*/
public abstract class AAction : AIcon
{
    protected int ACTIONCODE = 0;
    protected float duration = 0.0f;
    public float Duration { get { return duration; } }
    protected int hpCost = 0;
    public int HPCost { get { return hpCost; } }
    protected int spCost = 0;
    public int SPCost { get { return spCost; } }
    protected int hppercentCost = 0;
    public int HPPercentCost { get { return hppercentCost; } }
    protected int sppercentCost = 0;
    public int SPPercentCost { get { return sppercentCost; } }

    // have to definite ACTIONCODE, DURATION and NAME at inheriting constracter
    protected AAction() { }
    public abstract bool CanDoAction(AAnimal target);
    protected bool CanDoActionAboutHPSP(AAnimal target)
    {
        int hp = target.GetHP(); int sp = target.GetSP();
        float[] subs = target.GetSubStatus();
        hp = hp - hpCost; sp = sp - spCost;
        hp = hp - Mathf.RoundToInt(subs[9] * ((float)hppercentCost / 100));
        sp = sp - Mathf.RoundToInt(subs[12] * ((float)sppercentCost / 100));
        if (hp >= 0 && sp >= subs[14]) { return true; } 
        return false;
    }
    public abstract void Action(AAnimal target);
    protected void SetMotionAndDurationAndUseHPSP(AAnimal target)
    {
        target.UseHPSP(hpCost, spCost, hppercentCost, sppercentCost);
        target.GetAnimator().SetInteger("ActionCode", ACTIONCODE);
        target.StartCoroutine(target.DoingAction(duration));
    }
    public override ACanvasManager Clicked(Vector3 clickedpos)
    {
        GameObject inst = Instantiate((GameObject)Resources.Load("Prefabs/GUI/PopUpTextCanvas"));
        inst.transform.GetChild(0).GetComponent<RectTransform>().localPosition = clickedpos + new Vector3(64, 64, 0);
        PopUpTextCanvasManager ptcm = inst.GetComponent<PopUpTextCanvasManager>();
        ptcm.Title = Name;
        ptcm.Content = Flavor;
        return ptcm;
    }

}

// Main Actions
public class IdleAction : AAction
{
    public IdleAction()
    {
        ACTIONCODE = 0;
        NAME = "Idle";
        duration = 0.0f;
    }
    public override bool CanDoAction(AAnimal target)
    {
        return true;
    }
    public override void Action(AAnimal target)
    {
        SetMotionAndDurationAndUseHPSP(target);
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
    public override bool CanDoAction(AAnimal target)
    {
        Vector3 dir2 = new Vector3(target.nextnextPOS.x - target.nextPOS.x, 0, target.nextnextPOS.z - target.nextPOS.z);
        dir2 = target.RoundToIntVector3XZ(dir2);
        float maxd = 1.0f;
        if (dir2.x != 0 && dir2.z != 0) { maxd = 1.5f; }
        RaycastHit hitFront; RaycastHit hitDown;
        Ray rayFront = new Ray(target.nextPOS + new Vector3(0, 1.5f, 0), dir2);
        Ray rayDown = new Ray(target.nextnextPOS + new Vector3(0, 1.5f, 0), new Vector3(0, -1, 0));
        if (Physics.Raycast(target.nextPOS + new Vector3(0, 1.5f, 0), dir2, out hitFront, maxd))
        {
            //            Debug.Log("hitsFront:" + hitFront.distance);
            if (hitFront.collider.tag == "Terrain" ||
                hitFront.collider.tag == "Environment" ||
                hitFront.collider.tag == "LifeSeed")
            {
                return false;
            }
        }
        if (Physics.Raycast(target.nextnextPOS + new Vector3(0, 1.5f, 0), -Vector3.up, out hitDown, 3.0f))
        {
            //            Debug.Log("hitDown:" + hitDown.distance);
            if (hitDown.collider.tag == "Environment" ||
                hitDown.collider.tag == "LifeSeed")
            {
                target.nextnextPOS.y = target.nextnextPOS.y + 1.5f - hitDown.distance;
            }
            else if (hitDown.collider.tag == "Terrain")
            {
                target.nextnextPOS.y = Terrain.activeTerrain.SampleHeight(target.nextnextPOS);
            }
            return true;
        }
        return false;
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
        SetMotionAndDurationAndUseHPSP(target);
    }

}
public class RunAction : AAction
{
    public RunAction()
    {
        ACTIONCODE = 2;
        NAME = "Run";
        duration = 1.0f;
        spCost = 1;
    }
    public override bool CanDoAction(AAnimal target)
    {
        // Run fix
        float[] subs = target.GetSubStatus();
        Vector3 dir2 = new Vector3(target.nextnextPOS.x - target.nextPOS.x, 0, target.nextnextPOS.z - target.nextPOS.z);
        dir2 = target.RoundToIntVector3XZ(dir2);
        dir2 = dir2 * Mathf.RoundToInt(subs[6]);
        target.nextnextPOS = target.nextPOS + dir2;

        float maxd = 1.0f;
        if (dir2.x != 0 && dir2.z != 0) { maxd = 1.5f; }
        RaycastHit hitFront; RaycastHit hitDown;
        Ray rayFront = new Ray(target.nextPOS + new Vector3(0, 1.5f, 0), dir2);
        Ray rayDown = new Ray(target.nextnextPOS + new Vector3(0, 1.5f, 0), new Vector3(0, -1, 0));
        if (Physics.Raycast(target.nextPOS + new Vector3(0, 1.5f, 0), dir2, out hitFront, maxd))
        {
            //            Debug.Log("hitsFront:" + hitFront.distance);
            if (hitFront.collider.tag == "Terrain" ||
                hitFront.collider.tag == "Environment" ||
                hitFront.collider.tag == "LifeSeed")
            {
                return false;
            }
        }
        if (Physics.Raycast(target.nextnextPOS + new Vector3(0, 1.5f, 0), -Vector3.up, out hitDown, 3.0f))
        {
            //            Debug.Log("hitDown:" + hitDown.distance);
            if (hitDown.collider.tag == "Environment" ||
                hitDown.collider.tag == "LifeSeed")
            {
                target.nextnextPOS.y = target.nextnextPOS.y + 1.5f - hitDown.distance;
            }
            else if (hitDown.collider.tag == "Terrain")
            {
                target.nextnextPOS.y = Terrain.activeTerrain.SampleHeight(target.nextnextPOS);
            }
            return true;
        }
        return false;
    }
    public override void Action(AAnimal target)
    {
        // almost same to WalkAction.Action()
        float diag = 1.0f;
        if (Mathf.Abs(target.nextPOS.x - target.nextnextPOS.x) != 0 &&
             Mathf.Abs(target.nextPOS.z - target.nextnextPOS.z) != 0) { diag = 1.5f; }
        else { diag = 1.0f; }
        float[] subs = target.GetSubStatus();
        if (subs[5] * subs[6] == 0) { }
        else { duration = (diag) / (subs[5]); }
        target.nextPOS = target.nextnextPOS;
        iTween.MoveTo(target.gameObject,
            iTween.Hash("position", target.nextPOS,
            "time", duration,
            "easetype", "linear"
            ));
        SetMotionAndDurationAndUseHPSP(target);
    }

}
