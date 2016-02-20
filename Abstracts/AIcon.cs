using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using IconType;

namespace IconType
{
    enum IconTypeList { Mind, Action, Item, Weapon, Ring, Buff };
}

public abstract class AIcon : MonoBehaviour {
    public int Number = 1;
    public bool CanTogether = false;
    protected string _name = ""; // const
    public string Name { get { return _name; } }
    public int IconType;
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
    public virtual void Awake()
    {
        if (gameObject.GetComponent<Image>()) { } else { gameObject.AddComponent<Image>(); }
        icon = GetComponent<Image>().sprite;
        gameObject.tag = "Icon";
    }

    public abstract ACanvasManager Clicked(Vector3 newcanvaspos);

}
