using UnityEngine;
using UnityEngine.UI;

public abstract class AMind : AIcon {

    protected int PROFICIENCY = 0;
    public int Proficiency
    {
        get { return PROFICIENCY; }
        set { PROFICIENCY = value; }
    }
    protected int MINDLEVEL = 0;
    public int MindLevel
    {
        get { return MINDLEVEL; }
    }
    protected string FLAVOR = "";
    public string Flavor {
        get { return FLAVOR; }
    }
    protected int NUMofMindSkills = 1;
    protected AMindSkill[] skills;


    public override ACanvasManager Clicked(Vector3 clickedpos)
    {
        GameObject inst = Instantiate((GameObject)Resources.Load("Prefabs/GUI/PopUpTextCanvas"));
        inst.transform.GetChild(0).GetComponent<RectTransform>().localPosition = clickedpos + new Vector3(64, 64, 0);
        PopUpTextCanvasManager ptcm = inst.GetComponent<PopUpTextCanvasManager>();
        ptcm.Title = Name;
        ptcm.Content = Flavor;

        return ptcm;
    }

    /** 
    *Please definite these at inheriting constracter.
    *- string Name
    *- Image ICON
    *- int Proficiency
    */
    protected abstract void Start();

    public void GrowProficiency(int addp) { PROFICIENCY = PROFICIENCY + addp; }
    public abstract AMindSkill GetMindSkill(int index);

}
