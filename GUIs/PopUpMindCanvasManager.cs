using UnityEngine;
using System.Collections;

public class PopUpMindCanvasManager : PopUpIconCanvasManager {
    protected GameObject targetMind = null;
    // Use this for initialization
    protected override void Awake()
    {
        myKersolRect = transform.GetChild(0).FindChild("Kersol").GetComponent<RectTransform>();

        pointa = 1;
        pointaNUM = 10;
        kersolPOSfix = new Vector3(-4, -4, 0);
        firstpointa = 1;
        initPointaAndKersol();
        moveKersol();
    }
    public void SetMind(AMind mind)
    {
        targetMind = Instantiate((GameObject)Resources.Load("Prefabs/Minds/" + mind.Name));
        targetMind.transform.SetParent(transform);
        targetMind.GetComponent<AMind>().GrowProficiency(mind.Proficiency);
        for (int i = 1; i <= targetMind.GetComponent<AMind>().NumofMindSkills; i++)
        {
            setPointa(i);
            AIcon targeticon = targetMind.transform.GetChild(i).GetComponent<AIcon>();
            Target.GetComponent<SelectableTargetManager>().TargetIcon = targeticon;
            Target.GetComponent<AIcon>().Icon = targeticon.Icon;
        }
        initPointaAndKersol();
        moveKersol();
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
            if (Input.GetButtonDown("Cancel")) { DestroyThisCanvas(); }
            if (Input.GetButtonDown("Submit"))
            {
                if (Target.GetComponent<SelectableTargetManager>().TargetIcon != null)
                {
                    nextCanvas = clickTarget();
                    if (nextCanvas != null) { nextCanvas.GetComponent<ACanvasManager>().SetBackCanvas(this); }
                }
            }
        }
    }

}
