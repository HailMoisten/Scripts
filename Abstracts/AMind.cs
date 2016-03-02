using UnityEngine;
using UnityEngine.UI;
using IconAndErrorType;

public abstract class AMind : AIcon {
    protected string prefabPass = string.Empty;
    private GameManager gameManager = null;
    public override void Awake()
    {
        base.Awake();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
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

    public void GrowProficiency(float addp, bool isplayer) {
        if (MindLevel >= 4) { addp--; }
        else if (MindLevel >= 8) { addp--; addp--; }
        else if (MindLevel >= 10) { addp = 0; }
        if ((int)Proficiency % 100 >= 66) { addp--; }
        addp += gameManager.Difficulty;

        if (addp <= 0 || MindLevel >= 10) { addp = 0; }
        else
        {
            Proficiency = Proficiency + addp;
            if (Proficiency >= 1000) { Proficiency = 1000; }
            if (isplayer)
            {
                GameObject.Find("PlayerCanvas(Clone)").GetComponent<PlayerCanvasManager>().ShowInformationText(
                    "+ " + addp + " Prof. (" + Proficiency + ") " + "[" + Name + "]");
            }
            int curml = MindLevel;
            MindLevel = Mathf.FloorToInt(Proficiency / 100);
            if (curml < MindLevel)
            {
                if (isplayer)
                {
                    GameObject.Find("PlayerCanvas(Clone)").GetComponent<PlayerCanvasManager>().ShowInformationText(
                        "Level up to " + MindLevel + ". " + "[" + Name + "]");
                }
            }
        }
    }
    public AAction GetMindSkill(int index)
    {
        if (MindLevel >= index)
        {
            if (transform.GetChild(index).GetComponent<AAction>().IsPassive)
            { return null; }
            return transform.GetChild(index).GetComponent<AAction>();
        }
        else { Debug.Log("Need more MindLevel."); }
        return null;
    }

}
