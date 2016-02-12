using UnityEngine;
using System.Collections;

public abstract class ABuff : AIcon {

    public bool IsUsed = false;
    protected float duration = 8.0f; public float Duration { get { return duration; } }

    public abstract int[] BuffToMainStatus(int[] mains);
    public abstract float[] BuffToSubStatus(float[] subs);
    public abstract float BuffToHP(float hp);
    public abstract float BuffToHPOnlyOnce(float hp);
    public abstract float BuffToSP(float sp);
    public abstract float BuffToSPOnlyOnce(float sp);

    public void Used(){ IsUsed = true; Destroy(gameObject, duration); }

    public override ACanvasManager Clicked(Vector3 clickedpos)
    {
        GameObject inst = (GameObject)Instantiate(Resources.Load("Prefabs/GUI/PopUpTextCanvas"), Vector3.one, Quaternion.identity);
        inst.transform.GetChild(0).GetComponent<RectTransform>().localPosition = clickedpos + new Vector3(64, 64, 0);
        PopUpTextCanvasManager ptcm = inst.GetComponent<PopUpTextCanvasManager>();
        ptcm.Title = Name;
        ptcm.Content = "Duration " + Duration + " sec\n" + Flavor;
        return ptcm;
    }
}
