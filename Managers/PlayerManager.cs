using UnityEngine;
using System.Collections;
using UnityStandardAssets.Utility;
using System;
using System.Collections.Generic;

public class PlayerManager : AChild {

    private void SetnextnextPOS()
    {
        nextPOS = RoundToIntVector3XZ(nextPOS);
        nextnextPOS = nextPOS + DIR;
    }
    private void SettargetPOS()
    {
        targetPOS = nextPOS + DIR + Vector3.up;
    }
    private void SettargetPOS(int n)
    {
        targetPOS = nextPOS + actionShortcuts[n].SkillPOSVector + Vector3.up;
    }
    private void SettargetPOS(int n, bool focustarget)
    {
        Vector3 sv = actionShortcuts[n].SkillPOSVector;
        if (focusedAnimal != null && focustarget)
        { sv = focusedAnimal.nextPOS - nextPOS; }
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        { sv += new Vector3(0, Input.GetAxisRaw("Vertical"), 0);
        } else {
            sv += (Quaternion.AngleAxis(45 * camAngle, new Vector3(0, 1, 0)) *
                new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")));
        }
        int limit = actionShortcuts[n].SkillRange;
        sv = new Vector3(Mathf.RoundToInt(sv.x), Mathf.RoundToInt(sv.y), Mathf.RoundToInt(sv.z));
        if (sv.x > limit) { sv.x = limit; } else if (sv.x < -1 * limit) { sv.x = -1 * limit; }
        if (sv.y > limit) { sv.y = limit; } else if (sv.y < -1 * limit) { sv.y = -1 * limit; }
        if (sv.z > limit) { sv.z = limit; } else if (sv.z < -1 * limit) { sv.z = -1 * limit; }
        actionShortcuts[n].SkillPOSVector = sv;
        targetPOS = nextPOS + sv + Vector3.up;
    }
    private void resizeVisualAssistTarget(int n)
    {
        Vector3 sv = actionShortcuts[n].SkillScaleVector;
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        { sv += new Vector3(0, Input.GetAxisRaw("Vertical"), 0);
        } else {
            sv += (Quaternion.AngleAxis(90 * (camAngle / 2), Vector3.up) * new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")));
        }
        sv = new Vector3(Mathf.RoundToInt(Mathf.Abs(sv.x)), Mathf.RoundToInt(Mathf.Abs(sv.y)), Mathf.RoundToInt(Mathf.Abs(sv.z)));
        int limit = actionShortcuts[n].SkillScaleOneSideLimit;
        if (sv.x > limit) { sv -= Vector3.right; } else if (sv.x < 1) { sv += Vector3.right; }
        if (sv.y > limit) { sv -= Vector3.up; } else if (sv.y < 1) { sv += Vector3.up; }
        if (sv.z > limit) { sv -= Vector3.forward; } else if (sv.z < 1) { sv += Vector3.forward; }
        actionShortcuts[n].SkillScaleVector = sv;
        visualAssistTarget.transform.localScale = sv;
    }
    private void controlVisualAssistTarget(int n)
    {
        if (visualAssistTarget != null)
        {
            SettargetPOS(n, false);
            visualAssistTarget.transform.position = targetPOS;
        }
        else {
            if (Input.GetKey(KeyCode.LeftControl)) { SettargetPOS(n); } else { SettargetPOS(n, true); }
            visualAssistTarget = (GameObject)Instantiate(Resources.Load("Prefabs/GUI/VisualAssistTarget"), targetPOS, Quaternion.identity);
            visualAssistTarget.transform.localScale = actionShortcuts[n].SkillScaleVector;
        }
    }
    private void SetDirection()
    {
        DIR.x = Input.GetAxisRaw("Horizontal");
        DIR.y = 0;
        DIR.z = Input.GetAxisRaw("Vertical");
        DIR = Quaternion.AngleAxis(45 * camAngle, new Vector3(0, 1, 0))*DIR;
        DIR = RoundToIntVector3XZ(DIR);
    }
    private void CameraRotateClockwise()
    {
        camAngle++; camAngle = camAngle % 8;
        iTween.RotateTo(cam, iTween.Hash("y", 45 * camAngle, "easetype", "easeOutQuad"));
    }
    private void CameraRotateCounterclockwise()
    {
        camAngle--; camAngle = camAngle % 8;
        iTween.RotateTo(cam, iTween.Hash("y", 45 * camAngle, "easetype", "easeOutQuad"));
    }
    private void CameraRotateUp() { cam.GetComponent<CameraManager>().declementHeight(); }
    private void CameraRotateDown() { cam.GetComponent<CameraManager>().inclementHeight(); }
    private void CameraZoomIn() { cam.GetComponent<CameraManager>().declementDistance(); }
    private void CameraZoomOut() { cam.GetComponent<CameraManager>().inclementDistance(); }

    private GameObject cam;
    private int camAngle = 0;

    private ACanvasManager playerCanvas;
    private GameObject visualAssist;
    private GameObject visualAssistTarget;
    private GameObject focus = null;

    public bool isMenuAwake = false;

    // Use this for initialization
    public override void Awake () {
        base.Awake();

        gameObject.tag = "Player";
        cam = GameObject.Find("Camera");
        playerCanvas = Instantiate((GameObject)Resources.Load("Prefabs/GUI/PlayerCanvas")).GetComponent<ACanvasManager>();
        Initialize();
    }
    protected override void Initialize()
    {
        setMainStatus(165, 100, 100, 100, 100, 100);
        //setMainStatus(10, 5, 10, 10, 2, 5);
        GameObject item = new GameObject("AirShard");
        item.AddComponent<AirShard>();
        item.transform.SetParent(ItemBag.transform);
        GameObject mind1 = Instantiate((GameObject)Resources.Load("Prefabs/Minds/Mage"));
        mind1.transform.SetParent(MindBag.transform);
        //actionShortcuts[1] = Mind.GetChild(0).GetComponent<AMind>().GetMindSkill(1);
        //actionShortcuts[1].Icon = Mind.GetChild(0).GetChild(1).GetComponent<AAction>().Icon;
        //actionShortcuts[2] = ItemBag.GetChild(0).GetComponent<AAction>();
        //actionShortcuts[2].Icon = ItemBag.GetChild(0).GetComponent<AAction>().Icon;
    }

    // Update is called once per frame
    void Update ()
    {
        // Inputs(GUI and Camera)
        if (Input.GetButtonDown("CameraRotate")) { CameraRotateClockwise(); }
        if (Input.GetButtonDown("CameraRotateCounter")) { CameraRotateCounterclockwise(); }
        if (Input.GetButtonDown("CameraRotateUp")) { CameraRotateUp(); }
        if (Input.GetButtonDown("CameraRotateDown")) { CameraRotateDown(); }
        if (Input.GetButtonDown("CameraZoomIn")) { CameraZoomIn(); }
        if (Input.GetButtonDown("CameraZoomOut")) { CameraZoomOut(); }

        //On Menu
        if (isMenuAwake)
        {
            // Inputs(On Menu) is on MenuKersolManager.cs

        }
        else
        {
            // Inputs(On Charactor)
            if (isInput) { StartCoroutine(InputCD()); }
            else {
                if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow)
                    || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow))
                {
                    SetDirection();
                    SetnextnextPOS();
                    if (Input.GetButton("Action_1") || Input.GetButton("Action_2") ||
                        Input.GetButton("Action_3") || Input.GetButton("Action_4") ||
                        Input.GetButton("Action_5") || Input.GetButton("Action_6") ||
                        Input.GetButton("Action_7") || Input.GetButton("Action_8")) { }
                    else if (Input.GetButton("TargetSeak"))
                    {
                        focusedAnimal = visionManager.GetNextTargetAnimal();
                    }
                    else if (Input.GetKey(KeyCode.Space))
                    {
                        AddAction(mainComponentPool.GetComponent<Run>());
                    }
                    else if (Input.GetKey(KeyCode.LeftShift))
                    {
                        AddAction(mainComponentPool.GetComponent<Idle>());
                    }
                    else
                    {
                        AddAction(mainComponentPool.GetComponent<Walk>());
                    }
                }
                for (int n = 1; n <= 8; n++)
                {
                    if (actionShortcuts[n] != null)
                    {
                        if (Input.GetButtonDown("Action_" + n))
                        {
                            if (actionShortcuts[n] != null)
                            {
                                if (actionShortcuts[n].CanSelectPosition ||
                                    actionShortcuts[n].CanResize ||
                                    actionShortcuts[n].IsChargeSkill)
                                { controlVisualAssistTarget(n); }
                                else { AddAction(actionShortcuts[n]); }
                            }
                        }
                        if (Input.GetButtonUp("Action_" + n))
                        {
                            if (actionShortcuts[n] != null)
                            {
                                if (actionShortcuts[n].CanSelectPosition ||
                                    actionShortcuts[n].CanResize ||
                                    actionShortcuts[n].IsChargeSkill)
                                {
                                    AddAction(actionShortcuts[n]);
                                    Destroy(GameObject.Find("VisualAssistTarget(Clone)"));
                                }
                            }
                        }
                        if (Input.GetButton("Action_" + n))
                        {
                            if (actionShortcuts[n].IsChargeSkill)
                            {
                                if (actionShortcuts[n].Charged) { }
                                else { actionShortcuts[n].Charge(this); }
                            }
                        }
                    }
                }

                if (Input.GetButtonDown("BattleReady"))
                {
                    BattleReady = !BattleReady;
                    if (BattleReady) { }
                    else { focusedAnimal = null; }
                }
                if (Input.GetButtonDown("Attack"))
                {
                    SettargetPOS();
                    AddAction(mainComponentPool.GetComponent<Attack>());
                }
                if (Input.GetButtonUp("Guard"))
                {
                    AddAction(mainComponentPool.GetComponent<Guard>());
                }
                if (Input.GetButton("Guard"))
                {
                    if (mainComponentPool.GetComponent<Guard>().IsChargeSkill)
                    {
                        if (mainComponentPool.GetComponent<Guard>().Charged) { }
                        else { mainComponentPool.GetComponent<Guard>().Charge(this); }
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow)
                || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                for (int n = 1; n <= 8; n++)
                {
                    if (actionShortcuts[n] != null)
                    {
                        if (Input.GetButton("Action_" + n))
                        {
                            if (Input.GetKey(KeyCode.LeftShift) && actionShortcuts[n].CanResize)
                            { resizeVisualAssistTarget(n); }
                            else if (actionShortcuts[n].CanSelectPosition)
                            { controlVisualAssistTarget(n); }
                            else
                            {
                                SetDirection();
                                SetnextnextPOS();
                                if (Input.GetKey(KeyCode.Space))
                                {
                                    AddAction(mainComponentPool.GetComponent<Run>());
                                }
                                else if (Input.GetKey(KeyCode.LeftShift))
                                {
                                    AddAction(mainComponentPool.GetComponent<Idle>());
                                }
                                else
                                {
                                    AddAction(mainComponentPool.GetComponent<Walk>());
                                }
                            }
                        }
                    }
                }
            }
            if (Input.GetButtonDown("Submit"))
            {
                if(SubmitAction == null) { }
                else {
                    AddAction(SubmitAction);
                    SubmitAction = null;
                }
            }

            if (Input.GetButtonDown("TargetSeak"))
            {
                visionManager.OnOff(true);
                focusedAnimal = visionManager.GetNextTargetAnimal();
            }
            if (Input.GetButtonUp("TargetSeak"))
            {
                visionManager.OnOff(false);
            }
            if (Input.GetButtonDown("IncCurBurst"))
            {
            }
            if (Input.GetButtonDown("DecCurBurst"))
            {
            }
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
            }
            if (Input.GetKeyDown(KeyCode.RightShift))
            {
                if (visualAssist != null) { Destroy(GameObject.Find("VisualAssistManhattanDiag(Clone)")); }
                else
                {
                    visualAssist = (GameObject)Instantiate(Resources.Load("Prefabs/GUI/VisualAssistManhattanDiag"), nextPOS, Quaternion.identity);
                }
            }


            // Update every flame
            FocusControl();
            DoAction();
        }

    }

    private void FocusControl()
    {
        if (focusedAnimal != null)
        {
            if (focus == null)
            {
                focus = (GameObject)Instantiate(Resources.Load("Prefabs/Utilities/Focus"), focusedAnimal.transform.position + (2 * Vector3.up), Quaternion.identity);
                focus.transform.SetParent(transform);
            }
            focus.transform.position = focusedAnimal.transform.position + (2 * Vector3.up);
        }
        else {
            if (focus)
            {
                Destroy(focus);
            }
        }
    }
    public override void YouDied()
    {

    }

}
