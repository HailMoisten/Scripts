﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VisionManager : MonoBehaviour {
    private PlayerManager playerManager;
    SphereCollider myCollider = null;
    Light myLight = null;
    public List<AAnimal> targetAnimals;
    private int targetPointa = 0;
    public AAnimal GetNextTargetAnimal()
    {
        if (myCollider.enabled)
        {
            targetPointa++;
            if (targetAnimals.Count == 0) { return null; }
            else if (targetPointa >= targetAnimals.Count) { targetPointa = 0; }
            return targetAnimals[targetPointa];
        }
        return null;
    }

    // Use this for initialization
    void Awake () {
        playerManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
        myCollider = GetComponent<SphereCollider>();
        myLight = GetComponent<Light>();
        targetAnimals = new List<AAnimal>();
        targetAnimals.Capacity = 1;
        myCollider.enabled = false;
        myLight.enabled = false;
    }

    public void SetMindSlots(int mindslots)
    {
        myLight.range = mindslots * 3;
        transform.localScale = Vector3.one * mindslots * 3;
        targetAnimals.Capacity = mindslots;
    }

    public void OnOff(bool onoff)
    {
        myCollider.enabled = onoff;
        myLight.enabled = onoff;
        targetAnimals.Clear();
    }

    protected void OnTriggerEnter(Collider colliderInfo)
    {
        if (colliderInfo.gameObject.layer == LayerMask.NameToLayer("Animal"))
        {
            targetAnimals.Add(colliderInfo.gameObject.GetComponent<AAnimal>());
        }
    }
    protected void OnTriggerExit(Collider colliderInfo)
    {
        if (colliderInfo.gameObject.layer == LayerMask.NameToLayer("Animal"))
        {
            for (int i = 0; i < targetAnimals.Count; i++)
            {
                if (targetAnimals[i].name == colliderInfo.gameObject.name)
                {
                    targetAnimals.RemoveAt(i);
                }
            }
        }
    }
}
