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
    protected GameObject buff = null;

    public void SetMainParam(GameObject damageeffect, float damageeffectduration, GameObject buff, int attackdamage, int magicdamage, float damageduration, float casttime, Vector3 centerposition, int size)
    {
        damageEffect = damageeffect; damageEffectDuration = damageeffectduration;
        this.buff = buff;
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
            AAnimal target = colliderInfo.gameObject.GetComponent<AAnimal>();
            target.TakeDamage(attackDamage, magicDamage);
            if (buff != null)
            {
                foreach (Transform b in target.Buffs.transform)
                {
                    if (b.gameObject.GetComponent<ABuff>().Name == buff.GetComponent<ABuff>().Name) { Destroy(b.gameObject); }
                }
                GameObject newbuff = Instantiate(buff);
                newbuff.transform.SetParent(target.Buffs.transform);
            }
        }
    }

}
