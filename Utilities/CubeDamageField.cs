using UnityEngine;
using System.Collections;

public class CubeDamageField : ADamageField {

    public void SetAndAwake()
    {
        transform.position = center;
        transform.localScale = new Vector3(size, 0, size);
        StartCoroutine(AwakeAndDestroy());
    }

}
