using UnityEngine;
using System.Collections;
using IconType;

// Actions what LifeSeeds have.
/*    public interface IAction
    {
        int GetActionCode();
        void Action(LifeSeed target, int actioncode);
    }
*/
public abstract class AAction : AIcon
{
    public override void Awake()
    {
        base.Awake();
        IconType = (int)IconTypeList.Action;
        gameObject.tag = "Action";
    }

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

    protected bool canSelectPosition = false;
    public bool CanSelectPosition { get { return canSelectPosition; } }
    protected bool canResize = false;
    public bool CanResize { get { return canResize; } }
    protected Vector3 skillScaleVector = Vector3.one;
    public Vector3 SkillScaleVector { get { return skillScaleVector; } set { skillScaleVector = value; } }
    protected float skillScale = 1.0f;
    public float SkillScale { get { return skillScale; } }
    protected int skillScaleOneSideLimit = 1;
    public int SkillScaleOneSideLimit { get { return skillScaleOneSideLimit; } }
    protected int skillRange = 1;
    public int SkillRange { get { return skillRange; } }
    protected bool isChargeSkill = false;
    public bool IsChargeSkill { get { return isChargeSkill; } }
    protected bool charged = false;
    public bool Charged { get { return charged; } }
    protected int chargeCount = 1; // Inclement a times per second.
    public int ChargeCount { get { return chargeCount; } set { chargeCount = value; } }
    protected int chargeLimit = 1;
    public int ChargeLimit { get { return chargeLimit; } }
    protected bool isCharged = false;
    protected IEnumerator chargedCD(float time)
    {
        isCharged = true;
        yield return new WaitForSeconds(time);
        isCharged = false;
    }
    public void Charge(AAnimal target)
    {
        if (isCharged) { }
        else
        {
            if (ChargeCount >= ChargeLimit)
            {
                charged = true;
                GameObject ef = (GameObject)Instantiate(Resources.Load("Prefabs/Effects/Utilities/Charged"), target.nextPOS + Vector3.up, Quaternion.identity);
                ef.GetComponent<EffectManager>().Go();
                Debug.Log("Charged!");
            }
            else { chargeCount++; target.UseHPSP(0, SPCost, 0, 0); }
            StartCoroutine(chargedCD(CastTime / target.MovementSpeed));
        }
    }

    protected float castTime;
    public float CastTime { get { return castTime; } }
    protected float damageDuration = 0.10f;
    public float DamageDuration { get { return damageDuration; } }
    protected GameObject damageEffect = null;
    public GameObject DamageEffect { get { return damageEffect; } }
    protected GameObject buff = null;
    public GameObject Buff { get { return buff; } }

    public override ACanvasManager Clicked(Vector3 clickedpos)
    {
        GameObject inst = Instantiate((GameObject)Resources.Load("Prefabs/GUI/PopUpActionCanvas"));
        inst.transform.GetChild(0).GetComponent<RectTransform>().localPosition = clickedpos + new Vector3(60, 60, 0);
        PopUpIconCanvasManager ptcm = inst.GetComponent<PopUpIconCanvasManager>();
        ptcm.Title = Name;
        ptcm.Icon = Icon;
        ptcm.Content = "Resize " + CanResize + ", OneSideLimit " + SkillScaleOneSideLimit + "\n" +
            "Position " + CanSelectPosition + ", Range " + SkillRange + "\n" +
            "Charge " + IsChargeSkill + ", Limit " + ChargeLimit + "\n" + 
            "Duration " + Duration + " sec";
        ptcm.Flavor = Flavor;
        return ptcm;
    }

}

