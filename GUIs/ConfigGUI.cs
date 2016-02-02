using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ConfigGUI : MonoBehaviour {

    private GameObject ConfigKersol;
    private FadeManager ConfigFade;
    private int configSelectNum = 5;
    private int configSelectDistace = 44;
    private int configSelectPointa = 1;

    void OnGUI()
    {

    }
    void Start()
    {
        ConfigFade = GetComponent<FadeManager>();
        ConfigKersol = GameObject.Find("Kersol");
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            if (configSelectPointa == 1)
            {
                configSelectPointa = 1;
            }
            else
            {
                configSelectPointa--;
                ConfigKersol.transform.Translate(0, configSelectDistace, 0);
            }
        }

        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            if (configSelectPointa == configSelectNum)
            {
                configSelectPointa = configSelectNum;
            }
            else
            {
                configSelectPointa++;
                ConfigKersol.transform.Translate(0, -configSelectDistace, 0);
            }
        }

        if (Input.GetKeyUp(KeyCode.Return))
        {
            if (configSelectPointa == configSelectNum)
            {
                SceneManager.UnloadScene(1);
                Scene title = SceneManager.GetSceneByName("Title");
                SceneManager.SetActiveScene(title);
            }
        }

        if (Input.GetKeyUp(KeyCode.Backspace))
        {

        }

    }

}
