using UnityEngine;
using System.Collections;

public class CubeDamageField : ADamageField {

    public void SetAndAwake()
    {
        myCollider = GetComponent<BoxCollider>();
        myCollider.enabled = false;

        transform.position = center;
        transform.localScale = skillScaleVector + (-0.1f * Vector3.one);
        StartCoroutine(AwakeAndDestroy());
    }

}
