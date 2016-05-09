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
        icon = Resources.Load<Sprite>("Images/GUI/glass_black");
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
        icon = Resources.Load<Sprite>("Images/Icons/Utility/Walk");
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
        else if (myself.MovementSpeed <= 1.0f) { duration = (diag) / (myself.MovementSpeed * 2); }
		else { duration = (diag) / (myself.MovementSpeed); }
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
    private Vector3[] runRouteForCalc;
    private float[] runLength;
    private int runStep;
    private Vector3[] runRouteForReal;
    public override void Awake()
    {
        _name = "Run";
        base.Awake();
        actioncode = 2;
        duration = 1.0f;
        spCost = 1;
        icon = Resources.Load<Sprite>("Images/Icons/Utility/Run");
    }
    public override int CanDoAction(AAnimal myself)
    {
        runRouteForCalc = new Vector3[myself.CurrentRun + 1];
        runRouteForCalc[0] = myself.nextPOS;
        runLength = new float[myself.CurrentRun + 1];
        runLength[0] = 1.0f;
        runStep = 0;
        RaycastHit hitFront; RaycastHit hitDown;
        Ray rayDown, rayFront;
        Vector3 dir = new Vector3(myself.nextnextPOS.x - myself.nextPOS.x, 0, myself.nextnextPOS.z - myself.nextPOS.z);

        for (int i = 1; i <= myself.CurrentRun; i++)
        {
            if (runStep == 0)
            {
                runLength[i] = 1.0f;
                runRouteForCalc[i] = runRouteForCalc[i - 1] + new Vector3(dir.x, 0, dir.z);
                rayDown = new Ray(runRouteForCalc[i] + new Vector3(0, myself.ObjectScale.y - 0.5f, 0), Vector3.down);
                if (Physics.Raycast(rayDown, out hitDown, 2 * (myself.ObjectScale.y - 0.5f)))
                {
                    if (hitDown.collider.gameObject.layer == LayerMask.NameToLayer("Environment") ||
                        hitDown.collider.gameObject.layer == LayerMask.NameToLayer("Animal"))
                    {
                        runRouteForCalc[i] += new Vector3(0, myself.ObjectScale.y - 0.5f - hitDown.distance, 0);
                    }
                    else if (hitDown.collider.gameObject.layer == LayerMask.NameToLayer("Terrain"))
                    {
                        runRouteForCalc[i] += new Vector3(0, Terrain.activeTerrain.SampleHeight(runRouteForCalc[i]) - runRouteForCalc[i].y, 0);
                    }
                }
                else
                {
                    if (i == 1) { return (int)ErrorTypeList.Move; }
                    runStep = i - 1;
                }

                if (runStep == 0)
                {
                    dir = new Vector3(runRouteForCalc[i].x - runRouteForCalc[i - 1].x, runRouteForCalc[i].y - runRouteForCalc[i - 1].y, runRouteForCalc[i].z - runRouteForCalc[i - 1].z);
                    if (dir.x != 0 && dir.y != 0 && dir.z != 0) { runLength[i] = 2.25f; }
                    else if (dir.x != 0 && dir.y != 0) { runLength[i] = 1.5f; }
                    else if (dir.y != 0 && dir.z != 0) { runLength[i] = 1.5f; }
                    else if (dir.z != 0 && dir.x != 0) { runLength[i] = 1.5f; }

                    rayFront = new Ray(runRouteForCalc[i - 1] + new Vector3(0, myself.ObjectScale.y - 0.5f, 0), dir);
                    if (Physics.Raycast(rayFront, out hitFront, runLength[i]))
                    {
                        if (hitFront.collider.gameObject.layer == LayerMask.NameToLayer("Animal"))
                        {
                            if (myself.name == hitFront.collider.gameObject.name) { }
                            else
                            {
                                if (i == 1) { return (int)ErrorTypeList.Move; }
                                runStep = i - 1;
                            }
                        }
                        else if (hitFront.collider.gameObject.layer == LayerMask.NameToLayer("Terrain") ||
                            hitFront.collider.gameObject.layer == LayerMask.NameToLayer("Environment"))
                        {
                            if (i == 1) { return (int)ErrorTypeList.Move; }
                            runStep = i - 1;
                        }
                    }
                }
            }
        }
        if (runStep == 0) { runStep = myself.CurrentRun; }
        return (int)ErrorTypeList.Nothing;
    }
    public override void SetParamsNeedAnimal(AAnimal myself)
    {
    }
    public override void Action(AAnimal myself)
    {
        duration = 0.0f;
        runRouteForReal = new Vector3[runStep];
        spCost = 1 * runStep;
        float speed = 1.0f;
        if (myself.MovementSpeed * myself.MovementBurst == 0) { }
        else { speed = myself.MovementSpeed * myself.MovementBurst; }

        for (int i = 1; i <= runRouteForReal.Length; i++)
        {
            duration += runLength[i] / speed;
            runRouteForReal[i - 1] = runRouteForCalc[i];
        }

        myself.POS = myself.nextPOS;
        myself.nextPOS = runRouteForReal[runStep - 1];
        if (runRouteForReal.Length == 1)
        {
            iTween.MoveTo(myself.gameObject,
                iTween.Hash("position", runRouteForReal[0], "time", duration, "easetype", "linear"));
        }
        else
        {
            iTween.MoveTo(myself.gameObject,
                iTween.Hash("path", runRouteForReal, "time", duration, "easetype", "linear"));
        }
        myself.GetAnimator().SetFloat("Speed", speed);
        myself.GetAnimator().SetInteger("ActionCode", actioncode);
        myself.UseHPSP(HPCost, SPCost, HPPercentCost, SPPercentCost);
        myself.StartCoroutine(myself.DoingAction(duration));
    }

}
public class Jump : AAction
{
    private List<Vector3> jumpRouteList = new List<Vector3>();
    private Vector3 nextpos;
    private float currentRun;
    private float currentJump;

