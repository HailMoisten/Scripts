﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ErrorTextCanvasManager : ACanvasManager {
    public void SetAndDestroy(int errorNum)
    {
        Transform erTextTrans = transform.GetChild(0);
        string content = "";
        switch (errorNum)
        {
            case 1: content = "I can not move to there."; break;
            case 2: content = "I dont have enough number of it."; break;
            case 3: content = "I can not do it."; break;
            case 4: content = "Need more MindLevel."; break;
            case 5: content = "Need more Level."; break;
            default: content = "Unexpected error."; break;
        }
        erTextTrans.GetComponent<Text>().text = content;
        erTextTrans.GetComponent<TextShade>().TextUpdate();
        Destroy(gameObject, 2.0f);
    }

    float lastRealTime = 0.0f;
    void Update()
    {
        if (lastRealTime == 0)
        {
            lastRealTime = Time.realtimeSinceStartup;
        }
        if (Time.realtimeSinceStartup - lastRealTime > 2.0f)
        {
            Destroy(gameObject);
        }
    }

    // Use this for initialization
    protected override void Awake()
    {
    }

}
