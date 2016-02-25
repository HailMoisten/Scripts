using UnityEngine;
using System.Collections;

public abstract class AChild : AAnimal {

    public override void Awake()
    {
        base.Awake();
        gameObject.tag = "Child";
    }

}
