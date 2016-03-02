using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using IconAndErrorType;

public class EquipmentMenuCanvasManager : ACanvasManager
{
    private PlayerManager playerManager;
    private Sprite nullSprite;
    private Text targetIconNameText;
    private GameObject returnIMC = null;

    // Use this for initialization
    protected override void Awake()
    {
        myKersolRect = transform.FindChild("Kersol").GetComponent<RectTransform>();
        pointa = 0;
        pointaNUM = 21;
        kersolPOSfix = new Vector3(-4, -4, 0);
        firstpointa = 1;

        playerManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
        nullSprite = Resources.Load<Sprite>("Images/GUI/glass_black");
        targetIconNameText = gameObject.transform.FindChild("TargetNameText").GetComponent<Text>();
        returnIMC = (GameObject)Resources.Load("Prefabs/GUI/ReturnInventoryMenuCanvas");

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
                setPointa(2 + n);
                Target.GetComponent<SelectableTargetManager>().TargetIcon = null;
                Target.GetComponent<AIcon>().Icon = nullSprite;
                Target.GetComponent<SelectableTargetManager>().SetNumber(0);
                if (playerManager.actionShortcuts[n].GetComponent<AIcon>())
                {
                    AIcon actionIcon = playerManager.actionShortcuts[n].GetComponent<AIcon>();
                    Target.GetComponent<SelectableTargetManager>().TargetIcon = actionIcon;
                    Target.GetComponent<AIcon>().Icon = actionIcon.Icon;
                    if (actionIcon.CanTogether) {
                        Target.GetComponent<SelectableTargetManager>().SetNumber(actionIcon.Number);
                    }
                }
            }
            catch (Exception) { Debug.Log("Missing action."); }
        }
        // Minds
        for (int n = 0; n <= playerManager.Mind.transform.childCount-1; n++)
        {
            setPointa(11 + n);
            Target.GetComponent<SelectableTargetManager>().TargetIcon = null;
            Target.GetComponent<AIcon>().Icon = nullSprite;
            Target.GetComponent<SelectableTargetManager>().SetNumber(0);

            if (n > playerManager.MindSlots) { }
            else 
            {
                AIcon mindsIcon = playerManager.Mind.transform.GetChild(n).GetComponent<AIcon>();
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
        playerManager.UsePassiveActions();

    }

    // Update is called once per frame
    void Update()
    {
        if (ReturnedAction != null)
        {
            if (ReturnedAction.IsPassive)
            {
                GameObject ecanvas = Instantiate((GameObject)Resources.Load("Prefabs/GUI/ErrorTextCanvas"));
                ecanvas.GetComponent<ErrorTextCanvasManager>().SetAndDestroy((int)ErrorTypeList.IsPassive);
            }
            else
            {
                if (pointa <= 2 || pointa >= 11) { }
                else { playerManager.actionShortcuts[pointa - 2] = ReturnedAction; }
                ReturnedAction = null;
                setupEquipmentMenu();
                initPointaAndKersol();
                moveKersol();
            }
        }
        if (ReturnedMindGO != null)
        {
            Debug.Log("ReturnedMind is not null");
            if (pointa <= 10 || pointa >= 22) { }
            else
            {
                GameObject newmind = (GameObject)Instantiate(ReturnedMindGO);
                newmind.transform.SetParent(playerManager.Mind);
            }
            ReturnedMindGO = null;
            setupEquipmentMenu();
            initPointaAndKersol();
            moveKersol();
        }

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
                    GameObject retcanvas = Instantiate(returnIMC);
                    retcanvas.GetComponent<ReturnInventoryMenuCanvasManager>().TargetIconType = (int)IconTypeList.Action;
                    nextCanvas = retcanvas.GetComponent<ACanvasManager>();
                    if (nextCanvas != null) { nextCanvas.GetComponent<ACanvasManager>().SetBackCanvas(this); }
                }
                else if (pointa >= 11) // Mind
                {
                    GameObject retcanvas = Instantiate(returnIMC);
                    retcanvas.GetComponent<ReturnInventoryMenuCanvasManager>().TargetIconType = (int)IconTypeList.Mind;
                    nextCanvas = retcanvas.GetComponent<ACanvasManager>();
                    if (nextCanvas != null) { nextCanvas.GetComponent<ACanvasManager>().SetBackCanvas(this); }
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
            if (Input.GetButtonDown("Guard"))
            {
                if (pointa == 1) { }// Weapon
                else if (pointa == 2) { }// Ring
                else if (pointa >= 3 && pointa <= 10)// Actions
                {
                    playerManager.actionShortcuts[pointa - 2] = null;
                    setupEquipmentMenu();
                    initPointaAndKersol();
                    moveKersol();
                }
                /*                else if (pointa >= 11) // Mind
                                {
                                    GameObject target = playerManager.Mind.GetChild(pointa - 11).gameObject;
                                    GameObject copy = Instantiate(target);
                                    copy.transform.SetParent(playerManager.MindBag);
                                    Destroy(target);
                                    setupEquipmentMenu();
                                    initPointaAndKersol();
                                    moveKersol();
                                }
                */
            }
        }
    }

}
