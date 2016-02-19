using UnityEngine;
using System.Collections;

public abstract class AItem : AAction {
    public int Number = 1;
//    public int Number { get { return number; } set { number = value; } }
    protected string modelPass = "";
    protected bool canTogether = false;

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
}
