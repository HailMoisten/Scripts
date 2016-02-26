using UnityEngine;
using System.Collections;

public class ItemSymbolManager : MonoBehaviour {
    void Awake()
    {
        gameObject.layer = LayerMask.NameToLayer("Item");
    }
    int step = 30;
	// Update is called once per frame
	void Update () {
        transform.rotation = Quaternion.Euler(
        0,
        Time.time * this.step,
        0
    );
    }
}
