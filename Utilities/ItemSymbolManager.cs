using UnityEngine;
using System.Collections;

public class ItemSymbolManager : MonoBehaviour {
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
