using UnityEngine;
using System.Collections;

public abstract class ADamageField : MonoBehaviour {

    protected int attackDamage = 0;
    protected int magicDamage = 0;
    protected float duration = 1.0f;
    protected float casttime = 0.0f;
    protected Vector3 center = new Vector3(0, 0, 0);
    protected int size = 1;

    public void SetMainParam(int attackdamage, int magicdamage, float duration, float casttime, Vector3 centerposition, int size)
    {
        attackDamage = attackdamage; magicDamage = magicdamage;
        this.duration = duration; this.casttime = casttime;
        center = centerposition; this.size = size;
    }


    public IEnumerator AwakeAndDestroy()
    {
        yield return new WaitForSeconds(casttime);
        GetComponent<BoxCollider>().gameObject.SetActive(true);
        Destroy(gameObject, duration);
    }

    protected void OnTriggerEnter(Collider colliderInfo)
    {
        if (colliderInfo.gameObject.tag == "Animal" || colliderInfo.gameObject.tag == "Player")
        {
            colliderInfo.gameObject.GetComponent<AAnimal>().TakeDamage(attackDamage, magicDamage);
        }
    }

}
