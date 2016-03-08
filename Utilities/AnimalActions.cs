using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using IconAndErrorType;

public class AnimalActions { }

// Animal Actions
public class Idle : AAction
{
    public override void Awake()
    {
        _name = "Idle";
        base.Awake();
        actioncode = 0;
        duration = 0.0f;
    }
    public override void SetParamsNeedAnimal(AAnimal myself)
    {
        duration = myself.GCD;
    }
    public override void Action(AAnimal myself)
    {
        SetMotionAndDurationAndUseHPSP(myself);
    }

}
public class Walk : AAction
{
    public override void Awake()
    {
        _name = "Walk";
        base.Awake();
        actioncode = 1;
        duration = 1.0f;
        spCost = 0.5f;
    }
    public override int CanDoAction(AAnimal myself)
    {
        RaycastHit hitFront; RaycastHit hitDown;
        Ray rayDown = new Ray(myself.nextnextPOS + new Vector3(0, myself.ObjectScale.y - 0.5f, 0), Vector3.down);

        if (Physics.Raycast(rayDown, out hitDown, 2 * (myself.ObjectScale.y - 0.5f)))
        {
            if (hitDown.collider.gameObject.layer == LayerMask.NameToLayer("Environment") ||
                hitDown.collider.gameObject.layer == LayerMask.NameToLayer("Animal"))
            {
                myself.nextnextPOS += new Vector3(0, myself.ObjectScale.y - 0.5f - hitDown.distance, 0);
            }
            else if (hitDown.collider.gameObject.layer == LayerMask.NameToLayer("Terrain"))
            {
                myself.nextnextPOS += new Vector3(0, Terrain.activeTerrain.SampleHeight(myself.nextnextPOS) - myself.nextnextPOS.y, 0);
            }
        }
        else
        {
            return (int)ErrorTypeList.Move;
        }

        Vector3 dir = new Vector3(myself.nextnextPOS.x - myself.nextPOS.x, myself.nextnextPOS.y - myself.nextPOS.y, myself.nextnextPOS.z - myself.nextPOS.z);
        dir = myself.RoundToIntVector3XZ(dir);
        float maxd = 1.0f;
        if (dir.x != 0 && dir.y != 0 && dir.z != 0) { maxd = 2.25f; }
        else if (dir.x != 0 && dir.y != 0) { maxd = 1.5f; }
        else if (dir.y != 0 && dir.z != 0) { maxd = 1.5f; }
        else if (dir.z != 0 && dir.x != 0) { maxd = 1.5f; }

        Ray rayFront = new Ray(myself.nextPOS + new Vector3(0, myself.ObjectScale.y - 0.5f, 0), dir);
        if (Physics.Raycast(rayFront, out hitFront, maxd))
        {
            if (hitFront.collider.gameObject.layer == LayerMask.NameToLayer("Animal"))
            {
                if (myself.name == hitFront.collider.gameObject.name) { }
                else
                {
                    return (int)ErrorTypeList.Move;
                }
            }
            else if (hitFront.collider.gameObject.layer == LayerMask.NameToLayer("Terrain") ||
                hitFront.collider.gameObject.layer == LayerMask.NameToLayer("Environment"))
            {
                return (int)ErrorTypeList.Move;
            }
        }
        return (int)ErrorTypeList.Nothing;
    }
    public override void SetParamsNeedAnimal(AAnimal myself)
    {
        float diag = 1.0f;
        if (Mathf.Abs(myself.nextPOS.x - myself.nextnextPOS.x) != 0 &&
             Mathf.Abs(myself.nextPOS.z - myself.nextnextPOS.z) != 0) { diag = 1.5f; }
        else { diag = 1.0f; }
        if (myself.MovementSpeed <= 0) { }
        else if (myself.MovementSpeed <= 1.0f) { duration = (diag) / (myself.MovementSpeed); }
        else { duration = (diag) / (2.0f); }
    }
    public override void Action(AAnimal myself)
    {
        myself.POS = myself.nextPOS;
        myself.nextPOS = myself.nextnextPOS;
        iTween.MoveTo(myself.gameObject,
            iTween.Hash("position", myself.nextPOS,
            "time", duration,
            "easetype", "linear"
            ));
        SetMotionAndDurationAndUseHPSP(myself);
    }

}
public class Run : AAction
{
    private Vector3[] runRoute;
    private float[] runLength;
    public override void Awake()
    {
        _name = "Run";
        base.Awake();
        actioncode = 2;
        duration = 1.0f;
        spCost = 1;
    }
    public override int CanDoAction(AAnimal myself)
    {
        runRoute = new Vector3[myself.CurrentRun + 1];
        runRoute[0] = myself.nextPOS;
        runLength = new float[myself.CurrentRun + 1];
        runLength[0] = 1.0f;
        bool endrun = false;
        RaycastHit hitFront; RaycastHit hitDown;
        Ray rayDown, rayFront;
        Vector3 dir;
        dir = new Vector3(myself.nextnextPOS.x - myself.nextPOS.x, 0, myself.nextnextPOS.z - myself.nextPOS.z);
        dir = myself.RoundToIntVector3XZ(dir);

        for (int i = 1; i <= myself.CurrentRun; i++)
        {
            if (endrun) { runRoute[i] = Vector3.zero; runLength[i] = 0.0f; }
            else
            {
                runLength[i] = 1.0f;
                runRoute[i] = runRoute[i - 1] + new Vector3(dir.x, 0, dir.z);
                rayDown = new Ray(runRoute[i] + new Vector3(0, myself.ObjectScale.y - 0.5f, 0), Vector3.down);
                if (Physics.Raycast(rayDown, out hitDown, 2 * (myself.ObjectScale.y - 0.5f)))
                {
                    //            Debug.Log("hitDown:" + hitDown.distance);
                    if (hitDown.collider.gameObject.layer == LayerMask.NameToLayer("Environment") ||
                        hitDown.collider.gameObject.layer == LayerMask.NameToLayer("Animal"))
                    {
                        runRoute[i] += new Vector3(0, myself.ObjectScale.y - 0.5f - hitDown.distance, 0);
                    }
                    else if (hitDown.collider.gameObject.layer == LayerMask.NameToLayer("Terrain"))
                    {
                        runRoute[i] += new Vector3(0, Terrain.activeTerrain.SampleHeight(runRoute[i]) - runRoute[i].y, 0);
                    }
                }
                else
                {
                    endrun = true;
                    if (i == 1) { return (int)ErrorTypeList.Move; }
                }

                if (endrun) { }
                else
                {
                    dir = new Vector3(runRoute[i].x - runRoute[i - 1].x, runRoute[i].y - runRoute[i - 1].y, runRoute[i].z - runRoute[i - 1].z);
                    dir = myself.RoundToIntVector3XZ(dir);
                    if (dir.x != 0 && dir.y != 0 && dir.z != 0) { runLength[i] = 2.25f; }
                    else if (dir.x != 0 && dir.y != 0) { runLength[i] = 1.5f; }
                    else if (dir.y != 0 && dir.z != 0) { runLength[i] = 1.5f; }
                    else if (dir.z != 0 && dir.x != 0) { runLength[i] = 1.5f; }

                    rayFront = new Ray(runRoute[i - 1] + new Vector3(0, myself.ObjectScale.y - 0.5f, 0), dir);
                    if (Physics.Raycast(rayFront, out hitFront, runLength[i]))
                    {
                        //            Debug.Log("hitsFront:" + hitFront.distance);
                        if (hitFront.collider.gameObject.layer == LayerMask.NameToLayer("Animal"))
                        {
                            if (myself.name == hitFront.collider.gameObject.name) { }
                            else
                            {
                                endrun = true;
                                if (i == 1) { return (int)ErrorTypeList.Move; }
                            }
                        }
                        else if (hitFront.collider.gameObject.layer == LayerMask.NameToLayer("Terrain") ||
                            hitFront.collider.gameObject.layer == LayerMask.NameToLayer("Environment"))
                        {
                            endrun = true;
                            if (i == 1) { return (int)ErrorTypeList.Move; }
                        }
                    }
                }
                if (endrun) { runRoute[i] = Vector3.zero; runLength[i] = 0.0f; }

            }
        }
        return (int)ErrorTypeList.Nothing;
    }
    public override void SetParamsNeedAnimal(AAnimal myself)
    {
    }
    public override void Action(AAnimal myself)
    {
        StartCoroutine(running(myself));
    }
    private IEnumerator running(AAnimal myself)
    {
        duration = 0.1f; // for noise
        float speed = 1.0f;
        if (myself.MovementSpeed * myself.MovementBurst == 0) { }
        else { speed = myself.MovementSpeed * myself.MovementBurst; }
        Vector3 endPOS = myself.nextnextPOS;
        for (int i = 1; i <= runRoute.Length - 1; i++)
        {
            if (runLength[i] <= 0.0f) { }
            else { duration += runLength[i] / speed; }
            if (runRoute[i] == Vector3.zero) { }
            else { endPOS = runRoute[i]; }
        }

        myself.GetAnimator().SetInteger("CurrentRun", runRoute.Length - 1);
        myself.GetAnimator().SetInteger("ActionCode", actioncode);
        myself.StartCoroutine(myself.DoingAction(duration));
        myself.POS = myself.nextPOS;
        myself.nextPOS = endPOS;
        float sectorduration = 0.0f;
        for (int i = 1; i <= runRoute.Length - 1; i++)
        {
            if (runRoute[i] == Vector3.zero) { }
            else
            {
                sectorduration = runLength[i] / speed;
                iTween.MoveTo(myself.gameObject,
                    iTween.Hash("position", runRoute[i],
                    "time", sectorduration,
                    "easetype", "linear"
                    ));
                myself.UseHPSP(HPCost, SPCost, HPPercentCost, SPPercentCost);
                yield return new WaitForSeconds(sectorduration);
            }
        }
    }

}
public class Attack : AAction
{
    public override void Awake()
    {
        _name = "Attack";
        base.Awake();
        actioncode = 4;
        duration = 1.0f;
        spCost = 2;
    }

