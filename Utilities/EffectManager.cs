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
    public void Awake()
    {
        transform.localScale = Scale;
        Transform par = null;
        for (int n = 0; n < transform.childCount; n++)
        {
            par = transform.GetChild(n);
            if (par.tag == "RemainParticle")
            {
                emitrate = par.GetComponent<ParticleSystem>().emissionRate;
                emitrate = emitrate * Mathf.Sqrt(Scale.x * Scale.y * Scale.z);
                par.GetComponent<ParticleSystem>().emissionRate = emitrate;
            }
            else if (par.tag == "BusrtParticle")
            {
                emitrate = par.GetComponent<ParticleSystem>().maxParticles;
                emitrate = emitrate * Mathf.Sqrt(Scale.x * Scale.y * Scale.z);
                par.GetComponent<ParticleSystem>().maxParticles = Mathf.RoundToInt(emitrate);
            }
        }
        Destroy(gameObject, Duration);
    }
}
