﻿using UnityEngine;
using System.Collections;
using IconAndErrorType;

public class AnimalActions { }

// Animal Actions
public class Idle : AAction
{
    public override void Awake()
    {
        base.Awake();
        actioncode = (int)ActionCodeList.Idle;
        _name = "Idle";
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
        base.Awake();
        actioncode = (int)ActionCodeList.Walk;
        _name = "Walk";
        duration = 1.0f;
        spCost = 1;
    }
    public override int CanDoAction(AAnimal myself)
    {
        Vector3 dir2 = new Vector3(myself.nextnextPOS.x - myself.nextPOS.x, 0, myself.nextnextPOS.z - myself.nextPOS.z);
        dir2 = myself.RoundToIntVector3XZ(dir2);
        float maxd = 1.0f;
        if (dir2.x != 0 && dir2.z != 0) { maxd = 1.5f; }
        RaycastHit hitFront; RaycastHit hitDown;
        Ray rayFront = new Ray(myself.nextPOS + new Vector3(0, 1.5f, 0), dir2);
        Ray rayDown = new Ray(myself.nextnextPOS + new Vector3(0, 1.5f, 0), new Vector3(0, -1, 0));
        if (Physics.Raycast(rayFront, out hitFront, maxd))
        {
            //            Debug.Log("hitsFront:" + hitFront.distance);
            if (hitFront.collider.gameObject.layer == LayerMask.NameToLayer("Terrain") ||
                hitFront.collider.gameObject.layer == LayerMask.NameToLayer("Environment") ||
                hitFront.collider.gameObject.layer == LayerMask.NameToLayer("Animal"))
            {
                return (int)ErrorTypeList.Move;
            }
        }
        if (Physics.Raycast(rayDown, out hitDown, 3.0f))
        {
            //            Debug.Log("hitDown:" + hitDown.distance);
            if (hitDown.collider.gameObject.layer == LayerMask.NameToLayer("Environment") ||
                hitDown.collider.gameObject.layer == LayerMask.NameToLayer("Animal"))
            {
                myself.nextnextPOS = myself.nextnextPOS + new Vector3(0, 1.5f - hitDown.distance, 0);
            }
            else if (hitDown.collider.gameObject.layer == LayerMask.NameToLayer("Terrain"))
            {
                myself.nextnextPOS = myself.nextnextPOS + new Vector3(0, Terrain.activeTerrain.SampleHeight(myself.nextnextPOS) - myself.nextnextPOS.y, 0);
            }
            return (int)ErrorTypeList.Nothing;
        }
        return (int)ErrorTypeList.Move;
    }
    public override void SetParamsNeedAnimal(AAnimal myself)
    {
        float diag = 1.0f;
        if (Mathf.Abs(myself.nextPOS.x - myself.nextnextPOS.x) != 0 &&
             Mathf.Abs(myself.nextPOS.z - myself.nextnextPOS.z) != 0) { diag = 1.5f; }
        else { diag = 1.0f; }
        if (myself.MovementSpeed == 0) { }
        else { duration = (diag) / myself.MovementSpeed; }
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
    public override void Awake()
    {
        base.Awake();
        actioncode = (int)ActionCodeList.Run;
        _name = "Run";
        duration = 1.0f;
        spCost = 2;
    }
    public override int CanDoAction(AAnimal myself)
    {
        // Run fix
        float[] subs = myself.GetSubStatus();
        Vector3 dir2 = new Vector3(myself.nextnextPOS.x - myself.nextPOS.x, 0, myself.nextnextPOS.z - myself.nextPOS.z);
        dir2 = myself.RoundToIntVector3XZ(dir2);
        //        dir2 = dir2 * Mathf.RoundToInt(target.RunRatio);
        //        target.nextnextPOS = target.nextPOS + dir2;

        float maxd = 1.0f;
        if (dir2.x != 0 && dir2.z != 0) { maxd = 1.5f; }
        RaycastHit hitFront; RaycastHit hitDown;
        Ray rayFront = new Ray(myself.nextPOS + new Vector3(0, 1.5f, 0), dir2);
        Ray rayDown = new Ray(myself.nextnextPOS + new Vector3(0, 1.5f, 0), new Vector3(0, -1, 0));
        if (Physics.Raycast(rayFront, out hitFront, maxd))
        {
            //            Debug.Log("hitsFront:" + hitFront.distance);
            if (hitFront.collider.gameObject.layer == LayerMask.NameToLayer("Terrain") ||
                hitFront.collider.gameObject.layer == LayerMask.NameToLayer("Environment") ||
                hitFront.collider.gameObject.layer == LayerMask.NameToLayer("Animal"))
            {
                return (int)ErrorTypeList.Move;
            }
        }
        if (Physics.Raycast(rayDown, out hitDown, 3.0f))
        {
            //            Debug.Log("hitDown:" + hitDown.distance);
            if (hitDown.collider.gameObject.layer == LayerMask.NameToLayer("Environment") ||
                hitDown.collider.gameObject.layer == LayerMask.NameToLayer("Animal"))
            {
                myself.nextnextPOS = myself.nextnextPOS + new Vector3(0, 1.5f - hitDown.distance, 0);
            }
            else if (hitDown.collider.gameObject.layer == LayerMask.NameToLayer("Terrain"))
            {
                myself.nextnextPOS = myself.nextnextPOS + new Vector3(0, Terrain.activeTerrain.SampleHeight(myself.nextnextPOS) - myself.nextnextPOS.y, 0);
            }
            return (int)ErrorTypeList.Nothing;
        }
        return (int)ErrorTypeList.Move;
    }
    public override void SetParamsNeedAnimal(AAnimal myself)
    {
        float diag = 1.0f;
        if (Mathf.Abs(myself.nextPOS.x - myself.nextnextPOS.x) != 0 &&
             Mathf.Abs(myself.nextPOS.z - myself.nextnextPOS.z) != 0) { diag = 1.5f; }
        else { diag = 1.0f; }
        float[] subs = myself.GetSubStatus();
        if (myself.MovementSpeed * myself.RunRatio == 0) { }
        else { duration = (diag) / (myself.MovementSpeed * myself.RunRatio); }
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
public class Attack : AAction
{
    public override void Awake()
    {
        base.Awake();
        actioncode = (int)ActionCodeList.Attack;
        _name = "Attack";
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
    }
    public override void Action(AAnimal myself)
    {
        duration = 1.0f / myself.MovementSpeed;
        CreateCubeDamageField(myself, myself.AD, 0, myself.targetPOS);
        base.Action(myself);
        SetMotionAndDurationAndUseHPSP(myself);
    }
}
public class Guard : AAction
{
    public override void Awake()
    {
        base.Awake();
        actioncode = (int)ActionCodeList.Guard;
        _name = "Guard";
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
        base.Awake();
        actioncode = -2;
        _name = "PickUp";
        duration = 0.5f;
    }
    public override int CanDoAction(AAnimal myself)
    {
        Vector3 diff = myself.nextPOS - TargetItem.transform.position;
        if (Mathf.Abs(diff.x) < 1.5f && Mathf.Abs(diff.y) < 1.5f && Mathf.Abs(diff.z) < 1.5f)
        { return (int)ErrorTypeList.Nothing; }
        return -1;
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
            item.transform.SetParent(myself.ItemBag.transform);
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
        base.Awake();
        actioncode = -1;
        _name = "Stunned";
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