using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public abstract class AIcon : MonoBehaviour {

    protected string NAME = ""; // const
    public string Name { get { return NAME; } }
    protected string TYPE = ""; // mind, item, action, etc...
    public string Type { get { return TYPE; } }
    protected Sprite ICON = null; // const
    public Sprite Icon
    {
        get { return ICON; }
        set
        {
            ICON = value;
            GetComponent<Image>().sprite = value;
        }
    }
    public abstract ACanvasManager Clicked();

}
