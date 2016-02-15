﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PlayerCanvasManager : ACanvasManager {

    private PlayerManager playerManager;
    private RectTransform HPBar;
    private Text HPText;
    private RectTransform SPBar;
    private Text SPText;
    private ABuff targetBuff;
    // This is not <string> for destroying Instantiated GameObject
    public List<SelectableTargetManager> buffListSTM = new List<SelectableTargetManager>();

    protected override void Awake()
    {
        playerManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
        HPBar = transform.FindChild("HPBar").GetComponent<RectTransform>();
        HPText = transform.FindChild("HPText").GetComponent<Text>();
        SPBar = transform.FindChild("SPBar").GetComponent<RectTransform>();
        SPText = transform.FindChild("SPText").GetComponent<Text>();
        myKersolRect = transform.FindChild("Kersol").GetComponent<RectTransform>();
        pointa = 0;
        pointaNUM = 0;
        kersolPOSfix = new Vector3(-4, -4, 0);
        firstpointa = 0;

        initPointaAndKersol();
    }

    // Update is called once per frame
    void Update () {
        int hpbarwid = (int)(250 * playerManager.HP / playerManager.MaxHP);
        int spbarwid = (int)(250 * playerManager.SP / playerManager.MaxSP);
        HPBar.sizeDelta = new Vector2(hpbarwid, HPBar.sizeDelta.y);
        HPText.text = "HP    " + Mathf.RoundToInt(playerManager.HP).ToString(); HPText.GetComponent<TextShade>().TextUpdate();
        SPBar.sizeDelta = new Vector2(spbarwid, SPBar.sizeDelta.y);
        SPText.text = "SP    " + Mathf.RoundToInt(playerManager.SP).ToString(); SPText.GetComponent<TextShade>().TextUpdate();

        if (playerManager.Buffs.transform.childCount > 0) { updateBuffs(); }

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
    private void updateBuffs()
    {
        for (int i = 0; i < playerManager.Buffs.transform.childCount; i++)
        {
            targetBuff = playerManager.Buffs.transform.GetChild(i).GetComponent<ABuff>();
            if (targetBuff.IsDrawn)
            {
                if (targetBuff.Sands <= 0.0f)
                {
                    for (int j = 0; j < buffListSTM.Count; j++) {
                        if (buffListSTM[j].TargetIcon.Name == targetBuff.Name) {
                            Destroy(buffListSTM[j].gameObject); buffListSTM.RemoveAt(j);
                        }
                    }
                    Destroy(targetBuff.gameObject);
                }
            }
            else
            {
                if (buffListSTM.Count == 0)
                {
                    GameObject temp = (GameObject)Instantiate(Resources.Load("Prefabs/GUI/SelectableTarget"), Vector3.zero, Quaternion.identity);
                    temp.transform.SetParent(transform);
                    temp.GetComponent<RectTransform>().position = new Vector3(700 + (24 * i), 60, 0);
                    buffListSTM.Add(temp.GetComponent<SelectableTargetManager>());
                    buffListSTM[buffListSTM.Count - 1].TargetIcon = targetBuff.GetComponent<AIcon>();
                    buffListSTM[buffListSTM.Count - 1].Icon = targetBuff.Icon;

                    targetBuff.IsDrawn = true;
                }
                for (int j = 0; j < buffListSTM.Count; j++)
                {
                    if (buffListSTM[j].TargetIcon.Name == targetBuff.Name)
                    {
                        targetBuff.Sands = targetBuff.Duration; targetBuff.IsDrawn = true;
                    }
                    else
                    {
                        GameObject temp = (GameObject)Instantiate(Resources.Load("Prefabs/GUI/SelectableTarget"), Vector3.zero, Quaternion.identity);
                        temp.transform.SetParent(transform);
                        temp.GetComponent<RectTransform>().position = new Vector3(700 + (24 * i), 60, 0);
                        buffListSTM.Add(temp.GetComponent<SelectableTargetManager>());
                        buffListSTM[buffListSTM.Count - 1].TargetIcon = targetBuff.GetComponent<AIcon>();
                        buffListSTM[buffListSTM.Count - 1].Icon = targetBuff.Icon;

                        targetBuff.IsDrawn = true;
                    }
                }
            }
        }
    }

}
