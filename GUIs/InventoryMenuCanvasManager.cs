using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InventoryMenuCanvasManager : ACanvasManager
{

    private PlayerManager playerManager;
    private Text targetIconNameText;

    // Use this for initialization
    protected override void Awake()
    {
        myKersolRect = transform.FindChild("Kersol").GetComponent<RectTransform>();
        pointa = 1;
        pointaNUM = 24;
        kersolPOSfix = new Vector3(-4, -4, 0);
        firstpointa = 1;

        playerManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
        targetIconNameText = gameObject.transform.FindChild("TargetNameText").GetComponent<Text>();

        setupInventory(1, 1);
        initPointaAndKersol();
        targetIconNameText.text = targetIconName;
        moveKersol();
    }
    private void setupInventory(int type, int page)
    {

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
                if (pointa <= 4) { }
                else { targetIconNameText.text = targetIconName; }
                moveKersol();
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (pointa >= 24) { }
                else { setPointa(pointa + 4); }
                if (pointa <= 4) { }
                else { targetIconNameText.text = targetIconName; }
                moveKersol();
            }
            if (Input.GetKeyDown(KeyCode.RightArrow)) {
                inclementPointa();
                if (pointa <= 4) { }
                else { targetIconNameText.text = targetIconName; }
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
                    if (pointa <= 4) { }
                    else { targetIconNameText.text = targetIconName; }
                    moveKersol();
                }
            }
            if (Input.GetButtonDown("Submit"))
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
