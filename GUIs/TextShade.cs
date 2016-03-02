using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextShade : MonoBehaviour {
    public bool duplicated;
    // Use this for initialization
    void Set() {
        if (transform.parent.GetComponent<TextShade>()) { duplicated = true; }
        else
        {
            duplicated = true;
            GameObject main = Instantiate(gameObject);
            main.transform.SetParent(transform);
            main.transform.localPosition = Vector3.up;
            main.transform.localScale = Vector3.one;
            main.GetComponent<Text>().color = new Color(1.0f, 1.0f, 1.0f);
        }
    }

    public void TextUpdate ()
    {
        if (duplicated) { }
        else { Set(); }
        transform.GetChild(0).GetComponent<Text>().text = transform.GetComponent<Text>().text;
    }

}
