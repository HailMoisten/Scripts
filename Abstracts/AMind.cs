using UnityEngine;
using UnityEngine.UI;
using IconAndErrorType;

public abstract class AMind : AIcon {
    protected string prefabPass = string.Empty;
    public override void Awake()
    {
        base.Awake();
        IconType = (int)IconTypeList.Mind;
        gameObject.tag = "Mind";
        NumofMindSkills = 10;
        Proficiency = 100;
        MindLevel = 1;
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
        pmcm.SetMind(gameObject);
        return pmcm;
    }
    /// <summary>
    /// * Please definite these at inheriting constracter.
    ///  - string Name
    ///  - Image ICON
    ///  - int Proficiency
    /// </summary>

    public void GainProficiency(int gainp, bool isplayer) {
        if (MindLevel >= 4) { gainp--; }
        else if (MindLevel >= 8) { gainp--; gainp--; }
        else if (MindLevel >= 10) { gainp = 0; }
        if ((int)Proficiency % 100 >= 66) { gainp--; }
        if (isplayer) { gainp += GameManager.Difficulty; }

        if (gainp <= 0 || MindLevel >= 10) { gainp = 0; }
        else
        {
            Proficiency = Proficiency + gainp;
            if (Proficiency >= 1000) { Proficiency = 1000; }
            if (isplayer)
            {
                GameObject.Find("PlayerCanvas(Clone)").GetComponent<PlayerCanvasManager>().ShowInformationText(
                    "+ " + gainp + " Prof. (" + Proficiency + ") " + "[" + Name + "]");
            }
            int curml = MindLevel;
            MindLevel = Mathf.FloorToInt(Proficiency / 100);
            if (curml < MindLevel)
            {
                MindLevelUp(isplayer);
            }
        }
    }
    private void MindLevelUp(bool isplayer)
    {
        Transform animalt = transform.parent.parent.parent;
        if (animalt.GetComponent<AAnimal>())
        {
            animalt.GetComponent<AAnimal>().UsePassiveActions();
        }
        if (isplayer)
        {
            GameObject.Find("PlayerCanvas(Clone)").GetComponent<PlayerCanvasManager>().ShowInformationText(
                "Level up to " + MindLevel + ". " + "[" + Name + "]");
        }
    }
    public AAction GetMindSkill(int index)
    {
        if (MindLevel >= index && transform.childCount >= index)
        {
            return transform.GetChild(index).GetComponent<AAction>();
        }
        return null;
    }

}
