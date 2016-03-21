using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PlayerCanvasManager : ACanvasManager {

    private PlayerManager playerManager;
    private Camera myCamera;
    private RectTransform HPBar;
    private RectTransform HPEnd;
    private Text HPText;
    private RectTransform SPBar;
    private RectTransform SPEnd;
    private Text SPText;
    private Image BattleReadyOff;
    private Image BattleReadyOn;
    private bool lastBattleReady;
    private Image CurrentActionIcon;
    private Image NextActionIcon;
    private Sprite nullIcon;
    private bool seakingSTM;
    private GameObject seakingSTMModeText;
    private Text CurrentRunText;
    private Text CurrentJumpText;
    private bool needUpdateBuffs;
    private ABuff targetBuff;
    // This is not <string> for destroying Instantiated GameObject
    public List<SelectableTargetManager> buffListSTM = new List<SelectableTargetManager>();
    private GameObject submitActionPopUp = null;

    protected override void Awake()
    {
        HPBar = transform.FindChild("HPBar").GetComponent<RectTransform>();
        HPEnd = HPBar.transform.FindChild("HPEnd").GetComponent<RectTransform>();
        HPText = transform.FindChild("HPText").GetComponent<Text>();
        SPBar = transform.FindChild("SPBar").GetComponent<RectTransform>();
        SPEnd = SPBar.transform.FindChild("SPEnd").GetComponent<RectTransform>();
        SPText = transform.FindChild("SPText").GetComponent<Text>();
        BattleReadyOff = transform.FindChild("BattleReadyOff").GetComponent<Image>();
        BattleReadyOn = transform.FindChild("BattleReadyOn").GetComponent<Image>();
        lastBattleReady = true;
        CurrentActionIcon = transform.FindChild("CurrentActionIcon").GetComponent<Image>();
        NextActionIcon = transform.FindChild("NextActionIcon").GetComponent<Image>();
        nullIcon = Resources.Load<Sprite>("Images/GUI/glass_black");
        seakingSTMModeText = transform.FindChild("SeakingSTMModeText").gameObject;
        seakingSTMModeText.SetActive(false);
        CurrentRunText = transform.FindChild("CurrentRunText").GetComponent<Text>();
        CurrentJumpText = transform.FindChild("CurrentJumpText").GetComponent<Text>();

        myKersolRect = transform.FindChild("Kersol").GetComponent<RectTransform>();
        pointa = 0;
        pointaNUM = 0;
        kersolPOSfix = new Vector3(-4, -4, 0);
        firstpointa = 0;

        playerManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
        myCamera = GameObject.Find("Camera").GetComponent<Camera>();
        initPointaAndKersol();
        needUpdateBuffs = true;
    }

    private bool updateActionStackFlag = false;
    // Update is called once per frame
    void Update () {
        updateHPSP();
        updateBuffs();
        updateSubmitAction();
        updateBattleReady();
        updateCurrentRunJump();

        updateActionStackFlag = !updateActionStackFlag;
        if (updateActionStackFlag) { updateActionStack(); }

        if (nextCanvas != null) { }
        else
        {
            if (seakingSTM)
            {
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    inclementPointa();
                    moveKersol();
                }
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    declementPointa();
                    moveKersol();
                }
                if (Input.GetButtonDown("Attack") || Input.GetButtonDown("Submit"))
                {
                    if (Target.GetComponent<SelectableTargetManager>().TargetIcon != null)
                    {
                        nextCanvas = clickTarget();
                        if (nextCanvas != null) { nextCanvas.GetComponent<ACanvasManager>().SetBackCanvas(this); }
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                seakSTMToggle();
            }
            if (Input.GetButtonDown("Options"))
            {
                openMenu();
            }
        }

    }
    private void openMenu()
    {
        playerManager.isMenuAwake = true;
        Time.timeScale = 0.0f;
        nextCanvas = Instantiate((GameObject)Resources.Load("Prefabs/GUI/MenuCanvas")).GetComponent<ACanvasManager>();
        nextCanvas.GetComponent<ACanvasManager>().SetBackCanvas(this);
    }
    public void CloseMenu()
    {
        playerManager.isMenuAwake = false;
        Time.timeScale = 1.0f;
        nextCanvas.DestroyThisCanvas();
    }
    private void seakSTMToggle()
    {
        playerManager.isMenuAwake = !playerManager.isMenuAwake;
        if (playerManager.isMenuAwake)
        {
            seakingSTM = true;
            seakingSTMModeText.SetActive(true);
            Time.timeScale = 0.0f;
            pointaNUM = playerManager.Buffs.childCount;
            firstpointa = 1;
            initPointaAndKersol();
            moveKersol();
        }
        else
        {
            seakingSTM = false;
            seakingSTMModeText.SetActive(false);
            Time.timeScale = 1.0f;
            if (nextCanvas != null)
            {
                nextCanvas.DestroyThisCanvas();
            }
            initPointaAndKersol();
        }

    }
    private int hpbarwid, spbarwid;
    private void updateHPSP()
    {
        hpbarwid = (int)(250 * playerManager.HP / playerManager.MaxHP);
        spbarwid = (int)(250 * playerManager.SP / playerManager.MaxSP);
        HPBar.sizeDelta = new Vector2(hpbarwid, HPBar.sizeDelta.y);
        HPEnd.localPosition = new Vector3(hpbarwid, 0, 0);
        HPText.text = "HP    " + Mathf.RoundToInt(playerManager.HP).ToString(); HPText.GetComponent<TextShade>().TextUpdate();
        SPBar.sizeDelta = new Vector2(spbarwid, SPBar.sizeDelta.y);
        SPEnd.localPosition = new Vector3(spbarwid, 0, 0);
        SPText.text = "SP    " + Mathf.RoundToInt(playerManager.SP).ToString(); SPText.GetComponent<TextShade>().TextUpdate();
    }
    private void updateBuffs()
    {
        if (playerManager.Buffs.transform.childCount == 0 &&
            !needUpdateBuffs)
        { }
        else
        {
            if (playerManager.Buffs.transform.childCount == 0)
            {
                for (int j = 0; j < buffListSTM.Count; j++)
                {
                    Destroy(buffListSTM[j].gameObject);
                }
                buffListSTM.Clear();
                needUpdateBuffs = false;
            }
            else
            {
                needUpdateBuffs = true;
                // Update buffs which exist in Buffs.
                for (int i = 0; i < playerManager.Buffs.transform.childCount; i++)
                {
                    targetBuff = playerManager.Buffs.transform.GetChild(i).GetComponent<ABuff>();
                    if (targetBuff.IsDrawn)
                    {
                        if (targetBuff.IsToggle) { }
                        else
                        {
                            for (int j = 0; j < buffListSTM.Count; j++)
                            {
                                if (buffListSTM[j].TargetIcon.Name == targetBuff.Name)
                                {
                                    buffListSTM[j].SetNumber(Mathf.CeilToInt(targetBuff.Sands));
                                }
                            }
                        }
                    }
                    else
                    {
                        GameObject temp = (GameObject)Instantiate(Resources.Load("Prefabs/GUI/SelectableTarget"), Vector3.zero, Quaternion.identity);
                        temp.transform.SetParent(transform);
                        temp.GetComponent<RectTransform>().position = new Vector3(700 + (44 * i), 60, 0);
                        buffListSTM.Add(temp.GetComponent<SelectableTargetManager>());
                        buffListSTM[buffListSTM.Count - 1].TargetIcon = targetBuff.GetComponent<AIcon>();
                        buffListSTM[buffListSTM.Count - 1].Icon = targetBuff.Icon;

                        targetBuff.IsDrawn = true;
                    }
                }

                // Delete buffs which do not exist in Buffs
                for (int j = 0; j < buffListSTM.Count; j++)
                {
                    bool exist = false;
                    for (int i = 0; i < playerManager.Buffs.transform.childCount; i++)
                    {
                        targetBuff = playerManager.Buffs.transform.GetChild(i).GetComponent<ABuff>();
                        if (buffListSTM[j].TargetIcon.Name == targetBuff.Name)
                        { exist = true; }
                    }
                    if (exist) { }
                    else
                    {
                        Destroy(buffListSTM[j].gameObject); buffListSTM.RemoveAt(j);
                    }
                    buffListSTM[j].GetComponent<RectTransform>().position = new Vector3(700 + (44 * j), 60, 0);
                }

            }

        }
    }
    private void updateSubmitAction()
    {
        if (playerManager.SubmitAction != null)
        {
            if (submitActionPopUp == null)
            {
                GameObject sat = (GameObject)Resources.Load("Prefabs/GUI/SubmitActionPopUp");
                sat.transform.GetChild(0).GetComponent<Text>().text = playerManager.SubmitAction.Name;
                submitActionPopUp = Instantiate(sat);
                submitActionPopUp.transform.SetParent(transform);
                submitActionPopUp.transform.GetChild(0).GetComponent<TextShade>().TextUpdate();
            }
        }
        else
        {
            if (submitActionPopUp != null) { Destroy(submitActionPopUp); }
        }
    }

    private void updateBattleReady()
    {
        if (lastBattleReady == playerManager.BattleReady) { }
        else
        {
            lastBattleReady = playerManager.BattleReady;
            if (lastBattleReady)
            {
                BattleReadyOn.enabled = true;
                BattleReadyOff.enabled = false;
            }
            else
            {
                BattleReadyOn.enabled = false;
                BattleReadyOff.enabled = true;
            }
        }
    }
    private string lastCurrentActionName = "";
    private string lastNextActionName = "";
    private void updateActionStack()
    {
        if (playerManager.actionStack[0] == null) {
            if (lastCurrentActionName == "null") { }
            else { CurrentActionIcon.sprite = nullIcon; lastCurrentActionName = "null"; }
        }
        else if (lastCurrentActionName == playerManager.actionStack[0].Name) { }
        else
        {
            CurrentActionIcon.sprite = playerManager.actionStack[0].Icon;
            lastCurrentActionName = playerManager.actionStack[0].Name;
        }

        if (playerManager.actionStack[1] == null)
        {
            if (lastNextActionName == "null") { }
            else { NextActionIcon.sprite = nullIcon; lastNextActionName = "null"; }
        }
        else if (lastNextActionName == playerManager.actionStack[1].Name) { }
        else
        {
            NextActionIcon.sprite = playerManager.actionStack[1].Icon;
            lastNextActionName = playerManager.actionStack[1].Name;
        }
    }
    int lastcurRun = 0; int lastcurJump = 0;
    private void updateCurrentRunJump()
    {
        if (lastcurRun == playerManager.CurrentRun) { }
        else
        {
            lastcurRun = playerManager.CurrentRun;
            CurrentRunText.text = "";
            for (int i = 1; i <= playerManager.CurrentRun; i++) { CurrentRunText.text += "▶"; }
        }
        if (lastcurJump == playerManager.CurrentJump) { }
        else
        {
            lastcurJump = playerManager.CurrentJump;
            CurrentJumpText.text = "";
            if (lastcurJump == 0) { }
            else { for (int j = 1; j <= playerManager.CurrentJump; j++) { CurrentJumpText.text += "▶"; } }
        }
    }
    public void PickUpPopUp(AItem targetitem)
    {
        nextCanvas = Instantiate((GameObject)Resources.Load("Prefabs/GUI/PickUpPopUpCanvas")).GetComponent<ACanvasManager>();
        nextCanvas.GetComponent<ACanvasManager>().SetBackCanvas(this);
        nextCanvas.transform.FindChild("SelectableTarget").GetComponent<SelectableTargetManager>().Icon = targetitem.Icon;
        nextCanvas.transform.FindChild("SelectableTarget").GetComponent<SelectableTargetManager>().TargetIcon = targetitem.GetComponent<AIcon>();
        nextCanvas.transform.FindChild("NameText").GetComponent<Text>().text = targetitem.Name;
        nextCanvas.transform.FindChild("NameText").GetComponent<TextShade>().TextUpdate();
        nextCanvas.transform.FindChild("NumText").GetComponent<Text>().text = "x " + targetitem.Number;
        nextCanvas.transform.FindChild("NumText").GetComponent<TextShade>().TextUpdate();
    }
    public void ShowInformationText(string content)
    {
        GameObject infotext = Instantiate((GameObject)Resources.Load("Prefabs/GUI/InformationText"));
        infotext.transform.SetParent(transform);
        infotext.GetComponent<RectTransform>().localPosition += Vector3.down * infotextcount * 24;
        infotext.GetComponent<Text>().text = content;
        infotext.GetComponent<TextShade>().TextUpdate();
        StartCoroutine(InformationTextDestroy(infotext));
    }
    private int infotextcount = 0;
    private IEnumerator InformationTextDestroy(GameObject infotext)
    {
        infotextcount++;
        yield return new WaitForSeconds(2.0f);
        Destroy(infotext);
        infotextcount--;
    }
}
