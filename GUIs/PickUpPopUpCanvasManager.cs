using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PickUpPopUpCanvasManager : ACanvasManager
{
    // Use this for initialization
    protected override void Awake()
    {
        myKersolRect = transform.FindChild("Kersol").GetComponent<RectTransform>();
        pointa = 0;
        pointaNUM = 0;
        kersolPOSfix = new Vector3(-4, -4, 0);
        firstpointa = 1;

        initPointaAndKersol();
    }
    // Update is called once per frame
    private void Update()
    {
        if (nextCanvas != null)
        {
        }
        else
        {
            if (Input.GetButtonDown("Cancel"))
            {
                DestroyThisCanvas();
            }
            if (Input.GetButtonDown("Submit"))
            {
                DestroyThisCanvas();
            }
        }
    }
}
