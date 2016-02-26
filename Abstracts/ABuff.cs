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
    public string objectPass = "";
    public bool FirstsUsed { get; set; }
    public bool LastsFlag { get; set; }
    public bool IsDrawn { get; set; }
    public bool IsToggle { get; set; }
    public float Duration { get; set; }
    public float Sands { get; set; }
    public bool Toggle { get; set; }
    public virtual int[] BuffToMainStatus(int[] mains) { return mains; }
    public virtual float[] BuffToSubStatus(float[] subs) { return subs; }
    public virtual float BuffToHP(float hp) { return hp; }
    public virtual float BuffToHPFirst(float hp) { return hp; }
    public virtual float BuffToHPLast(float hp) { return hp; }
    public virtual float BuffToSP(float sp) { return sp; }
    public virtual float BuffToSPFirst(float sp) { return sp; }
    public virtual float BuffToSPLast(float sp) { return sp; }


    public void UsedFirsts(){ FirstsUsed = true; Sands = Duration; }
    public void UsedLasts() { Destroy(gameObject); }
    protected void Update()
    {
        if (IsToggle) { if (Toggle) { } else { LastsFlag = true; } }
        else
        {
            if (FirstsUsed) {
                Sands -= Time.deltaTime;
                if (Sands <= 0.0f) { LastsFlag = true; }
            }
        }
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
