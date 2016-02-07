using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerCanvasManager : ACanvasManager {

    private PlayerManager playerManager;
    private RectTransform HPBar;
    private Text HPText;
    private RectTransform SPBar;
    private Text SPText;

    protected override void Start()
    {
        playerManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
        HPBar = transform.FindChild("HPBar").GetComponent<RectTransform>();
        HPText = transform.FindChild("HPText").GetComponent<Text>();
        SPBar = transform.FindChild("SPBar").GetComponent<RectTransform>();
        SPText = transform.FindChild("SPText").GetComponent<Text>();
        myKersolRect = transform.FindChild("Kersol").GetComponent<RectTransform>();
        pointa = 0;
        pointaNUM = 0;
        kersolPOSfix = new Vector3(-4, -4, 0);
        firstpointa = 0;

        initPointaAndKersol();
    }

    // Update is called once per frame
    void Update () {
        int hpbarwid = (int)(250 * playerManager.HP / playerManager.MaxHP);
        int spbarwid = (int)(250 * playerManager.SP / playerManager.MaxSP);
        HPBar.sizeDelta = new Vector2(hpbarwid, HPBar.sizeDelta.y);
        HPText.text = "HP    " + Mathf.RoundToInt(playerManager.HP).ToString(); HPText.GetComponent<TextShade>().TextUpdate();
        SPBar.sizeDelta = new Vector2(spbarwid, SPBar.sizeDelta.y);
        SPText.text = "SP    " + Mathf.RoundToInt(playerManager.SP).ToString(); SPText.GetComponent<TextShade>().TextUpdate();

        if (nextCanvas != null) { }
        else
        {
            if (Input.GetButtonDown("Options"))
            {
                openMenu();
            }
        }
    }
    private void openMenu()
    {
        playerManager.isMenuAwake = true;
        Time.timeScale = 0.0f;
        nextCanvas = Instantiate((GameObject)Resources.Load("Prefabs/GUI/MenuCanvas")).GetComponent<ACanvasManager>();
        nextCanvas.GetComponent<ACanvasManager>().SetBackCanvas(this);
    }
    public void CloseMenu()
    {
        playerManager.isMenuAwake = false;
        Time.timeScale = 1.0f;
        nextCanvas.DestroyThisCanvas();
    }

}
