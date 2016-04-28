using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuCanvasManager : ACanvasManager {

    private PlayerManager playerManager;
    private PlayerCanvasManager playerCanvas;
    private GameObject[] nextMenus = new GameObject[4];

    protected override void Awake()
    {
        myKersolRect = transform.FindChild("Kersol").GetComponent<RectTransform>();
        pointa = 1;
        pointaNUM = 6;
        kersolPOSfix = new Vector3(0, -8, 0);
        firstpointa = 0;

        playerManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
        playerCanvas = GameObject.Find("PlayerCanvas(Clone)").GetComponent<PlayerCanvasManager>();
        nextMenus[0] = (GameObject)Resources.Load("Prefabs/GUI/InventoryMenuCanvas");
        nextMenus[1] = (GameObject)Resources.Load("Prefabs/GUI/EquipmentMenuCanvas");
        nextMenus[2] = (GameObject)Resources.Load("Prefabs/GUI/StatusMenuCanvas");
        nextMenus[3] = (GameObject)Resources.Load("Prefabs/GUI/ControlsMenuCanvas");
        if (SceneManager.GetActiveScene().name == "Flash")
        {
            transform.FindChild("SelectableTarget (5)").GetComponent<Text>().text = "Leave the flash";
        }

        initPointaAndKersol();
    }
    void Update()
    {
        // Inputs(On Menu)
        Vector3 menuDIR = new Vector3(0, 0, 0);
        menuDIR.x = Input.GetAxisRaw("Horizontal");
        menuDIR.z = Input.GetAxisRaw("Vertical");
        if (nextCanvas != null)
        {
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.UpArrow)) { declementPointa(); moveKersol(); }
            if (Input.GetKeyDown(KeyCode.DownArrow)) { inclementPointa(); moveKersol(); }
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetButtonDown("Submit")) { if (pointa <= 0 || pointa >= pointaNUM+1) { } else { openNextMenu(); } }
            if (Input.GetKeyDown(KeyCode.LeftArrow)) { closeAllMenu(); }
        }
        if (Input.GetButtonDown("Options"))
        {
            closeAllMenu();
        }

        if (ReturnedBool)
        {
            if (pointa == 5) { PassTheFlashGate();}
            else if (pointa == 6) { StartCoroutine(BackToTitle());}
            ReturnedBool = false;
        }
    }

    private void closeAllMenu()
    {
        playerCanvas.CloseMenu(); DestroyThisCanvas();
    }

    private void openNextMenu()
    {
        setPointa(pointa);
        moveKersol();
        if (pointa == 5 || pointa == 6) {
            nextCanvas = Instantiate ((GameObject)Resources.Load ("Prefabs/GUI/YesNoPopUpTextCanvas")).GetComponent<ACanvasManager>();
            YesNoPopUpTextCanvasManager yesnoptcm = nextCanvas.GetComponent<YesNoPopUpTextCanvasManager>();
            nextCanvas.SetBackCanvas(this);
            yesnoptcm.Title = "Confirmation";
            if (pointa == 5)
            {
                if (SceneManager.GetActiveScene().name == "Flash") { yesnoptcm.Content = "Leave the flash soon. Are you sure?";}
                else { yesnoptcm.Content = "Enter the flash soon. Are you sure?"; }
            }
            else if (pointa == 6) { yesnoptcm.Content = "Back to title soon. Are you sure?"; }
        } else
        {
            nextCanvas = Instantiate(nextMenus[pointa-1]).GetComponent<ACanvasManager>();
            nextCanvas.SetBackCanvas(this);
        }
    }

    private void PassTheFlashGate()
    {
        if (SceneManager.GetActiveScene().name == "Flash")
        { FadeManager.Instance.LoadLevel("AlmaVillage", 3.0f, 5.0f); }
        else { FadeManager.Instance.LoadLevel("Flash", 3.0f, 5.0f); }
        playerCanvas.CloseMenu();
    }

    private IEnumerator BackToTitle()
    {
        Debug.Log("BackToTitle.");
        yield return new WaitForEndOfFrame();
        FadeManager.Instance.LoadLevel("Title", 3.0f, 5.0f);
        playerCanvas.CloseMenu();
    }

}
