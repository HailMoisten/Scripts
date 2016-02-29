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
    protected string BuffName = null;
    protected virtual void Awake()
    {
        gameObject.tag = "DamageField";
        gameObject.layer = LayerMask.NameToLayer("DamageField");
    }

    public void SetMainParam(AAnimal creator, int attackdamage, int magicdamage, Vector3 centerposition, GameObject damageeffect, Vector3 skillscalevector, string buffname, float damageduration, float casttime)
    {
        Creator = creator;
        attackDamage = attackdamage; magicDamage = magicdamage; center = centerposition;
        damageEffect = damageeffect; skillScaleVector = skillscalevector;
        BuffName = buffname;
        damageDuration = damageduration; castTime = casttime;
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
        if (colliderInfo.gameObject.layer == LayerMask.NameToLayer("Animal"))
        {
            AAnimal target = colliderInfo.gameObject.GetComponent<AAnimal>();
            target.TakeDamage(attackDamage, magicDamage);
            if (BuffName != string.Empty || BuffName != "")
            {
                target.TakeBuff(BuffName);
            }
        }
    }

}
