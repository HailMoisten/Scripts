using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using IconAndErrorType;

public class ReturnInventoryMenuCanvasManager : InventoryMenuCanvasManager {

    public int TargetIconType;
    private SelectableTargetManager targetSTM;
    protected AMind targetMind = null;

    // Use this for initialization
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void setupInventory()
    {
        Transform target = null;
        if (currentInventory <= 1) { target = playerManager.ItemBag; }
        else if (currentInventory == 2) { target = playerManager.WeaponBag; }
        else if (currentInventory == 3) { target = playerManager.RingBag; }
        else if (currentInventory >= 4) { target = playerManager.Mind; }
        int pages = 1 + (target.childCount / 20);
        if (currentPage > pages) { currentPage = pages; } else if (currentPage < 1) { currentPage = 1; }
        int pagehead = (currentPage - 1) * 20;
        for (int n = 0; n <= 19; n++)
        {
            setPointa(5 + n);
            Target.GetComponent<SelectableTargetManager>().TargetIcon = null;
            Target.GetComponent<AIcon>().Icon = nullSprite;
            Target.GetComponent<SelectableTargetManager>().SetNumber(0);
            try
            {
                if (n + pagehead <= target.childCount - 1)
                {
                    AIcon targeticon = target.GetChild(n + pagehead).GetComponent<AIcon>();
                    Target.GetComponent<SelectableTargetManager>().TargetIcon = targeticon;
                    Target.GetComponent<AIcon>().Icon = targeticon.Icon;
                    if (targeticon.CanTogether)
                    {
                        Target.GetComponent<SelectableTargetManager>().SetNumber(targeticon.Number);
                    }

                }
            }
            catch (Exception) { Debug.Log("wrong calc"); }
        }
        setPointa(currentInventory); setPointa(5); moveKersol();
    }

    // Update is called once per frame
    void Update()
    {
        if (nextCanvas != null)
        {
            if (Input.GetButtonDown("Submit"))
            {
                if (targetMind != null) {
                    if (targetMind.MindLevel >= nextCanvas.GetPointa())
                    {
                        backCanvas.ReturnedAction = targetMind.transform.GetChild(nextCanvas.GetPointa()).GetComponent<AAction>();
                    }
                    else
                    {
                        GameObject ecanvas = Instantiate((GameObject)Resources.Load("Prefabs/GUI/ErrorTextCanvas"));
                        ecanvas.GetComponent<ErrorTextCanvasManager>().SetAndDestroy((int)ErrorTypeList.MindLevel);
                    }
                    DestroyThisCanvas();
                }
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                Debug.Log(ReturnedAction);

                if (pointa <= 4) { }
                else { setPointa(pointa - 4); }
                targetIconNameText.text = targetIconName;
                moveKersol();
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (pointa >= 23) { setPointa(26); }
                else if (pointa >= 21) { setPointa(25); }
                else { setPointa(pointa + 4); }
                targetIconNameText.text = targetIconName;
                moveKersol();
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                inclementPointa();
                targetIconNameText.text = targetIconName;
                moveKersol();
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (pointa % 4 == 1)
                {
                    DestroyThisCanvas();
                }
                else {
                    declementPointa();
                    targetIconNameText.text = targetIconName;
                    moveKersol();
                }
            }
            if (Input.GetButtonDown("Submit"))
            {
                if (pointa <= 4) { currentInventory = pointa; setupInventory(); }
                else if (pointa == 25) { currentPage--; setupInventory(); setPointa(25); }
                else if (pointa == 26) { currentPage++; setupInventory(); setPointa(26); }
                else
                {
                    targetSTM = Target.GetComponent<SelectableTargetManager>();
                    if (targetSTM.TargetIcon != null)
                    {
                        if (TargetIconType == (int)IconTypeList.Weapon && targetSTM.TargetIcon.IconType == (int)IconTypeList.Weapon)
                        {
                            //equipcanvas.ReturnedWeapon = playerManager.WeaponBag.GetChild(pointa - 4 - ((currentPage - 1) * 20) - 1).GetComponent<AWeapon>();
                            DestroyThisCanvas();
                        }
                        else if (TargetIconType == (int)IconTypeList.Ring && targetSTM.TargetIcon.IconType == (int)IconTypeList.Ring)
                        {
                            //equipcanvas.ReturnedWeapon = playerManager.WeaponBag.GetChild(pointa - 4 - ((currentPage - 1) * 20) - 1).GetComponent<ARing>();
                            DestroyThisCanvas();
                        }
                        else if (TargetIconType == (int)IconTypeList.Action)
                        {
                            if (targetSTM.TargetIcon.IconType == (int)IconTypeList.Mind)
                            {
                                targetMind = playerManager.Mind.GetChild(pointa - 4 - 1 - ((currentPage - 1) * 20)).GetComponent<AMind>();
                                nextCanvas = clickTarget();
                                if (nextCanvas != null) { nextCanvas.GetComponent<ACanvasManager>().SetBackCanvas(this); }
                            }
                            else if (targetSTM.TargetIcon.IconType == (int)IconTypeList.Item)
                            {
                                backCanvas.ReturnedAction = playerManager.ItemBag.GetChild(pointa - 4 - 1 - ((currentPage - 1) * 20)).GetComponent<AAction>();
                                DestroyThisCanvas();
                            }
                            else if (targetSTM.TargetIcon.IconType == (int)IconTypeList.Weapon)
                            {
                                backCanvas.ReturnedAction = playerManager.WeaponBag.GetChild(pointa - 4 - 1 - ((currentPage - 1) * 20)).GetComponent<AAction>();
                                DestroyThisCanvas();
                            }
                            else if (targetSTM.TargetIcon.IconType == (int)IconTypeList.Ring)
                            {
                                backCanvas.ReturnedAction = playerManager.RingBag.GetChild(pointa - 4 - 1 - ((currentPage - 1) * 20)).GetComponent<AAction>();
                                DestroyThisCanvas();
                            }
                        }
                        else if (TargetIconType == (int)IconTypeList.Mind && targetSTM.TargetIcon.IconType == (int)IconTypeList.Mind)
                        {
                            backCanvas.ReturnedMindGO = playerManager.MindBag.GetChild(pointa - 4 - 1 - ((currentPage - 1) * 20)).gameObject;
                            DestroyThisCanvas();
                        }
                        else
                        {
                            nextCanvas = clickTarget();
                            if (nextCanvas != null) { nextCanvas.GetComponent<ACanvasManager>().SetBackCanvas(this); }
                        }
                    }
                }
            }
            if (Input.GetButtonDown("Attack"))
            {
                if (Target.GetComponent<SelectableTargetManager>().TargetIcon != null)
                {
                    nextCanvas = clickTarget();
                    if (nextCanvas != null) { nextCanvas.GetComponent<ACanvasManager>().SetBackCanvas(this); }
                }
            }
            if (Input.GetButtonDown("Cancel"))
            {
                DestroyThisCanvas();
            }
        }
    }

}
