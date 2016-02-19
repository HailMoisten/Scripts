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

    protected override void Awake()
    {
        myKersolRect = transform.FindChild("Kersol").GetComponent<RectTransform>();
        pointa = 1;
        pointaNUM = 2;
        kersolPOSfix = new Vector3(-4, -4, 0);
        firstpointa = 0;

        initPointaAndKersol();
    }

    // dont make Update
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow)) { inclementPointa(); moveKersol(); }
        if (Input.GetKeyDown(KeyCode.LeftArrow)) { declementPointa(); moveKersol(); }
        // Should give pointa;
        if (Input.GetButtonDown("Submit"))
        {
            backCanvas.ReturnedIcon = Target.GetComponent<SelectableTargetManager>().TargetIcon;
            DestroyThisCanvas();
        }
    }

}
