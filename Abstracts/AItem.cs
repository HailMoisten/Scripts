using UnityEngine;
using System.Collections;
using IconAndErrorType;

public abstract class AItem : AAction {
    protected string objectPass = "";
    public override void Awake()
    {
        base.Awake();
        CanTogether = true;
        IconType = (int)IconTypeList.Item;
        gameObject.tag = "Item";
    }

    public override int CanDoAction(AAnimal target)
    {
        if (Number <= 0)
        {
            return (int)ErrorTypeList.Number;
        }
        return (int)ErrorTypeList.Nothing;
    }
    public void PickUp()
    {
        Destroy(gameObject, 0.25f);
    }
    protected void Materialize(Vector3 pos)
    {
        GameObject temp = (GameObject)Instantiate(Resources.Load(objectPass), pos + (0.25f*Vector3.up), Quaternion.identity);
        temp.AddComponent(GetType());
        Number--;
        if(Number <= 0) { Destroy(gameObject, 0.25f); }
    }
    public override ACanvasManager Clicked(Vector3 clickedpos)
    {
        GameObject inst = Instantiate((GameObject)Resources.Load("Prefabs/GUI/PopUpIconCanvas"));
        inst.transform.GetChild(0).GetComponent<RectTransform>().localPosition = clickedpos + new Vector3(64, 64, 0);
        PopUpIconCanvasManager ptcm = inst.GetComponent<PopUpIconCanvasManager>();
        ptcm.Title = Name;
        ptcm.Icon = Icon;
        ptcm.Content = "Number of possession " + Number;
        ptcm.Flavor = Flavor;
        return ptcm;
    }

}
