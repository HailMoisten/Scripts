using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AirShard : AItem {
    public override void Awake()
    {
        base.Awake();
        actioncode = -2;
        _name = "AirShard";
        flavor = "This is shard of element. It will be a catalyst of magic.";
        objectPass = "Prefabs/Items/BasicItemSymbol";
        duration = 0.5f;
        icon = Resources.Load<Sprite>("Images/Icons/Item/AirShard");
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
