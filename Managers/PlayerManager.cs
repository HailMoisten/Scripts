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
    private void SettargetPOS()
    {
        targetPOS = targetPOS + (Quaternion.AngleAxis(45 * camAngle, new Vector3(0, 1, 0)) *
            new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")));
        targetPOS = RoundToIntVector3XZ(targetPOS);
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
    void Start () {
        setUtilities();

        transform.tag = "Player";
        cam = GameObject.Find("Camera");
        playerCanvas = Instantiate((GameObject)Resources.Load("Prefabs/GUI/PlayerCanvas")).GetComponent<ACanvasManager>();

        setMainStatus(165, 100, 100, 100, 100, 100);
        //setMainStatus(10, 5, 10, 10, 2, 5);
//        setMindSkillShortcuts();

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
    }
    protected override void setMindSkillShortcuts()
    {
        mindSkillShortcuts[1] = Minds.transform.GetChild(0).GetComponent<AMind>().GetMindSkill(1);
//        mindSkillShortcuts[1].Icon = Minds.transform.GetChild(0).GetChild(1).GetComponent<AMindSkill>().Icon;
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
                    if (Input.GetKey(KeyCode.Space)) // Action : Run
                    {
                        AddAction(mainActionPool.GetComponent<RunAction>());
                    }
                    else if (Input.GetButton("Action_1"))
                    {
                        if (visualAssistTarget != null) { }
                        else { visualAssistTarget = (GameObject)Instantiate(Resources.Load("Prefabs/GUI/VisualAssistTarget"), targetPOS, Quaternion.identity); }
                        SettargetPOS();
                        visualAssistTarget.transform.position = targetPOS;
                        isInput = true;
                    }
                    else if (Input.GetKey(KeyCode.LeftShift))
                    {
                        // Only doRotate();
                        AddAction(mainActionPool.GetComponent<IdleAction>());
                    }
                    else // Action : Walk
                    {
                        AddAction(mainActionPool.GetComponent<WalkAction>());
                    }
                }
                if (Input.GetButtonDown("Action_1"))
                {
                    if (visualAssistTarget != null) { }
                    else { visualAssistTarget = (GameObject)Instantiate(Resources.Load("Prefabs/GUI/VisualAssistTarget"), targetPOS, Quaternion.identity);
                        targetPOS = nextPOS + Vector3.up; }
                }
                if (Input.GetButtonUp("Action_1"))
                {
                    AddAction(mindSkillShortcuts[1]);
                    Destroy(GameObject.Find("VisualAssistTarget(Clone)"));
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
            }
            // Update every flame
            DoAction();
        }

    }

    public override void YouDied()
    {

    }

}
