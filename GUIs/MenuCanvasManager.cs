using UnityEngine;

public class MenuCanvasManager : ACanvasManager {

    private PlayerManager playerManager;
    private PlayerCanvasManager playerCanvas;
    private GameObject[] nextMenus = new GameObject[4];

    protected override void Start()
    {
        myKersolRect = transform.FindChild("Kersol").GetComponent<RectTransform>();
        pointa = 1;
        pointaNUM = 4;
        kersolPOSfixX = 0;
        kersolPOSfixY = -8;

        playerManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
        playerCanvas = GameObject.Find("PlayerCanvas(Clone)").GetComponent<PlayerCanvasManager>();
        nextMenus[0] = (GameObject)Resources.Load("Prefabs/GUI/ItemMenuCanvas");
        nextMenus[1] = (GameObject)Resources.Load("Prefabs/GUI/EquipmentMenuCanvas");
        nextMenus[2] = (GameObject)Resources.Load("Prefabs/GUI/StatusMenuCanvas");
        nextMenus[3] = (GameObject)Resources.Load("Prefabs/GUI/ControlsMenuCanvas");
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
            if (Input.GetKeyDown(KeyCode.RightArrow)) { openNextMenu(); }
            if (Input.GetKeyDown(KeyCode.LeftArrow)) { closeAllMenu(); }
        }
        if (Input.GetButtonDown("Options"))
        {
            closeAllMenu();
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
        nextCanvas = Instantiate(nextMenus[pointa-1]).GetComponent<ACanvasManager>();
        nextCanvas.GetComponent<ACanvasManager>().SetBackCanvas(this);
    }

}
