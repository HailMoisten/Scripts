using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class InventoryMenuCanvasManager : ACanvasManager
{

    private PlayerManager playerManager;
    private Text targetIconNameText;
    private Sprite nullSprite;
    private int currentInventory = 1;
    private int currentPage = 1;

    // Use this for initialization
    protected override void Awake()
    {
        myKersolRect = transform.FindChild("Kersol").GetComponent<RectTransform>();
        pointa = 1;
        pointaNUM = 26;
        kersolPOSfix = new Vector3(-4, -4, 0);
        firstpointa = 1;

        playerManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
        targetIconNameText = gameObject.transform.FindChild("TargetNameText").GetComponent<Text>();
        nullSprite = Resources.Load<Sprite>("Images/GUI/glass_white");

        setupInventory();
        initPointaAndKersol();
        targetIconNameText.text = targetIconName;
        moveKersol();
    }
    private void setupInventory()
    {
        Transform target = null;
        if (currentInventory <= 1) { target = playerManager.ItemBag; }
        else if (currentInventory == 2) { target = playerManager.WeaponBag; }
        else if (currentInventory == 3) { target = playerManager.RingBag; }
        else if (currentInventory >= 4) { target = playerManager.MindBag; }
        int pages = 1 + (target.childCount / 20);
        if (currentPage > pages) { currentPage = pages; } else if (currentPage < 1){ currentPage = 1; }
        int pagehead = (currentPage - 1) * 20;
        for (int n = 0; n <= 19; n++)
        {
            setPointa(5 + n);
            Target.GetComponent<SelectableTargetManager>().TargetIcon = null;
            Target.GetComponent<AIcon>().Icon = nullSprite;
            try
            {
                if (n + pagehead <= target.childCount - 1)
                {
                    AIcon targeticon = target.GetChild(n + pagehead).GetComponent<AIcon>();
                    Target.GetComponent<SelectableTargetManager>().TargetIcon = targeticon;
                    Target.GetComponent<AIcon>().Icon = targeticon.Icon;
                }
            }
            catch (Exception) { Debug.Log("wrong calc"); }
        }
        setPointa(currentInventory);
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
                if (pointa <= 4) { }
                else { setPointa(pointa - 4); }
                targetIconNameText.text = targetIconName;
                moveKersol();
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (pointa >= 23) { setPointa(26); } else if (pointa >= 21) { setPointa(25); }
                else { setPointa(pointa + 4); }
                targetIconNameText.text = targetIconName;
                moveKersol();
            }
            if (Input.GetKeyDown(KeyCode.RightArrow)) {
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
