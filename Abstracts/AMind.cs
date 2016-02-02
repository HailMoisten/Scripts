﻿using UnityEngine;
using UnityEngine.UI;

public abstract class AMind : AIcon {

    protected int PROFICIENCY = 0;
    public int Proficiency
    {
        get { return PROFICIENCY; }
        set { PROFICIENCY = value; }
    }
    protected string FLAVOR = "";
    public string Flavor {
        get { return FLAVOR; }
    }

    public override ACanvasManager Clicked()
    {
        GameObject inst = Instantiate((GameObject)Resources.Load("Prefabs/GUI/PopUpTextCanvas"));
        PopUpTextCanvasManager ptcm = inst.GetComponent<PopUpTextCanvasManager>();
        ptcm.Title = Name;
        ptcm.Content = Flavor;

        return ptcm;
    }

    /** 
    *Please definite these at inheriting constracter.
    *- string Name
    *- Image ICON
    *- int Proficiency
    */
    protected abstract void Start();


    public void GrowProficiency(int addp) { PROFICIENCY = PROFICIENCY + addp; }

}
