using UnityEngine;
using System.Collections;
using IconType;

public abstract class AItem : AAction {
    protected string modelPass = "";
    public override void Awake()
    {
        base.Awake();
        CanTogether = true;
        IconType = (int)IconTypeList.Item;
        gameObject.tag = "Item";
    }

    public override bool CanDoAction(AAnimal target)
    {
        if (Number <= 0)
        {
            GameObject ecanvas = Instantiate((GameObject)Resources.Load("Prefabs/GUI/ErrorTextCanvas"));
            ecanvas.GetComponent<ErrorTextCanvasManager>().SetAndDestroy(2);
            return false;
        }
        return true;
    }
    public void PickUp()
    {
        Destroy(gameObject, 0.25f);
    }
    protected void Materialize(Vector3 pos)
    {
        GameObject temp = (GameObject)Instantiate(Resources.Load(modelPass), pos + (0.25f*Vector3.up), Quaternion.identity);
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
