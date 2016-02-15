using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ConfigCanvasManager : ACanvasManager {

    protected override void Awake()
    {
        myKersolRect = transform.FindChild("Kersol").GetComponent<RectTransform>();
        pointa = 1;
        pointaNUM = 5;
        kersolPOSfix = new Vector3(-24, 11, 0);
        firstpointa = 0;

        initPointaAndKersol();
    }

    // dont make Update
    void Update()
    {
        Vector3 menuDIR = new Vector3(0, 0, 0);
        menuDIR.x = Input.GetAxisRaw("Horizontal");
        menuDIR.z = Input.GetAxisRaw("Vertical");
        if (nextCanvas != null)
        {
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.DownArrow)) { inclementPointa(); moveKersol(); }
            if (Input.GetKeyDown(KeyCode.UpArrow)) { declementPointa(); moveKersol(); }
            if (Input.GetButtonDown("Submit"))
            {
                if (pointa == pointaNUM)
                {
                    DestroyThisCanvas();
                }
                else { }
            }
            if (Input.GetKeyUp(KeyCode.Backspace)) { DestroyThisCanvas(); }
        }
    }

}
