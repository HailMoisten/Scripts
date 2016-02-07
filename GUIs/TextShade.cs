using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextShade : MonoBehaviour {
    // Use this for initialization
    void Start () {
        if (transform.parent.GetComponent<TextShade>()) { }
        else
        {
            GameObject main = Instantiate(gameObject);
            main.transform.SetParent(transform);
            main.transform.localPosition = transform.position + Vector3.up;
            main.transform.localScale = Vector3.one;
            main.GetComponent<Text>().color = new Color(1.0f, 1.0f, 1.0f);
        }
    }

    public void TextUpdate ()
    {
        transform.GetChild(0).GetComponent<Text>().text = transform.GetComponent<Text>().text;
    }

}
