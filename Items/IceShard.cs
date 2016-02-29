using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class IceShard : AItem
{
    public override void Awake()
    {
        _name = "IceShard";
        base.Awake();
        actioncode = -2;
        flavor = "This is shard of ice element. It will be a catalyst of magic.";
        objectPass = "Prefabs/Items/BasicItemSymbol";
        duration = 0.5f;
        icon = Resources.Load<Sprite>("Images/Icons/Item/IceShard");
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
