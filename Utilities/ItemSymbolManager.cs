using UnityEngine;
using System.Collections;

public class ItemSymbolManager : MonoBehaviour {
    bool usedGravity = false;
    void Awake()
    {
        gameObject.layer = LayerMask.NameToLayer("Item");
    }
    int step = 30;
	// Update is called once per frame
	void Update () {
        if (usedGravity) { }
        else
        {
            Vector3 surface = transform.position + new Vector3(0, Terrain.activeTerrain.SampleHeight(transform.position) - transform.position.y, 0);
            iTween.MoveTo(gameObject,
                iTween.Hash("position", surface,
                "time", 2.0f,
                "easetype", "linear"
                ));
            usedGravity = true;
        }

        transform.rotation = Quaternion.Euler(
        0,
        Time.time * this.step,
        0
        );
    }
}
