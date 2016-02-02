using UnityEngine;
using System.Collections;

public class ControlsMenuCanvasManager : ACanvasManager {

    // Use this for initialization
    protected override void Start() {
        myKersolRect = transform.FindChild("Kersol").GetComponent<RectTransform>();
        pointa = 0;
        pointaNUM = 0;
        targetPOSfixX = 0;
        targetPOSfixY = 0;

    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            DestroyThisCanvas();
        }
    }
}
