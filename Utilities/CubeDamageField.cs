using UnityEngine;
using System.Collections;

public class CubeDamageField : ADamageField {

    public void SetAndAwake()
    {
        myCollider = GetComponent<BoxCollider>();
        myCollider.enabled = false;

        transform.position = center;
        transform.localScale = new Vector3(size - 0.1f, 1 - 0.1f, size - 0.1f);
        StartCoroutine(AwakeAndDestroy());
    }

}
