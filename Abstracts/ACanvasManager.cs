using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public abstract class ACanvasManager : MonoBehaviour {

    private GameObject target = null;
    protected GameObject Target { get { return target; } }
    private GameObject lastTarget = null;
    protected RectTransform myKersolRect = null;
    protected ACanvasManager backCanvas = null;
    protected ACanvasManager nextCanvas = null;
    public bool YesNoAnswerOfNextPopUpTextCanvas = false;

    // About Kersol
    protected int pointa = 1;
    public int GetPointa() { return pointa; }
    protected int pointaNUM = 1;
    protected int targetPOSfixX = 0;
    protected int targetPOSfixY = 0;

    // At least, please set myKersolRect.
    // Can we access to protected constracters from Prefabs?
    protected abstract void Start();
    /*
    {
        myKersolRect = transform.FindObject("Kersol").gameObject.GetComponent<RectTransform>();
        backCanvas = null;
        nextCanvas = null;
        pointa = FirstTarget;
        pointaNUM = NumOfSelectableTexts;
        targetPOSfixX = -5;
        targetPOSfixY = -5;
    }
    */

    protected void inclementPointa() { if (pointa >= pointaNUM) { } else { pointa++; } setTarget();}
    protected void declementPointa() { if (pointa <= 1) { } else { pointa--; } setTarget(); }
    protected void setPointa(int n) { if (n <= 0 || n >= pointaNUM+1) { } else { pointa = n; setTarget(); } }
    protected void setTarget()
    {
        lastTarget = target;
        target = transform.FindChild("SelectableTarget (" + pointa + ")").gameObject;
    }
    protected void moveKersol()
    {
        if (lastTarget != null)
        {
            lastTarget.GetComponent<SelectableTargetManager>().OffKersol();
        }
        if (target != null)
        {
            Vector3 p = target.GetComponent<RectTransform>().position + new Vector3(targetPOSfixX, targetPOSfixY, 0);
            myKersolRect.position = p;
            target.GetComponent<SelectableTargetManager>().OnKersol();
        }
    }
    protected ACanvasManager clickTarget()
    {
        AIcon ai = target.transform.GetChild(0).GetComponent<AIcon>();
        if (ai != null) { return ai.Clicked(); }
        return null;
    }
    public void SetBackCanvas(ACanvasManager backC) { backCanvas = backC; }
    public void DestroyThisCanvas() {
        if (nextCanvas != null) { Destroy(nextCanvas.gameObject); }
        Destroy(gameObject);
    }
    public void DestroyNextCanvas() { if (nextCanvas != null) { Destroy(nextCanvas.gameObject); } }
}
