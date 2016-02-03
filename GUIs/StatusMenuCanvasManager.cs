﻿using UnityEngine;
using UnityEngine.UI;

public class StatusMenuCanvasManager : ACanvasManager
{

    private int subPointa = 0;
    private int subPointaNUM = 5;

    private PlayerManager playerManager;

    protected override void Start()
    {
        myKersolRect = transform.FindChild("Kersol").GetComponent<RectTransform>();
        pointa = 0;
        pointaNUM = 15;
        kersolPOSfix = new Vector3(44, 0, 0);
        firstpointa = 0;

        playerManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();

        setupStatusMenu();
        initPointaAndKersol();
    }
    private void setupStatusMenu()
    {
        setupColorLevelUpReward();
        // Level
        int level = playerManager.GetLevel();
        GameObject.Find("LevelText").GetComponent<Text>().text = "" + level;
        // Main Status
        int[] mains = playerManager.GetMainStatus();
        GameObject.Find("MainStatusText").GetComponent<Text>().text =
            mains[0] + "\n" + mains[1] + "\n" + mains[2] + "\n" + mains[3] + "\n" + mains[4] + "\n";
        // Sub Status
        float[] subs = playerManager.GetSubStatus();
        string[] floatsubs = new string[4];
        floatsubs[0] = string.Format("{0:f2}\r", subs[5]); floatsubs[1] = string.Format("{0:f2}\r", subs[6]);
        floatsubs[2] = string.Format("{0:f1}\r", subs[8]); floatsubs[3] = string.Format("{0:f1}\r", subs[11]);
        GameObject.Find("SubStatusText").GetComponent<Text>().text = 
            subs[0] + "\n" + subs[1] + "\n" + subs[2] + "\n" + subs[3] + "\n" + subs[4] + "\n" +
            floatsubs[0] + "\n" + floatsubs[1] + "\n" +
            subs[7] + " + " + floatsubs[2] + " / " + subs[9] + "\n" +
            subs[10] + " + " + floatsubs[3] + " / " + subs[12] + "\n" +
            subs[13] + "\n";
    }

    void Update()
    {
        // Inputs(On Menu)
        Vector3 menuDIR = new Vector3(0, 0, 0);
        menuDIR.x = Input.GetAxisRaw("Horizontal");
        menuDIR.z = Input.GetAxisRaw("Vertical");
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (subPointa == 0) { }
            else
            {
                playerManager.SetLevelUpReward(subPointa - 1, 1);
                setColorLevelUpReward();
                setPointa(subPointa * 3 - 2);
                moveKersol();
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (subPointa == 0) { }
            else
            {

                playerManager.SetLevelUpReward(subPointa - 1, -1);
                setColorLevelUpReward();
                setPointa(subPointa * 3 - 1);
                moveKersol();
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (subPointa >= subPointaNUM) { }
            else { subPointa++; setPointa(subPointa * 3); moveKersol(); }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (subPointa <= 1)
            {
                DestroyThisCanvas();
            }
            else { subPointa--; setPointa(subPointa * 3); moveKersol(); }
        }
    }

    private void setupColorLevelUpReward()
    {
        int[] p = playerManager.GetLevelUpReward();
        for (int i = 0; i < p.Length; i++)
        {
            setPointa((i + 1) * 3);
            Target.GetComponent<SelectableTargetManager>().setColor0to3(p[i]);
        }
        setPointa(subPointa * 3);
    }
    private void setColorLevelUpReward()
    {
        int[] p = playerManager.GetLevelUpReward();
        for (int i = 0; i < p.Length; i++)
        {
            setPointa((i + 1) * 3);
            moveKersol();
            Target.GetComponent<SelectableTargetManager>().setColor0to3(p[i]);
        }
        setPointa(subPointa * 3);
        moveKersol();
    }

}
