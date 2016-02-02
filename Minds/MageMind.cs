using UnityEngine;
using UnityEngine.UI;

public class MageMind : AMind {

    protected override void Start()
    {
        NAME = "Mage"; // const
        TYPE = "Mind"; // const
        FLAVOR = "Mage is a Nuker.";
        ICON = GetComponent<Image>().sprite; // const
        Proficiency = 0;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
