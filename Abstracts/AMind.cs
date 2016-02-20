using UnityEngine;
using UnityEngine.UI;
using IconType;

public abstract class AMind : AIcon {
    protected string prefabPass = string.Empty;
    public override void Awake()
    {
        base.Awake();
        IconType = (int)IconTypeList.Mind;
        gameObject.tag = "Mind";
        NumofMindSkills = 10;
    }

    public float Proficiency { get; set; }
    public int MindLevel { get; set; }

    public int NumofMindSkills { get; set; }

    public override ACanvasManager Clicked(Vector3 clickedpos)
    {
        GameObject inst = Instantiate((GameObject)Resources.Load("Prefabs/GUI/PopUpMindCanvas"));
        PopUpMindCanvasManager pmcm = inst.GetComponent<PopUpMindCanvasManager>();
        pmcm.Title = Name;
        pmcm.Icon = Icon;
        pmcm.Content = "Proficiency " + Proficiency + "\n" + 
            "MindLevel " + MindLevel;
        pmcm.Flavor = Flavor;
        pmcm.SetMind(this);
        return pmcm;
    }
    /// <summary>
    /// * Please definite these at inheriting constracter.
    ///  - string Name
    ///  - Image ICON
    ///  - int Proficiency
    /// </summary>

    public void GrowProficiency(float addp) {
        Proficiency = Proficiency + addp;
        if (Proficiency >= 900) { Proficiency = 900;}
        MindLevel = 1 + Mathf.RoundToInt(Proficiency / 100);
    }
    public AAction GetMindSkill(int index)
    {
        if (MindLevel >= index) { return transform.GetChild(index).GetComponent<AAction>(); }
        else { Debug.Log("Need more MindLevel."); }
        return null;
    }

}
