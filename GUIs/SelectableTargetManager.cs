﻿using UnityEngine;
using UnityEngine.UI;

public class SelectableTargetManager : AIcon {

    private bool isSelected = false;
    public bool changeImageAuto = false;
    public bool changeTextColorAuto = false;
    public bool changeTextSizeAuto = false;
    protected Color LightYellow = new Color(1.0f, 1.0f, 0.753f, 1.0f);
    protected Color LightBlue = new Color(0.914f, 0.914f, 1.0f, 1.0f);

    protected Color c0 = new Color(0.5f, 0.5f, 1.0f, 1.0f);
    protected Color c1 = new Color(1.0f, 1.0f, 0.5f, 1.0f);
    protected Color c2 = new Color(1.0f, 0.5f, 0.5f, 1.0f);
    protected Color c3 = new Color(0.5f, 1.0f, 1.0f, 1.0f);


    public void OnKersol()
    {
        isSelected = true;
        Selected();
    }
    public void OffKersol()
    {
        isSelected = false;
        Selected();
    }
    public void setColor0to3(int n)
    {
        Color c;
        switch (n)
        {
            case 1: c = c1; break;
            case 2: c = c2; break;
            case 3: c = c3; break;
            default: c = c0; break;
        }
        this.GetComponent<Text>().color = c;
    }
    private void Selected()
    {
        if (isSelected)
        {
            if (changeImageAuto) { }
            if (changeTextColorAuto) { this.GetComponent<Text>().color = LightYellow; }
            if (changeTextSizeAuto) { this.GetComponent<Text>().fontSize = 22; }
        }
        else
        {
            if (changeImageAuto) { }
            if (changeTextColorAuto) { this.GetComponent<Text>().color = LightBlue; }
            if (changeTextSizeAuto) { this.GetComponent<Text>().fontSize = 16; }
        }
    }

    public override ACanvasManager Clicked(Vector3 clickedpos)
    {
        AIcon child = transform.GetChild(0).GetComponent<AIcon>();
        if (child != null) { return child.Clicked(clickedpos); }
        else { return null; }
    }
}
