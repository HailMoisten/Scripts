﻿using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public abstract class ACanvasManager : MonoBehaviour {
    //    public AWeapon ReturnedWeapon = null;
    //    public ARing ReturnedRing = null;
    public AAction ReturnedAction = null;
    public GameObject ReturnedMindGO = null;// Only debug
    public int ReturnedInt = 0;
    public bool ReturnedBool = false;


    protected string targetIconName = string.Empty;
    private GameObject target = null;
    public GameObject Target { get { return target; } }
    private GameObject lastTarget = null;
    protected RectTransform myKersolRect = null;
    public ACanvasManager backCanvas = null;
    public ACanvasManager nextCanvas = null;
    public int SortOrder { get { return GetComponent<Canvas>().sortingOrder; } set { GetComponent<Canvas>().sortingOrder = value; } }

    // About Kersol
    protected int pointa = 0;
    public int GetPointa() { return pointa; }
    protected int pointaNUM = 0;
    protected Vector3 kersolPOSfix = new Vector3();
    protected int firstpointa = 0;
    protected Vector3 firstKersolPOS = new Vector3(-1280, -720, 0);

    // At least, please set myKersolRect.
    // Can we access to protected constracters from Prefabs?
    protected abstract void Awake();
    /*
    {
        myKersolRect = transform.FindObject("Kersol").gameObject.GetComponent<RectTransform>();
        backCanvas = null;
        nextCanvas = null;
        pointa = FirstTarget;
        pointaNUM = NumOfSelectableTexts;
        targetPOSfixX = -5;
        targetPOSfixY = -5;
    }
    */

    protected void inclementPointa() { if (pointa >= pointaNUM) { } else { pointa++; } setTarget();}
    protected void declementPointa() { if (pointa <= 1) { } else { pointa--; } setTarget(); }
    protected void setPointa(int n) { if (n <= 0 || n >= pointaNUM+1) { } else { pointa = n; setTarget(); } }
    protected void initPointaAndKersol() {
        if (firstpointa <= 0) { pointa = 0; setTarget(); } else { setPointa(firstpointa); }
        myKersolRect.localPosition = firstKersolPOS;
    }
    protected virtual void setTarget()
    {
        lastTarget = target;
        SelectableTargetManager[] selectables = GetComponentsInChildren<SelectableTargetManager>();
        if (pointa <= 0 || pointa >= selectables.Length + 1) { }
        else { target = selectables[pointa - 1].gameObject;
            try {
                targetIconName = target.GetComponent<SelectableTargetManager>().TargetIcon.Name;
            }
            catch (NullReferenceException) { targetIconName = " "; }
        }
    }
    protected void moveKersol()
    {
        if (lastTarget != null)
        {
            lastTarget.GetComponent<SelectableTargetManager>().OffKersol();
        }
        if (target != null)
        {
            myKersolRect.localPosition = target.GetComponent<RectTransform>().localPosition + kersolPOSfix;
            target.GetComponent<SelectableTargetManager>().OnKersol();
        }
    }
    protected ACanvasManager clickTarget()
    {
        return target.GetComponent<SelectableTargetManager>().Clicked(target.transform.position);
    }
    public void SetBackCanvas(ACanvasManager backC) { backCanvas = backC; SortOrder = backC.SortOrder + 1; }
    public void DestroyThisCanvas() {
        if (nextCanvas != null) { nextCanvas.DestroyThisCanvas(); }
        Destroy(gameObject);
    }
    public void DestroyNextCanvas() { if (nextCanvas != null) { Destroy(nextCanvas.gameObject); } }
}
