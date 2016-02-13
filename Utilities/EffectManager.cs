using UnityEngine;
using System.Collections;

public class EffectManager : MonoBehaviour {

    public Vector3 Scale { get; set; }
    public float Duration { get; set; }
    private float emitrate = 1.0f;

    public void Start()
    {
        Scale = Vector3.one;
        Duration = 5.0f;
    }
    // Use this for initialization
    public void Go()
    {
        transform.localScale = Scale;
        Duration = GetComponent<ParticleSystem>().duration;
        float scaleNum = Mathf.Sqrt(Scale.x * Scale.y * Scale.z);
        if (scaleNum > 1.0f) { scaleNum = 1; }
        Transform par = null;
        for (int n = 0; n < transform.childCount; n++)
        {
            par = transform.GetChild(n);
            if (par.tag == "RemainParticle")
            {
                // I had no choice...
                emitrate = par.GetComponent<ParticleSystem>().emissionRate;
                emitrate = emitrate * scaleNum;
                par.GetComponent<ParticleSystem>().emissionRate = emitrate;
            }
            else if (par.tag == "BurstParticle")
            {
                emitrate = par.GetComponent<ParticleSystem>().maxParticles;
                emitrate = emitrate * scaleNum;
                par.GetComponent<ParticleSystem>().maxParticles = Mathf.RoundToInt(emitrate);
            }
        }
        Destroy(gameObject, Duration);
    }
}
