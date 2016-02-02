using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PopUpTextCanvasManager : ACanvasManager {

    public string Title
    {
        set
        {
            transform.FindChild("Panel").transform.FindChild("TitleText").GetComponent<Text>().text = value;
        }
    }
    public string Content
    {
        set
        {
            transform.FindChild("Panel").transform.FindChild("ContentText").GetComponent<Text>().text = value;
        }
    }

    // Use this for initialization
    protected override void Start()
    {
        pointa = 0;
        pointaNUM = 0;
        targetPOSfixX = 40;
        targetPOSfixY = 40;
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetButtonDown("Submit")) { DestroyThisCanvas(); }
    }
}
