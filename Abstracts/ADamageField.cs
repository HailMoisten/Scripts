using UnityEngine;
using System.Collections;

public abstract class ADamageField : MonoBehaviour {

    protected int attackDamage = 0;
    protected int magicDamage = 0;
    protected float damageDuration = 1.0f;
    protected float castTime = 0.5f;
    protected Vector3 center = new Vector3(0, 0, 0);
    protected int size = 1;
    protected BoxCollider myCollider;
    protected GameObject damageEffect = null;
    protected float damageEffectDuration;

    public void SetMainParam(GameObject damageeffect, float damageeffectduration, int attackdamage, int magicdamage, float damageduration, float casttime, Vector3 centerposition, int size)
    {
        damageEffect = damageeffect; damageEffectDuration = damageeffectduration;
        attackDamage = attackdamage; magicDamage = magicdamage;
        this.damageDuration = damageduration; this.castTime = casttime;
        center = centerposition; this.size = size;
    }


    public IEnumerator AwakeAndDestroy()
    {
        yield return new WaitForSeconds(castTime);
        myCollider.enabled = true;
        GameObject dm = (GameObject)Instantiate(damageEffect, center, Quaternion.identity);
        Destroy(dm, damageEffectDuration);
        Destroy(gameObject, damageDuration);
    }

    protected void OnTriggerEnter(Collider colliderInfo)
    {
        if (colliderInfo.gameObject.tag == "Animal" || colliderInfo.gameObject.tag == "Player")
        {
            colliderInfo.gameObject.GetComponent<AAnimal>().TakeDamage(attackDamage, magicDamage);
        }
    }

}
