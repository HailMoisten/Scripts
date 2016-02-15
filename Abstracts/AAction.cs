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
    protected int actioncode = 0;
    protected float duration = 0.0f; public float Duration { get { return duration; } }
    protected int hpCost = 0; public int HPCost { get { return hpCost; } }
    protected int spCost = 0; public int SPCost { get { return spCost; } }
    protected int hppercentCost = 0; public int HPPercentCost { get { return hppercentCost; } }
    protected int sppercentCost = 0; public int SPPercentCost { get { return sppercentCost; } }

    // have to definite ACTIONCODE, DURATION and NAME at inheriting constracter
    public abstract bool CanDoAction(AAnimal target);
    protected bool CanDoActionAboutHPSP(AAnimal target)
    {
        float hp = target.HP; float sp = target.SP;
        float[] subs = target.GetSubStatus();
        hp = hp - hpCost; sp = sp - spCost;
        hp = hp - Mathf.RoundToInt(target.MaxHP * ((float)hppercentCost / 100));
        sp = sp - Mathf.RoundToInt(target.MaxSP * ((float)sppercentCost / 100));
        if (hp >= 0 && sp >= 0) { return true; }// Break_The_Limit
        return false;
    }
    public abstract void Action(AAnimal target);
    protected void SetMotionAndDurationAndUseHPSP(AAnimal target)
    {
        target.UseHPSP(hpCost, spCost, hppercentCost, sppercentCost);
        target.GetAnimator().SetInteger("ActionCode", actioncode);
        target.StartCoroutine(target.DoingAction(duration));
    }
    public override ACanvasManager Clicked(Vector3 clickedpos)
    {
        GameObject inst = (GameObject)Instantiate(Resources.Load("Prefabs/GUI/PopUpTextCanvas"), Vector3.zero, Quaternion.identity);
        inst.transform.GetChild(0).GetComponent<RectTransform>().localPosition = clickedpos + new Vector3(64, 64, 0);
        PopUpTextCanvasManager ptcm = inst.GetComponent<PopUpTextCanvasManager>();
        ptcm.Title = Name;
        ptcm.Content = "Duration " + Duration + " sec\n" + Flavor;
        return ptcm;
    }

}

// Main Actions
public class IdleAction : AAction
{
    public override void Awake()
    {
        actioncode = 0;
        _name = "Idle";
        duration = 0.0f;
    }

    public override bool CanDoAction(AAnimal target)
    {
        return true;
    }
    public override void Action(AAnimal target)
    {
        duration = target.GCD;
        SetMotionAndDurationAndUseHPSP(target);
    }

}
public class WalkAction : AAction
{
    public override void Awake()
    {
        actioncode = 1;
        _name = "Walk";
        duration = 1.0f;
        spCost = 1;
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
                hitFront.collider.tag == "Animal")
            {
                return false;
            }
        }
        if (Physics.Raycast(target.nextnextPOS + new Vector3(0, 1.5f, 0), -Vector3.up, out hitDown, 3.0f))
        {
            //            Debug.Log("hitDown:" + hitDown.distance);
            if (hitDown.collider.tag == "Environment" ||
                hitDown.collider.tag == "Animal")
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
        if (target.MovementSpeed == 0) { }
        else { duration = (diag) / target.MovementSpeed; }//time
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
    public override void Awake()
    {
        actioncode = 2;
        _name = "Run";
        duration = 1.0f;
        spCost = 2;
    }
    public override bool CanDoAction(AAnimal target)
    {
        // Run fix
        float[] subs = target.GetSubStatus();
        Vector3 dir2 = new Vector3(target.nextnextPOS.x - target.nextPOS.x, 0, target.nextnextPOS.z - target.nextPOS.z);
        dir2 = target.RoundToIntVector3XZ(dir2);
//        dir2 = dir2 * Mathf.RoundToInt(target.RunRatio);
//        target.nextnextPOS = target.nextPOS + dir2;

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
                hitFront.collider.tag == "Animal")
            {
                return false;
            }
        }
        if (Physics.Raycast(target.nextnextPOS + new Vector3(0, 1.5f, 0), -Vector3.up, out hitDown, 3.0f))
        {
            //            Debug.Log("hitDown:" + hitDown.distance);
            if (hitDown.collider.tag == "Environment" ||
                hitDown.collider.tag == "Animal")
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
        if (target.MovementSpeed * target.RunRatio == 0) { }
        else { duration = (diag) / (target.MovementSpeed * target.RunRatio); }
        target.nextPOS = target.nextnextPOS;
        iTween.MoveTo(target.gameObject,
            iTween.Hash("position", target.nextPOS,
            "time", duration,
            "easetype", "linear"
            ));
        SetMotionAndDurationAndUseHPSP(target);
    }

}
