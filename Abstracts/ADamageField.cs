using UnityEngine;
using System.Collections;

public abstract class ADamageField : MonoBehaviour {
    public AAnimal Creator = null;
    protected int attackDamage = 0;
    protected int magicDamage = 0;
    protected float damageDuration = 1.0f;
    protected float castTime = 0.5f;
    protected Vector3 center = new Vector3(0, 0, 0);
    protected BoxCollider myCollider;
    protected GameObject damageEffect = null;
    protected Vector3 skillScaleVector = Vector3.one;
    protected GameObject buff = null;
    protected void Awake()
    {
        gameObject.tag = "DamageField";
    }

    public void SetMainParam(AAnimal creator, GameObject damageeffect, Vector3 skillscalevector, GameObject buff, int attackdamage, int magicdamage, float damageduration, float casttime, Vector3 centerposition)
    {
        this.Creator = creator;
        damageEffect = damageeffect; skillScaleVector = skillscalevector;
        this.buff = buff;
        attackDamage = attackdamage; magicDamage = magicdamage;
        this.damageDuration = damageduration; this.castTime = casttime;
        center = centerposition;
    }


    public IEnumerator AwakeAndDestroy()
    {
        yield return new WaitForSeconds(castTime);
        myCollider.enabled = true;
        if (damageEffect != null)
        {
            GameObject dm = (GameObject)Instantiate(damageEffect, center, Quaternion.identity);
            dm.GetComponent<EffectManager>().Scale = skillScaleVector;
            dm.GetComponent<EffectManager>().Go();
        }
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
                GameObject newbuff = (GameObject)Instantiate(buff, Vector3.zero, Quaternion.identity);
                newbuff.transform.SetParent(target.Buffs.transform);
            }
        }
    }

}