    public override int CanDoAction(AAnimal myself)
    {
        if (myself.BattleReady) { }
        else { return (int)ErrorTypeList.BattleReady; }
        return (int)ErrorTypeList.Nothing;
    }
    public override void SetParamsNeedAnimal(AAnimal myself)
    {
        duration = 1.0f / myself.MovementSpeed;
        SkillPOSFix = myself.EyeLevel;
    }
    public override void Action(AAnimal myself)
    {
        duration = 1.0f / myself.MovementSpeed;
        CreateCubeDamageField(myself, myself.AD, 0, myself.targetPOS + SkillPOSFix);
        base.Action(myself);
        SetMotionAndDurationAndUseHPSP(myself);
    }
}
public class Guard : AAction
{
    public override void Awake()
    {
        _name = "Guard";
        base.Awake();
        actioncode = 5;
        castTime = 1.0f;
        duration = 1.0f;
        spCost = 2;
        isChargeSkill = true;
        chargeSpan = 1;
        chargeLimit = 64;
    }
    public override void SetParamsNeedAnimal(AAnimal myself)
    {
        castTime = 1.0f / myself.MovementSpeed;
        duration = CastTime;
    }
    protected override void ChargingAction(AAnimal myself)
    {
        myself.TakeBuff("Guarding");
    }
    public override void Action(AAnimal myself)
    {
        myself.RemoveBuff("Guarding");
        base.Action(myself);
        SetMotionAndDurationAndUseHPSP(myself);
    }
}

