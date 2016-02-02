using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TitleGUI : MonoBehaviour {

    private GameObject KersolTitle;
    private GameObject KersolConfig;
    private Canvas CanvasConfig;
    private FadeManager fade;
    private int selectDistance = 44;
    private int selectNum = 2;
    private int selectPointa = 1;
    private int selectNumConfig = 5;
    private int selectPointaConfig = 1;
    const int START = 1;
    const int CONFIG = 2;

    private bool IsTitle = true;

    // Use this for initialization
    void Start () {
        fade = GetComponent<FadeManager>();
        fade.FadeIn(5.0f);
        KersolTitle = GameObject.Find("KersolTitle");
        KersolConfig = GameObject.Find("KersolConfig");
        CanvasConfig = GameObject.Find("CanvasConfig").GetComponent<Canvas>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            if (IsTitle)
            {
                if (selectPointa == 1)
                {
                    selectPointa = 1;
                }
                else
                {
                    selectPointa--;
                    KersolTitle.transform.Translate(0, selectDistance, 0);
                }
            }
            else
            {
                if (selectPointaConfig == 1)
                {
                    selectPointaConfig = 1;
                }
                else
                {
                    selectPointaConfig--;
                    KersolConfig.transform.Translate(0, selectDistance, 0);
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            if (IsTitle)
            {
                if (selectPointa == selectNum)
                {
                    selectPointa = selectNum;
                }
                else
                {
                    selectPointa++;
                    KersolTitle.transform.Translate(0, -selectDistance, 0);
                }
            }
            else
            {
                if (selectPointaConfig == selectNumConfig)
                {
                    selectPointaConfig = selectNumConfig;
                }
                else
                {
                    selectPointaConfig++;
                    KersolConfig.transform.Translate(0, -selectDistance, 0);
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.Return))
        {
            if (IsTitle)
            {
                if (selectPointa == START)
                {
                    fade.LoadLevel("EzeVillage", 1.0f, 2.0f);
                }
                if (selectPointa == CONFIG)
                {
                    IsTitle = false;
                    CanvasConfig.sortingOrder = 1;
                }
            }
            else
            {
                if (selectPointaConfig == selectNumConfig)
                {
                    IsTitle = true;
                    CanvasConfig.sortingOrder = -1;
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.Backspace))
        {
            if (IsTitle){ }
            else
            {
                IsTitle = true;
                CanvasConfig.sortingOrder = -1;
            }
        }
    }

}
