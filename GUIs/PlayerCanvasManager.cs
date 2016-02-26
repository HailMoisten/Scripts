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
    private bool needUpdateHPSP;
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
        myKersolRect = transform.FindChild("Kersol").GetComponent<RectTransform>();
        pointa = 0;
        pointaNUM = 0;
        kersolPOSfix = new Vector3(-4, -4, 0);
        firstpointa = 0;

        playerManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
        myCamera = GameObject.Find("Camera").GetComponent<Camera>();
        initPointaAndKersol();
        needUpdateHPSP = true;
        needUpdateBuffs = true;
    }

    // Update is called once per frame
    void Update () {
        updateHPSP();
        updateBuffs();
        if (playerManager.SubmitAction != null) {
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

        if (nextCanvas != null) { }
        else
        {
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

    private int hpbarwid, spbarwid = 0;
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
                                    buffListSTM[j].SetNumber(Mathf.FloorToInt(targetBuff.Sands));
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

}
