using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class EquipmentMenuCanvasManager : ACanvasManager
{
    //    public AWeapon ReturnedWeapon = null;
    //    public ARing ReturnedRing = null;
    public AAction ReturnedAction = null;

    private PlayerManager playerManager;
    private Text targetIconNameText;
    
    // Use this for initialization
    protected override void Awake()
    {
        myKersolRect = transform.FindChild("Kersol").GetComponent<RectTransform>();
        pointa = 0;
        pointaNUM = 21;
        kersolPOSfix = new Vector3(-4, -4, 0);
        firstpointa = 1;

        playerManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
        targetIconNameText = gameObject.transform.FindChild("TargetNameText").GetComponent<Text>();

        setupEquipmentMenu();
        initPointaAndKersol();
        targetIconNameText.text = targetIconName;
        moveKersol();
    }

    private void setupEquipmentMenu()
    {
        // Actions
        for (int n = 1; n <= 8; n++)
        {
            try
            {
                if (playerManager.actionShortcuts[n].GetComponent<AIcon>())
                {
                    AIcon actionsIcon = playerManager.actionShortcuts[n].GetComponent<AIcon>();
                    Debug.Log(actionsIcon);
                    setPointa(2 + n);
                    Target.GetComponent<SelectableTargetManager>().TargetIcon = actionsIcon;
                    Target.GetComponent<AIcon>().Icon = actionsIcon.Icon;
                }
            }
            catch (Exception) { Debug.Log("Missing action."); }
        }
        // Minds
        for (int n = 0; n <= playerManager.Mind.transform.childCount-1; n++)
        {
            if (n > playerManager.MindSlots) { }
            else 
            {
                AIcon mindsIcon = playerManager.Mind.transform.GetChild(n).GetComponent<AIcon>();
                setPointa(11+n);
                Target.GetComponent<SelectableTargetManager>().TargetIcon = mindsIcon;
                Target.GetComponent<AIcon>().Icon = mindsIcon.Icon;
            }
        }
        for (int n = 21; n >= 11 + playerManager.MindSlots; n--)
        {
            setPointa(n);
            Target.SetActive(false);
        }
        pointaNUM = 10 + playerManager.MindSlots;
    }

    // Update is called once per frame
    void Update()
    {
        if (nextCanvas != null)
        {
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (pointa == 3 || pointa == 4) { setPointa(1); }
                else if (pointa == 5 || pointa == 6) { setPointa(2); }
                else { setPointa(pointa - 4); }
                targetIconNameText.text = targetIconName;
                moveKersol();
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (pointa == 1) { setPointa(3); }
                else if (pointa == 2) { setPointa(5); }
                else { setPointa(pointa + 4); }
                targetIconNameText.text = targetIconName;
                moveKersol();
            }
            if (Input.GetKeyDown(KeyCode.RightArrow)) { inclementPointa(); targetIconNameText.text = targetIconName; moveKersol(); }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (pointa <= 1 || pointa % 4 == 3)
                {
                    DestroyThisCanvas();
                }
                else { declementPointa(); targetIconNameText.text = targetIconName; moveKersol(); }
            }
            if (Input.GetButtonDown("Submit"))
            {
                if (pointa == 1) { }// Weapon
                else if (pointa == 2) { }// Ring
                else if (pointa >= 3 && pointa <= 10)// Actions
                {

                }
                else
                {
                    if (Target.GetComponent<SelectableTargetManager>().TargetIcon != null)
                    {
                        nextCanvas = clickTarget();
                        if (nextCanvas != null) { nextCanvas.GetComponent<ACanvasManager>().SetBackCanvas(this); }
                    }
                }
            }
        }
    }

}
