using UnityEngine;
using System.Collections;

public abstract class ABeast : AAnimal {
    public override void YouDied()
    {

    }
    public override void GainExperience(int gainp)
    {
    }
    protected override void SetDirection()
    {
    }
    protected override void SettargetPOS(int n, bool focustarget)
    {
    }

    public override void Awake () {
        base.Awake();
        gameObject.tag = "Beast";
    }
	
	// Update is called once per frame
	void Update () {
	
	}

}
