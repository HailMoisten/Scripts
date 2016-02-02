using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EquipmentMenuCanvasManager : ACanvasManager
{

    private PlayerManager playerManager;
    
    // Use this for initialization
    protected override void Start()
    {
        myKersolRect = transform.FindChild("Kersol").GetComponent<RectTransform>();
        pointa = 1;
        pointaNUM = 21;
        targetPOSfixX = -4;
        targetPOSfixY = -4;

        playerManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();

        setupEquipmentMenu();
    }

    private void setupEquipmentMenu()
    {
        float[] subs = playerManager.GetSubStatus();

        GameObject minds = playerManager.gameObject.transform.FindChild("Minds").gameObject;
        int count = 0;
        foreach (Transform mt in minds.transform)
        {
            count++;
            AMind em = mt.gameObject.GetComponent<AMind>();
            if (count > subs[4]) { }
            else if (em != null)
            {
                setPointa(10 + count);
                GameObject gm = Instantiate(mt.gameObject);
                gm.transform.SetParent(Target.transform);
                Target.GetComponent<AIcon>().Icon = em.Icon;
//                gm.SetActive(false);
            }
        }
        for (int n = 21; n >= 11 + subs[4]; n--)
        {
            setPointa(n);
            Target.SetActive(false);
        }
        pointaNUM = 10 + (int)subs[4];
        setPointa(1);

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
                if (pointa == 3 || pointa == 4) { setPointa(1); }
                else if (pointa == 5 || pointa == 6) { setPointa(2); }
                else { setPointa(pointa - 4); }
                moveKersol();
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (pointa == 1) { setPointa(3); }
                else if (pointa == 2) { setPointa(5); }
                else { setPointa(pointa + 4); }
                moveKersol();
            }
            if (Input.GetKeyDown(KeyCode.RightArrow)) { inclementPointa(); moveKersol(); }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (pointa <= 1 || pointa % 4 == 3)
                {
                    DestroyThisCanvas();
                }
                else { declementPointa(); moveKersol(); }
            }
            if (Input.GetButtonDown("Submit"))
            {
                nextCanvas = clickTarget();
                if (nextCanvas != null) { nextCanvas.GetComponent<ACanvasManager>().SetBackCanvas(this); }
            }
        }
    }

}
