using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using IconAndErrorType;

public class ErrorTextCanvasManager : ACanvasManager {
    public void SetAndDestroy(int errorNum)
    {
        Transform erTextTrans = transform.GetChild(0);
        string content = "";
        switch (errorNum)
        {
            case (int)ErrorTypeList.Nothing: content = "Nothing."; break;
            case (int)ErrorTypeList.Move: content = "I can not move to there."; break;
            case (int)ErrorTypeList.Number: content = "I don't have enough number of it."; break;
            case (int)ErrorTypeList.HPSP: content = "Need more HP or SP."; break;
            case (int)ErrorTypeList.MindLevel: content = "Need more MindLevel."; break;
            case (int)ErrorTypeList.Level: content = "Need more Level."; break;
            case (int)ErrorTypeList.BattleReady: content = "I am not ready to battle."; break;
            case (int)ErrorTypeList.Catalyst: content = "I don't have any catalyst."; break;
            case (int)ErrorTypeList.IsPassive: content = "It is passive action."; break;
            case (int)ErrorTypeList.TooFar: content = "Too far."; break;
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
