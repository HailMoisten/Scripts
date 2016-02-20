using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PopUpIconCanvasManager : ACanvasManager {

    public string Title
    {
        set
        {
            transform.GetChild(0).transform.FindChild("TitleText").GetComponent<Text>().text = value;
        }
    }
    public Sprite Icon
    {
        set
        {
            transform.GetChild(0).transform.FindChild("Icon").GetComponent<Image>().sprite = value;
        }
    }
    public string Content
    {
        set
        {
            transform.GetChild(0).transform.FindChild("ContentText").GetComponent<Text>().text = value;
        }
    }
    public string Flavor
    {
        set
        {
            transform.GetChild(0).transform.FindChild("FlavorText").GetComponent<Text>().text = value;
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
