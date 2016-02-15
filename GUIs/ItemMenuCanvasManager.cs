using UnityEngine;
using System.Collections;

public class ItemMenuCanvasManager : ACanvasManager
{

    private PlayerManager playerManager;

    // Use this for initialization
    protected override void Awake()
    {
        myKersolRect = transform.FindChild("Kersol").GetComponent<RectTransform>();
        pointa = 1;
        pointaNUM = 1;

        playerManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            DestroyThisCanvas();
        }
    }
}
