using UnityEngine;
using System.Collections;

public class ControlsMenuCanvasManager : ACanvasManager {

    // Use this for initialization
    protected override void Awake() {
        myKersolRect = transform.FindChild("Kersol").GetComponent<RectTransform>();
        pointa = 0;
        pointaNUM = 0;
        kersolPOSfix = new Vector3(-4, -4, 0);
        firstpointa = 0;

        initPointaAndKersol();
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            DestroyThisCanvas();
        }
    }
}