// Main Actions
public class IdleAction : AAction
{
    public override void Awake()
    {
        base.Awake();
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
        base.Awake();
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
        if (Physics.Raycast(rayFront, out hitFront, maxd))
        {
            //            Debug.Log("hitsFront:" + hitFront.distance);
            if (hitFront.collider.tag == "Terrain" ||
                hitFront.collider.tag == "Environment" ||
                hitFront.collider.tag == "Animal")
            {
                return false;
            }
        }
        if (Physics.Raycast(rayDown, out hitDown, 3.0f))
        {
            //            Debug.Log("hitDown:" + hitDown.distance);
            if (hitDown.collider.tag == "Environment" ||
                hitDown.collider.tag == "Animal")
            {
                target.nextnextPOS = target.nextnextPOS + new Vector3(0, 1.5f - hitDown.distance, 0);
            }
            else if (hitDown.collider.tag == "Terrain")
            {
                target.nextnextPOS = target.nextnextPOS + new Vector3(0, Terrain.activeTerrain.SampleHeight(target.nextnextPOS) - target.nextnextPOS.y, 0);
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
        target.POS = target.nextPOS;
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
        base.Awake();
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
        if (Physics.Raycast(rayFront, out hitFront, maxd))
        {
            //            Debug.Log("hitsFront:" + hitFront.distance);
            if (hitFront.collider.tag == "Terrain" ||
                hitFront.collider.tag == "Environment" ||
                hitFront.collider.tag == "Animal")
            {
                return false;
            }
        }
        if (Physics.Raycast(rayDown, out hitDown, 3.0f))
        {
            //            Debug.Log("hitDown:" + hitDown.distance);
            if (hitDown.collider.tag == "Environment" ||
                hitDown.collider.tag == "Animal")
            {
                target.nextnextPOS = target.nextnextPOS + new Vector3(0, 1.5f - hitDown.distance, 0);
            }
            else if (hitDown.collider.tag == "Terrain")
            {
                target.nextnextPOS = target.nextnextPOS + new Vector3(0, Terrain.activeTerrain.SampleHeight(target.nextnextPOS) - target.nextnextPOS.y, 0);
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
        target.POS = target.nextPOS;
        target.nextPOS = target.nextnextPOS;
        iTween.MoveTo(target.gameObject,
            iTween.Hash("position", target.nextPOS,
            "time", duration,
            "easetype", "linear"
            ));
        SetMotionAndDurationAndUseHPSP(target);
    }

}
public class Stunned : AAction
{
    public override void Awake()
    {
        base.Awake();
        actioncode = -1;
        _name = "Stunned";
        duration = 1.0f;
    }

    public override bool CanDoAction(AAnimal target)
    {
        return true;
    }
    public override void Action(AAnimal target)
    {
        duration = 1.0f / target.MovementSpeed;
        SetMotionAndDurationAndUseHPSP(target);
        target.Interrupting = false;
    }

}
public class PickUp : AAction
{
    public AItem TargetItem { get; set; }
    public override void Awake()
    {
        base.Awake();
        actioncode = -2;
        _name = "PickUp";
        duration = 0.5f;
    }

    public override bool CanDoAction(AAnimal target)
    {
        Vector3 diff = target.nextPOS - TargetItem.transform.position;
        if (Mathf.Abs(diff.x) < 1.5f && Mathf.Abs(diff.y) < 1.5f && Mathf.Abs(diff.z) < 1.5f) { return true; }
        return false;
    }
    public override void Action(AAnimal target)
    {
        if (target.ItemBag.FindChild(TargetItem.Name))
        {
            Transform t = target.ItemBag.FindChild(TargetItem.Name);
            if (t.GetComponent<AItem>().CanTogether)
            {
                t.GetComponent<AItem>().Number += TargetItem.Number;
            }
        }
        else
        {
            GameObject item = new GameObject(TargetItem.Name);
            item.AddComponent(TargetItem.GetType());
            item.transform.SetParent(target.ItemBag.transform);
        }
        TargetItem.PickUp();
        duration = 0.5f / target.MovementSpeed;
        TargetItem = null;
        SetMotionAndDurationAndUseHPSP(target);
    }

}