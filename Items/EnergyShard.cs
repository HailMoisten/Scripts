using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnergyShard : AItem
{
    public override void Awake()
    {
        _name = "EnergyShard";
        base.Awake();
        actioncode = -2;
        flavor = "This is shard of energy element. It will be a catalyst of magic.";
        objectPass = "Prefabs/Items/BasicItemSymbol";
        duration = 0.5f;
        icon = Resources.Load<Sprite>("Images/Icons/Item/EnergyShard");
    }
    public override void SetParamsNeedAnimal(AAnimal myself)
    {
        duration = 0.5f / myself.MovementSpeed;
    }
    public override void Action(AAnimal myself)
    {
        Materialize(myself.nextPOS + myself.DIR);
        SetMotionAndDurationAndUseHPSP(myself);
    }
}
