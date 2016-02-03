using UnityEngine;
using System.Collections;
using UnityStandardAssets.Utility;

public class PlayerManager : AChild {

    private IEnumerator GCD(float waittime)
    {
        yield return new WaitForSeconds(waittime);
        IsCD = false;
    }
    private void SetnextnextPOS()
    {
        nextPOS = RoundToIntVector3XZ(nextPOS);
        nextnextPOS = nextPOS + (Quaternion.AngleAxis(45 * camAngle, new Vector3(0, 1, 0))*
            new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")));
        nextnextPOS = RoundToIntVector3XZ(nextnextPOS);
    }
    private void SetDirection()
    {
        DIR.x = Input.GetAxisRaw("Horizontal");
        DIR.y = 0;
        DIR.z = Input.GetAxisRaw("Vertical");
        DIR = Quaternion.AngleAxis(45 * camAngle, new Vector3(0, 1, 0))*DIR;
        DIR = RoundToIntVector3XZ(DIR);
    }
    private void RunFix()
    {
        Vector3 dir2 = new Vector3(nextnextPOS.x - nextPOS.x, 0, nextnextPOS.z - nextPOS.z);
        dir2 = RoundToIntVector3XZ(dir2);
        dir2 = dir2 * Mathf.RoundToInt(RunRatio);
        nextnextPOS = nextPOS + dir2;
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

    // Use this for initialization
    void Start () {
        this.transform.tag = "Player";
        // 3*(Lv-1) + 5
        setMainStatus(165, 100, 100, 100, 100, 50);

        nextPOS = RoundToIntVector3XZ(this.transform.position);
        cam = GameObject.Find("Camera");
        playerCanvas = Instantiate((GameObject)Resources.Load("Prefabs/GUI/PlayerCanvas")).GetComponent<ACanvasManager>();
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
            if (IsCD) { StartCoroutine(GCD(CD)); }
            else {
                if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow)
                    || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow))
                {
                    SetDirection();
                    SetnextnextPOS();
                    if (Input.GetKey(KeyCode.Space)) // Action : Run
                    {
                        RunFix();
                        if (CanMoveToThere())
                        {
                            AddAction(new RunAction());
                        }
                        else { AddAction(new IdleAction()); }// need any SE // Cant walk.
                    }
                    else if (Input.GetKey(KeyCode.LeftShift))
                    {
                        // Only doRotate();
                        AddAction(new IdleAction());
                    }
                    else // Action : Walk
                    {
                        if (CanMoveToThere())
                        {
                            AddAction(new WalkAction());
                        }
                        else { AddAction(new IdleAction()); }// need any SE // Cant walk.
                    }
                }
                if (Input.GetButton("Fire1"))
                {
                }
            }
            // Update every flame
            DoAction();
            UIUpdate();
        }

    }

    public bool isMenuAwake = false;

    private void UIUpdate()
    {
        //HPBar, SPBar
    }
    public override void YouDied()
    {

    }

}
