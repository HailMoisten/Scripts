using UnityEngine;
using System.Collections;

public class ReturnPopUpMindCanvasManager : PopUpMindCanvasManager {

    // Use this for initialization
    protected override void Awake()
    {
        base.Awake();
    }
    // Update is called once per frame
    void Update()
    {
        if (nextCanvas != null)
        {
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (pointa <= 5) { }
                else { setPointa(pointa - 5); }
                moveKersol();
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (pointa >= 6) { }
                else { setPointa(pointa + 5); }
                moveKersol();
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                inclementPointa();
                moveKersol();
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (pointa == 1)
                {
                    DestroyThisCanvas();
                }
                else {
                    declementPointa();
                    moveKersol();
                }
            }

            if (Input.GetButtonDown("Submit"))
            {
                if(targetMind.GetComponent<AMind>().MindLevel >= pointa)
                {
                    DestroyThisCanvas();
                }
                else { Debug.Log("Need more Mind Level"); }
            }
            if (Input.GetButtonDown("Cancel"))
            {
                DestroyThisCanvas();
            }
        }
    }

}