    public override void Awake()
    {
        _name = "Jump";
        base.Awake();
        actioncode = 3;
        duration = 1.0f;
        spCost = 2;
        icon = Resources.Load<Sprite>("Images/Icons/Utility/Jump");
    }
    public override int CanDoAction(AAnimal myself)
    {
        jumpRouteList.Clear();
        nextpos = myself.nextPOS;
        jumpRouteList.Add(nextpos);
        currentRun = myself.CurrentRun;
        currentJump = myself.CurrentJump;
        bool landed = false;
        Vector3 endpos = myself.nextPOS;
        bool clashed = false;
        Vector3 dir = myself.DIR;
        float jumpsum = 0.0f;
        for (int i = 1; i <= currentJump; i++) { jumpsum += i; }
        RaycastHit hit;
        Ray ray;
        float distance = 1.0f;

        float sum = 0.0f;
        float r = 0;
        float j = 0;
        while (!landed && j <= 32)
        {
            //for (j = 0; j <= currentJump * 2; j++)
            //{
            //if (landed) { }
            //else
            //{
            // Horizontal Check
            if (dir == Vector3.zero || clashed) { }
            else {
                ray = new Ray(nextpos
                    + (currentRun * r * dir * (1.0f / (currentJump * 2.0f)))
                    + ((sum / jumpsum) * currentJump * Vector3.up)
                    + new Vector3(0, myself.ObjectScale.y - 0.5f, 0), dir);
                distance = currentRun / (currentJump * 2);
                if (dir.x != 0 && dir.z != 0) { distance = distance * 1.5f; }
                if (Physics.Raycast(ray, out hit, distance))
                {
                    Debug.Log("hitfront");
                    clashed = true;
                }

            }
            if (clashed) { }
            else { r++; }

            // Vertical Check
            if (j < currentJump)
            {
                ray = new Ray(nextpos
                    + ((currentRun * r * (1.0f / (currentJump * 2.0f))) * dir)
                    + ((sum / jumpsum) * currentJump * Vector3.up)
                    + new Vector3(0, myself.ObjectScale.y - 0.5f, 0), Vector3.up);
                distance = Mathf.Abs(((currentJump - j) / jumpsum) * currentJump) + 0.5f;
                if (Physics.Raycast(ray, out hit, distance))
                {
                    Debug.Log("hitup");
                    j = currentJump;
                }
            }
            else if (j > currentJump)
            {
                ray = new Ray(nextpos
                    + ((currentRun * r * (1.0f / (currentJump * 2.0f))) * dir)
                    + ((sum / jumpsum) * currentJump * Vector3.up)
                    + new Vector3(0, 0.5f, 0), Vector3.down);
                distance = Mathf.Abs(((currentJump - j) / jumpsum) * currentJump) + 0.5f;
                if (Physics.Raycast(ray, out hit, distance))
                {
                    Debug.Log("landed");
                    if (myself.name == hit.collider.gameObject.name) { }
                    else
                    {
                        endpos = myself.RoundToIntVector3XZ(hit.point);
                        landed = true;
                    }
                }
            }

            // Fix and Add

            if (j == currentJump)
            {
                r--;
            }
            else if (landed)
            {
                jumpRouteList.Add(endpos);
            }
            else {
                sum += currentJump - j;
                jumpRouteList.Add(nextpos
                    + ((currentRun * r * (1.0f / (currentJump * 2.0f))) * dir)
                    + ((sum / jumpsum) * currentJump * Vector3.up));
            }
            //            }
            //            }
            j++;

        }
        if (landed) { }
        else { return (int)ErrorTypeList.Jump; }
        return (int)ErrorTypeList.Nothing;
    }
    public override void SetParamsNeedAnimal(AAnimal myself)
    {
    }
    public override void Action(AAnimal myself)
    {
        float speed = 1.0f;
        if (myself.MovementSpeed * myself.MovementBurst == 0) { }
        else { speed = myself.MovementSpeed * myself.MovementBurst; }

        Vector3[] jumpRoute = new Vector3[jumpRouteList.Count];
        for (int c = 0; c <= jumpRouteList.Count - 1; c++) { jumpRoute[c] = jumpRouteList[c]; }
        for (int c = 0; c <= jumpRoute.Length - 1; c++) { Debug.Log(jumpRoute[c]); }

        if (jumpRouteList.Count <= 2)
        { duration = 0.0f; }
        else
        {
            duration = ((currentJump + 7) / 8) * (jumpRoute.Length / (1 + (currentJump * 2)));
            if (duration < 1.0f) { duration = 1.0f; }

            myself.POS = myself.nextPOS;
            myself.nextPOS = jumpRoute[jumpRoute.Length - 1];
            iTween.MoveTo(myself.gameObject,
                iTween.Hash("path", jumpRoute, "movetopath", false, "time", duration, "easetype", "linear"));

            myself.GetAnimator().SetFloat("Speed", speed);
        }
        SetMotionAndDurationAndUseHPSP(myself);
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
        icon = Resources.Load<Sprite>("Images/Icons/Utility/BattleReadyOn");
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
        icon = Resources.Load<Sprite>("Images/Icons/Buff/Guard");
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
        icon = Resources.Load<Sprite>("Images/Icons/Utility/PickUp");
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
            GameObject.Find("PlayerCanvas").GetComponent<PlayerCanvasManager>().PickUpPopUp(TargetItem);
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
        icon = Resources.Load<Sprite>("Images/Icons/Utility/Stunned");
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