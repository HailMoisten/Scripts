using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public abstract class AIcon : MonoBehaviour {

    protected string _name = ""; // const
    public string Name { get { return _name; } }
    protected string type = ""; // mind, item, action, etc...
    public string Type { get { return type; } }
    protected string flavor = "";
    public string Flavor
    {
        get { return flavor; }
    }
    protected Sprite icon = null; // const
    public Sprite Icon
    {
        get { return icon; }
        set
        {
            icon = value;
            GetComponent<Image>().sprite = value;
        }
    }
    public abstract void Start();

    public abstract ACanvasManager Clicked(Vector3 newcanvaspos);

}
