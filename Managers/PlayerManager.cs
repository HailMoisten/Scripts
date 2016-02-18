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
        int limit = mindSkillShortcuts[n].SkillRange;
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

        setMainStatus(165, 100, 100, 100, 100, 1);
        //setMainStatus(10, 5, 10, 10, 2, 5);

    }

    protected override void setUtilities()
    {
        nextPOS = RoundToIntVector3XZ(transform.position);
        BoxCollider myC = gameObject.AddComponent<BoxCollider>();
        myC.isTrigger = true; myC.center = new Vector3(0, 1, 0); myC.size = new Vector3(1, 2, 1);
        Rigidbody myRG = gameObject.AddComponent<Rigidbody>(); myRG.useGravity = false; myRG.isKinematic = true;
        Items = new GameObject("Items");
        Items.transform.SetParent(transform);
        Equipments = new GameObject("Equipments");
        Equipments.transform.SetParent(transform);
        Weapon = new GameObject("Weapon");
        Weapon.transform.SetParent(Equipments.transform);
        Accessories = new GameObject("Accessories");
        Accessories.transform.SetParent(Equipments.transform);
        mindSkillShortcuts = new AMindSkill[9];
        Minds = new GameObject("Minds");
        Minds.transform.SetParent(Equipments.transform);
        GameObject mind1 = Instantiate((GameObject)Resources.Load("Prefabs/Minds/MageMind"));
        mind1.transform.SetParent(Minds.transform);
        Buffs = new GameObject("Buffs");
        Buffs.transform.SetParent(transform);
        setMainActionPool();
        setMindSkillShortcuts();
    }
    protected override void setMindSkillShortcuts()
    {
        mindSkillShortcuts[1] = Minds.transform.GetChild(0).GetComponent<AMind>().GetMindSkill(1);
        mindSkillShortcuts[1].Icon = Minds.transform.GetChild(0).GetChild(1).GetComponent<AMindSkill>().Icon;
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
                    if (Input.GetButtonDown("Action_" + n))
                    {
                        if (mindSkillShortcuts[n] != null)
                        {
                            if (mindSkillShortcuts[n].CanSelectPosition)
                            { controlVisualAssistTarget(n); }
                            else { AddAction(mindSkillShortcuts[n]); }
                        }
                    }
                    if (Input.GetButtonUp("Action_" + n))
                    {
                        if (mindSkillShortcuts[n] != null)
                        {
                            if (mindSkillShortcuts[n].CanSelectPosition)
                            {
                                AddAction(mindSkillShortcuts[n]);
                                Destroy(GameObject.Find("VisualAssistTarget(Clone)"));
                            }
                        }
                    }
                    if (Input.GetButton("Action_" + n))
                    {
                        if (mindSkillShortcuts[n].IsChargeSkill)
                        {
                            if (mindSkillShortcuts[n].Charged) { }
                            else { mindSkillShortcuts[n].Charge(this); }
                        }
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow)
                || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                for (int n = 1; n <= 8; n++)
                {
                    if (Input.GetButton("Action_" + n))
                    {
                        if (Input.GetKey(KeyCode.LeftShift) && mindSkillShortcuts[n].CanResize)
                        { resizeVisualAssistTarget(n); }
                        else if (mindSkillShortcuts[n].CanSelectPosition)
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
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                setMindSkillShortcuts();
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
            sv = mindSkillShortcuts[n].SkillScaleVector + new Vector3(0, Input.GetAxisRaw("Vertical"), 0);
        }
        else
        {
            sv = mindSkillShortcuts[n].SkillScaleVector + (Quaternion.AngleAxis(90 * (camAngle / 2), Vector3.up) * new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")));
            sv = new Vector3(Mathf.RoundToInt(Mathf.Abs(sv.x)), Mathf.RoundToInt(Mathf.Abs(sv.y)), Mathf.RoundToInt(Mathf.Abs(sv.z)));
        }
        int limit = mindSkillShortcuts[n].SkillScaleOneSideLimit;
        if (sv.x > limit) { sv -= Vector3.right; } else if (sv.x < 1) { sv += Vector3.right; }
        if (sv.y > limit) { sv -= Vector3.up; } else if (sv.y < 1) { sv += Vector3.up; }
        if (sv.z > limit) { sv -= Vector3.forward; } else if (sv.z < 1) { sv += Vector3.forward; }
        mindSkillShortcuts[n].SkillScaleVector = sv;
        visualAssistTarget.transform.localScale = mindSkillShortcuts[n].SkillScaleVector;
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
            visualAssistTarget.transform.localScale = mindSkillShortcuts[n].SkillScaleVector;
        }
    }

    public override void YouDied()
    {

    }

}
