using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EquipmentMenuCanvasManager : ACanvasManager
{

    private PlayerManager playerManager;
    
    // Use this for initialization
    protected override void Start()
    {
        myKersolRect = transform.FindChild("Kersol").GetComponent<RectTransform>();
        pointa = 0;
        pointaNUM = 21;
        kersolPOSfix = new Vector3(-4, -4, 0);
        firstpointa = 0;

        playerManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();

        setupEquipmentMenu();
        initPointaAndKersol();
    }

    private void setupEquipmentMenu()
    {
        // Actions
        for (int n = 1; n <= 1; n++)
        {
            if (playerManager.mindSkillShortcuts[n].GetComponent<AIcon>() != null)
            {
                AIcon actionsIcon = playerManager.mindSkillShortcuts[n].GetComponent<AIcon>();
                setPointa(2 + n);
                Target.GetComponent<SelectableTargetManager>().TargetIcon = actionsIcon;
                Target.GetComponent<AIcon>().Icon = actionsIcon.Icon;
            }
        }
        // Minds
        for (int n = 0; n <= playerManager.Minds.transform.childCount-1; n++)
        {
            if (n > playerManager.MindSlots) { }
            else if (playerManager.Minds.transform.GetChild(n).GetComponent<AIcon>() != null)
            {
                AIcon mindsIcon = playerManager.Minds.transform.GetChild(n).GetComponent<AIcon>();
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
        pointaNUM = 10 + (int)playerManager.MindSlots;
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
                moveKersol();
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (pointa == 1) { setPointa(3); }
                else if (pointa == 2) { setPointa(5); }
                else { setPointa(pointa + 4); }
                moveKersol();
            }
            if (Input.GetKeyDown(KeyCode.RightArrow)) { inclementPointa(); moveKersol(); }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (pointa <= 1 || pointa % 4 == 3)
                {
                    DestroyThisCanvas();
                }
                else { declementPointa(); moveKersol(); }
            }
            if (Input.GetButtonDown("Submit"))
            {
                nextCanvas = clickTarget();
                if (nextCanvas != null) { nextCanvas.GetComponent<ACanvasManager>().SetBackCanvas(this); }
            }
        }
    }

}
