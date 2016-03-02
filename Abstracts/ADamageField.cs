using UnityEngine;
using System.Collections;

public abstract class ADamageField : MonoBehaviour {
    public AAnimal Creator = null;
    protected string mindName;
    protected int profPoint = 0;
    protected int attackDamage = 0;
    protected int magicDamage = 0;
    protected float damageDuration = 1.0f;
    protected float castTime = 0.5f;
    protected Vector3 center = new Vector3(0, 0, 0);
    protected BoxCollider myCollider;
    protected string BuffName = null;
    protected GameObject damageEffect = null;
    protected Vector3 skillScaleVector = Vector3.one;
    private int hitcount = 0;
    protected virtual void Awake()
    {
        gameObject.tag = "DamageField";
        gameObject.layer = LayerMask.NameToLayer("DamageField");
    }

    public void SetMainParam(AAnimal creator, string mindname, int profpoint,
        int attackdamage, int magicdamage, Vector3 centerposition, 
        GameObject damageeffect, Vector3 skillscalevector, 
        string buffname, 
        float damageduration, float casttime)
    {
        Creator = creator; mindName = mindname; profPoint = profpoint;
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
        yield return new WaitForSeconds(damageDuration);
        if (profPoint == 0 || mindName == "") { }
        else
        {
            if (Creator.Mind.FindChild(mindName))
            {
                if (Creator.tag == "Player")
                { Creator.Mind.FindChild(mindName).GetComponent<AMind>().GainProficiency(profPoint + hitcount, true); }
                else { Creator.Mind.FindChild(mindName).GetComponent<AMind>().GainProficiency(profPoint + hitcount, false); }
            }
        }
        Destroy(gameObject);
    }

    protected void OnTriggerEnter(Collider colliderInfo)
    {
        if (colliderInfo.gameObject.layer == LayerMask.NameToLayer("Animal"))
        {
            AAnimal target = colliderInfo.gameObject.GetComponent<AAnimal>();
            target.TakeDamage(attackDamage, magicDamage);
            hitcount++;
            if (BuffName != string.Empty || BuffName != "")
            {
                target.TakeBuff(BuffName);
            }
        }
    }

}
