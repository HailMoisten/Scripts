using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextFadeRise : MonoBehaviour {

    private Text mytext = null;
    private RectTransform myrect = null;
    private bool duplicated = false;
    // Use this for initialization
    public void Awake()
    {
        if (GetComponent<TextShade>()) { duplicated = GetComponent<TextShade>().duplicated; }
        mytext = GetComponent<Text>();
        myrect = GetComponent<RectTransform>();
        StartCoroutine(beforeFade(1.0f));
    }

    private IEnumerator beforeFade(float time)
    {
        yield return new WaitForSeconds(time);
        mytext.CrossFadeAlpha(0.0f, 1.0f, false);
    }
    // Update is called once per frame
    void Update ()
    {
        if (duplicated) { }
        else { myrect.localPosition += Vector3.up * Time.deltaTime * 3.0f; }
    }
}