public class PickUp : AAction
{
    public AItem TargetItem { get; set; }
    public override void Awake()
    {
        _name = "PickUp";
        base.Awake();
        actioncode = -2;
        duration = 0.5f;
    }
    public override int CanDoAction(AAnimal myself)
    {
        Vector3 diff = myself.nextPOS - TargetItem.transform.position;
        if (Mathf.Abs(diff.x) < 1.5f && Mathf.Abs(diff.y) < 1.5f && Mathf.Abs(diff.z) < 1.5f)
        { return (int)ErrorTypeList.Nothing; }
        return (int)ErrorTypeList.TooFar;
    }
    public override void SetParamsNeedAnimal(AAnimal myself)
    {
        duration = 0.5f / myself.MovementSpeed;
    }
    public override void Action(AAnimal myself)
    {
        if (myself.ItemBag.FindChild(TargetItem.Name))
        {
            Transform t = myself.ItemBag.FindChild(TargetItem.Name);
            if (t.GetComponent<AItem>().CanTogether)
            {
                t.GetComponent<AItem>().Number += TargetItem.Number;
            }
        }
        else
        {
            GameObject item = new GameObject(TargetItem.Name);
            item.AddComponent(TargetItem.GetType());
            item.GetComponent<AItem>().Number = TargetItem.Number;
            item.transform.SetParent(myself.ItemBag.transform);
        }
        if (myself.tag == "Player")
        {
            GameObject.Find("PlayerCanvas(Clone)").GetComponent<PlayerCanvasManager>().PickUpPopUp(TargetItem);
        }
        TargetItem.PickUp();
        TargetItem = null;
        SetMotionAndDurationAndUseHPSP(myself);
    }

}

public class Stunned : AAction
{
    public override void Awake()
    {
        _name = "Stunned";
        base.Awake();
        actioncode = -1;
        duration = 1.0f;
    }

    public override void SetParamsNeedAnimal(AAnimal myself)
    {
        duration = 1.0f / myself.MovementSpeed;
    }
    public override void Action(AAnimal myself)
    {
        SetMotionAndDurationAndUseHPSP(myself);
        myself.Interrupting = false;
    }

}