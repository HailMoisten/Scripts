using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PopUpTextCanvasManager : ACanvasManager {

    public string Title
    {
        set
        {
            transform.GetChild(0).transform.FindChild("TitleText").GetComponent<Text>().text = value;
        }
    }
    public string Content
    {
        set
        {
            transform.GetChild(0).transform.FindChild("ContentText").GetComponent<Text>().text = value;
        }
    }

    // Use this for initialization
    protected override void Awake()
    {
        pointa = 0;
        pointaNUM = 0;
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetButtonDown("Submit")) { DestroyThisCanvas(); }
    }
}
