using UnityEngine;
using System.Collections;

public class CubeDamageField : MonoBehaviour {
    private int attackDamage = 10;
    private int magicDamage = 0;
    private Vector3 center = new Vector3(0, 0, 0);
    private int manhattanSize = 1;
    private float duration = 1.0f;

    public void SetParamAndAwake(int ad, int md, Vector3 centerpos, int msize, float dur)
    {
        attackDamage = ad; magicDamage = md;
        center = centerpos; manhattanSize = msize;
        duration = dur;
        transform.position = center;
        transform.localScale = new Vector3(msize, 0, msize);
        GetComponent<BoxCollider>().gameObject.SetActive(true);
        Destroy(gameObject, duration);
    }
    
    void OnTriggerEnter(Collider colliderInfo)
    {
        if (colliderInfo.gameObject.tag == "Animal" || colliderInfo.gameObject.tag == "Player")
        {
            Debug.Log("hit");
            colliderInfo.gameObject.GetComponent<AAnimal>().TakeDamage(attackDamage, magicDamage);
        }
    }
}
