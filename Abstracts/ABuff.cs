﻿using UnityEngine;
using System.Collections;

public abstract class ABuff : AIcon {

    public bool IsUsed = false;
    protected float duration = 8.0f; public float Duration { get { return duration; } }
    public abstract int[] BuffToMainStatus(int[] mains);
    public abstract float[] BuffToSubStatus(float[] subs);
    public void Used(){ IsUsed = true; Destroy(gameObject, duration); }

    public override ACanvasManager Clicked(Vector3 clickedpos)
    {
        GameObject inst = Instantiate((GameObject)Resources.Load("Prefabs/GUI/PopUpTextCanvas"));
        inst.transform.GetChild(0).GetComponent<RectTransform>().localPosition = clickedpos + new Vector3(64, 64, 0);
        PopUpTextCanvasManager ptcm = inst.GetComponent<PopUpTextCanvasManager>();
        ptcm.Title = Name;
        ptcm.Content = "Duration " + Duration + " sec\n" + Flavor;
        return ptcm;
    }
}