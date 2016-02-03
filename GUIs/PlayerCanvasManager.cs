using UnityEngine;
using System.Collections;

public class PlayerCanvasManager : ACanvasManager {

    private PlayerManager playerManager;

    protected override void Start()
    {
        playerManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
//        myKersolRect = transform.FindChild("Kersol").GetComponent<RectTransform>();
        pointa = 0;
        pointaNUM = 0;
        kersolPOSfixX = -4;
        kersolPOSfixY = -4;
    }

    // Update is called once per frame
    void Update () {
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
