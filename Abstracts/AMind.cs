using UnityEngine;
using UnityEngine.UI;

public abstract class AMind : AIcon {

    protected int proficiency = 0;
    public int Proficiency
    {
        get { return proficiency; }
        set { proficiency = value; }
    }
    protected int mindlevel = 1;
    public int MindLevel
    {
        get { return mindlevel; }
    }

    protected int NUMofMindSkills = 1;

    public override ACanvasManager Clicked(Vector3 clickedpos)
    {
        GameObject inst = (GameObject)Instantiate(Resources.Load("Prefabs/GUI/PopUpTextCanvas"), Vector3.zero, Quaternion.identity);
        inst.transform.GetChild(0).GetComponent<RectTransform>().localPosition = clickedpos + new Vector3(64, 64, 0);
        PopUpTextCanvasManager ptcm = inst.GetComponent<PopUpTextCanvasManager>();
        ptcm.Title = _name;
        ptcm.Content = flavor;

        return ptcm;
    }
    /// <summary>
    /// * Please definite these at inheriting constracter.
    ///  - string Name
    ///  - Image ICON
    ///  - int Proficiency
    /// </summary>

    public void GrowProficiency(int addp) { proficiency = proficiency + addp; }
    public AAction GetMindSkill(int index)
    {
        if (mindlevel >= index) { return transform.GetChild(index).GetComponent<AAction>(); }
        else { Debug.Log("Need more MindLevel."); }
        return null;
    }

}
