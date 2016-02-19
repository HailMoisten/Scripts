using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ErrorTextCanvasManager : ACanvasManager {
    public void SetAndDestroy(int errorNum)
    {
        Transform erTextTrans = transform.GetChild(0);
        string content = "";
        switch (errorNum)
        {
            case 1: content = "Cant move to there."; break;
            case 2: content = "You dont have enough number of it."; break;
            case 3: content = "I can not do it."; break;
            default: content = "Unexpected error."; break;
        }
        erTextTrans.GetComponent<Text>().text = content;
        erTextTrans.GetComponent<TextShade>().TextUpdate();
        Destroy(gameObject, 2.0f);
    }

    // Use this for initialization
    protected override void Awake()
    {
    }

}
