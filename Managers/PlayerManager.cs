using UnityEngine;
using System.Collections;
using UnityStandardAssets.Utility;
using System;

public class PlayerManager : AChild {

    private void SetnextnextPOS()
    {
        nextPOS = RoundToIntVector3XZ(nextPOS);
        nextnextPOS = nextPOS + (Quaternion.AngleAxis(45 * camAngle, new Vector3(0, 1, 0))*
            new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")));
        nextnextPOS = RoundToIntVector3XZ(nextnextPOS);
    }
    private void SettargetPOS(int n)
    {
        Vector3 sv = targetPOS - nextPOS;
        Vector3 iv = Vector3.zero;
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            iv = new Vector3(0, Input.GetAxisRaw("Vertical"), 0);
        } else {
            iv = (Quaternion.AngleAxis(45 * camAngle, new Vector3(0, 1, 0)) *
                new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")));
        }
        int limit = actionShortcuts[n].SkillRange;
        if (Mathf.Abs(sv.x + iv.x) > limit) { } else { sv = sv + Vector3.right * iv.x; }
        if (Mathf.Abs(sv.y + iv.y) > limit) { } else { sv = sv + Vector3.up * iv.y; }
        if (Mathf.Abs(sv.z + iv.z) > limit) { } else { sv = sv + Vector3.forward * iv.z; }
        sv = new Vector3(Mathf.RoundToInt(sv.x), Mathf.RoundToInt(sv.y), Mathf.RoundToInt(sv.z));
        targetPOS = nextPOS + sv;
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

    public bool isMenuAwake = false;

    // Use this for initialization
    public override void Awake () {
        setUtilities();

        transform.tag = "Player";
        cam = GameObject.Find("Camera");
        playerCanvas = Instantiate((GameObject)Resources.Load("Prefabs/GUI/PlayerCanvas")).GetComponent<ACanvasManager>();

        setMainStatus(165, 100, 100, 100, 100, 100);
        //setMainStatus(10, 5, 10, 10, 2, 5);

    }

    protected override void setUtilities()
    {
        nextPOS = RoundToIntVector3XZ(transform.position);

        Inventory = transform.FindChild("Inventory");
        ItemBag = Inventory.FindChild("ItemBag");
        WeaponBag = Inventory.FindChild("WeaponBag");
        RingBag = Inventory.FindChild("RingBag");
        MindBag = Inventory.FindChild("MindBag");

        Equipment = transform.FindChild("Equipment");
        Weapon = Equipment.FindChild("Weapon");
        Ring = Equipment.FindChild("Ring");
        Mind = Equipment.FindChild("Mind");

        Buffs = transform.FindChild("Buffs");

        actionShortcuts = new AAction[9];
        GameObject item = new GameObject("AirShard");
        item.AddComponent<AirShard>();
        item.transform.SetParent(ItemBag.transform);
        GameObject mind1 = Instantiate((GameObject)Resources.Load("Prefabs/Minds/Mage"));
        mind1.transform.SetParent(MindBag.transform);
        setMainActionPool();
        setActionShortcuts();
    }
    protected override void setActionShortcuts()
    {
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
                    else if (Input.GetKey(KeyCode.Space))
                    {
                        AddAction(mainActionPool.GetComponent<RunAction>());
                    }
                    else if (Input.GetKey(KeyCode.LeftShift))
                    {
                        AddAction(mainActionPool.GetComponent<IdleAction>());
                    }
                    else
                    {
                        AddAction(mainActionPool.GetComponent<WalkAction>());
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
                                if (actionShortcuts[n].CanSelectPosition)
                                { controlVisualAssistTarget(n); }
                                else { AddAction(actionShortcuts[n]); }
                            }
                        }
                        if (Input.GetButtonUp("Action_" + n))
                        {
                            if (actionShortcuts[n] != null)
                            {
                                if (actionShortcuts[n].CanSelectPosition)
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
                                    AddAction(mainActionPool.GetComponent<RunAction>());
                                }
                                else if (Input.GetKey(KeyCode.LeftShift))
                                {
                                    AddAction(mainActionPool.GetComponent<IdleAction>());
                                }
                                else
                                {
                                    AddAction(mainActionPool.GetComponent<WalkAction>());
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
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                setActionShortcuts();
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
            DoAction();
        }

    }

    private void resizeVisualAssistTarget(int n)
    {
        Vector3 sv = Vector3.zero;
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            sv = actionShortcuts[n].SkillScaleVector + new Vector3(0, Input.GetAxisRaw("Vertical"), 0);
        }
        else
        {
            sv = actionShortcuts[n].SkillScaleVector + (Quaternion.AngleAxis(90 * (camAngle / 2), Vector3.up) * new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")));
            sv = new Vector3(Mathf.RoundToInt(Mathf.Abs(sv.x)), Mathf.RoundToInt(Mathf.Abs(sv.y)), Mathf.RoundToInt(Mathf.Abs(sv.z)));
        }
        int limit = actionShortcuts[n].SkillScaleOneSideLimit;
        if (sv.x > limit) { sv -= Vector3.right; } else if (sv.x < 1) { sv += Vector3.right; }
        if (sv.y > limit) { sv -= Vector3.up; } else if (sv.y < 1) { sv += Vector3.up; }
        if (sv.z > limit) { sv -= Vector3.forward; } else if (sv.z < 1) { sv += Vector3.forward; }
        actionShortcuts[n].SkillScaleVector = sv;
        visualAssistTarget.transform.localScale = actionShortcuts[n].SkillScaleVector;
    }
    private void controlVisualAssistTarget(int n)
    {
        if (visualAssistTarget != null)
        {
            SettargetPOS(n);
            visualAssistTarget.transform.position = targetPOS;
        }
        else {
            if (Input.GetKey(KeyCode.LeftAlt)) { } else { targetPOS = nextPOS + Vector3.up; }
            SettargetPOS(n);
            visualAssistTarget = (GameObject)Instantiate(Resources.Load("Prefabs/GUI/VisualAssistTarget"), targetPOS, Quaternion.identity);
            visualAssistTarget.transform.localScale = actionShortcuts[n].SkillScaleVector;
        }
    }

    public override void YouDied()
    {

    }

}
