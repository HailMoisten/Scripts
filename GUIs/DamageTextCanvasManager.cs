using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DamageTextCanvasManager : ACanvasManager {

    Transform adText;
    Transform mdText;

    public void SetAndDestroy(int a, int m, Vector3 pos)
    {
        adText = transform.GetChild(0);
        adText.GetComponent<RectTransform>().localPosition = GameObject.Find("Camera").GetComponent<Camera>().WorldToScreenPoint(pos) + new Vector3(0, 128, 0);
        if (a == 0) { adText.GetComponent<Text>().text = ""; }
        else
        {
            adText.GetComponent<Text>().text = a.ToString();
            adText.GetComponent<TextShade>().TextUpdate();
        }
        mdText = transform.GetChild(1);
        mdText.GetComponent<RectTransform>().localPosition = adText.GetComponent<RectTransform>().localPosition + new Vector3(0, -22, 0);
        if (m == 0) { mdText.GetComponent<Text>().text = ""; }
        else
        {
            mdText.GetComponent<Text>().text = m.ToString();
            mdText.GetComponent<TextShade>().TextUpdate();
        }

        Destroy(gameObject, 2.0f);
    }

    // Use this for initialization
    protected override void Awake () {
    }

}
