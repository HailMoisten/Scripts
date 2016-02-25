using UnityEngine;
using System.Collections;
using IconAndErrorType;
using System;

public abstract class ABuff : AIcon {
    public override void Awake()
    {
        base.Awake();
        IconType = (int)IconTypeList.Buff;
        gameObject.tag = "Buff";
    }

    public bool IsUsed { get; set; }
    public bool IsDrawn { get; set; }
    public float Duration { get; set; }
    public float Sands { get; set; }
    public abstract int[] BuffToMainStatus(int[] mains);
    public abstract float[] BuffToSubStatus(float[] subs);
    public abstract float BuffToHP(float hp);
    public abstract float BuffToHPOnlyOnce(float hp);
    public abstract float BuffToSP(float sp);
    public abstract float BuffToSPOnlyOnce(float sp);

    public void Used(){ IsUsed = true; Sands = Duration; }
    protected void Update()
    {
        if (IsUsed) { Sands -= Time.deltaTime;}
    }
    public override ACanvasManager Clicked(Vector3 clickedpos)
    {
        GameObject inst = Instantiate((GameObject)Resources.Load("Prefabs/GUI/PopUpIconCanvas"));
        inst.transform.GetChild(0).GetComponent<RectTransform>().localPosition = clickedpos + new Vector3(64, 64, 0);
        PopUpIconCanvasManager ptcm = inst.GetComponent<PopUpIconCanvasManager>();
        ptcm.Title = Name;
        ptcm.Icon = Icon;
        ptcm.Content = "Duration " + Duration + " sec\n";
        ptcm.Flavor = Flavor;
        return ptcm;
    }
}
