using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class YesNoPopUpTextCanvasManager : ACanvasManager {

    public string Title
    {
        set
        {
            transform.FindChild("TitleText").GetComponent<Text>().text = value;
        }
    }
    public string Content
    {
        set
        {
            transform.FindChild("ContentText").GetComponent<Text>().text = value;
        }
    }

    protected override void Start()
    {
        myKersolRect = transform.FindChild("Kersol").GetComponent<RectTransform>();
        pointa = 1;
        pointaNUM = 2;
        targetPOSfixX = -4;
        targetPOSfixY = -4;
    }

    // dont make Update
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow)) { inclementPointa(); moveKersol(); }
        if (Input.GetKeyDown(KeyCode.LeftArrow)) { declementPointa(); moveKersol(); }
        // Should give pointa;
        if (Input.GetButtonDown("Submit"))
        {
            if (pointa == 1) { backCanvas.YesNoAnswerOfNextPopUpTextCanvas = true; }
            else { backCanvas.YesNoAnswerOfNextPopUpTextCanvas = false; }
            DestroyThisCanvas();
        }
    }

}
