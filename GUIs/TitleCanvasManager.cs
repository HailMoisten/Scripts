using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TitleCanvasManager : ACanvasManager
{
    private FadeManager fade;
    private GameObject configCanvas;

    protected override void Start()
    {
        fade = GetComponent<FadeManager>();
        fade.FadeIn(5.0f);
        configCanvas = (GameObject)Resources.Load("Prefabs/GUI/ConfigCanvas");

        myKersolRect = transform.FindChild("Kersol").GetComponent<RectTransform>();
        pointa = 1;
        pointaNUM = 2;
        kersolPOSfix = new Vector3(-24, 11, 0);
        firstpointa = 0;

        initPointaAndKersol();
    }

    // dont make Update
    void Update()
    {
        Vector3 menuDIR = new Vector3(0, 0, 0);
        menuDIR.x = Input.GetAxisRaw("Horizontal");
        menuDIR.z = Input.GetAxisRaw("Vertical");
        if (nextCanvas != null)
        {
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.DownArrow)) { inclementPointa(); moveKersol(); }
            if (Input.GetKeyDown(KeyCode.UpArrow)) { declementPointa(); moveKersol(); }
            if (Input.GetButtonDown("Submit"))
            {
                if (pointa == 1)
                {
                    fade.LoadLevel("EzeVillage", 3.0f, 1.5f);
                    //                    fade.FadeOut(3.0f);
                    //                    newGame(3.0f);
                }
                else if (pointa == 2){
                    nextCanvas = Instantiate(configCanvas).GetComponent<ACanvasManager>();
                    nextCanvas.GetComponent<ACanvasManager>().SetBackCanvas(this);
                }
            }
            if (Input.GetKeyUp(KeyCode.Backspace))
            {
            }
        }
    }

    private IEnumerator newGame(float waittime)
    {
        yield return new WaitForSeconds(waittime);
        SceneManager.LoadScene("EzeVillage");
    }

}
