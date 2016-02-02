using UnityEngine;
using System.Collections;

public abstract class AAnimal : MonoBehaviour {
    // Necessary Public Methods
    public Animator GetAnimator() { return GetComponent<Animator>(); }
    public Vector3 RoundToIntVector3XZ(Vector3 pos) { return new Vector3(Mathf.RoundToInt(pos.x), pos.y, Mathf.RoundToInt(pos.z)); }

    // Status
    private const int statusGenus = 18;
    protected int Lv = 1, VIT = 1, STR = 1, AGI = 1, INT = 1, MND = 1;
    protected int AD = 1, MD = 1, AR = 1, MR = 1;
    protected int MindSlots = 1;
    protected float MovementSpeed = 1.0f;
    protected float RunRatio = 2.0f;
    protected float HPGainRatio = 1.0f, HPRegenRatio = 1.0f;
    protected float SPGainRatio = 1.0f, SPRegenRatio = 1.0f;
    protected int MAXHP = 1, MAXSP = 1;
    protected int HPRegen = 1, SPRegen = 1;
    protected int HP = 1, SP = 1;
    protected int Poise;
    protected int[] LevelUpReward = {0, 0, 0, 0, 0};
    public int GetLevel() { return Lv; }
    public int[] GetMainStatus()
    {
        int[] mains = new int[5];
        mains[0] = VIT; mains[1] = STR; mains[2] = AGI; mains[3] = INT; mains[4] = MND;
        return mains;
    }
    public float[] GetSubStatus()
    {
        float[] subs = new float[14];
        subs[0] = AD; subs[1] = MD; subs[2] = AR; subs[3] = MR;
        subs[4] = MindSlots; subs[5] = MovementSpeed; subs[6] = RunRatio;
        subs[7] = HP; subs[8] = HPRegen; subs[9] = MAXHP;
        subs[10] = SP; subs[11] = SPRegen; subs[12] = MAXSP;
        subs[13] = Poise;
        return subs;
    }
    public int[] GetHPSP()
    {
        int[] hpsp = new int[2];
        hpsp[0] = HP; hpsp[1] = SP;
        return hpsp;
    }
    public void SetLevelUpReward(int n, int incORdec)
    {
        if (n >= 0 && n <= 4)
        {
            if ((LevelUpReward[n] + incORdec) >= 0 && (LevelUpReward[n] + incORdec) <= 3)
            {
                int sum = incORdec;
                for (int i = 0; i < LevelUpReward.Length; i++) { sum = sum + LevelUpReward[i]; }
                if (sum >= 0 && sum <= 3) { LevelUpReward[n] = LevelUpReward[n] + incORdec; }
            }
            else
            {
                Debug.Log("Too more/less Reward.");
            }
        }
    }
    public int[] GetLevelUpReward() { return LevelUpReward; }
    protected void levelUp()
    {
        if (Lv >= 165) { }
        else
        {
            Lv++;
            VIT = VIT + LevelUpReward[0]; STR = STR + LevelUpReward[1]; AGI = AGI + LevelUpReward[2]; INT = INT + LevelUpReward[3]; MND = MND + LevelUpReward[4];
            int sum = 0;
            for (int i = 0; i < LevelUpReward.Length; i++) { sum = sum + LevelUpReward[i]; }
            if (sum == 0) { VIT++; VIT++; MND++; } else if (sum == 1) { VIT++; MND++; } else if (sum == 2) { VIT++; }
            setStatus(calcStatus());
        }
    }
    /**
    *for Debug or Reallocation(Soul Vessel)
    */
    protected void setMainStatus(int lv, int v, int s, int a, int i, int m)
    {
        Lv = lv; VIT = v; STR = s; AGI = a; INT = i; MND = m;
        setStatus(calcStatus());
    }
    protected float[] calcStatus()
    {
        float[] st = new float[statusGenus];
        // first step
        st[0] = (4 * STR) + (AGI);// AD
        st[1] = (4 * INT) + (AGI);// MD
        st[2] = Mathf.RoundToInt(STR * Mathf.Sqrt(VIT / 50));// AR
        st[3] = Mathf.RoundToInt(INT * Mathf.Sqrt(VIT / 50));// MR
        st[4] = 1 + (MND / 10);// MindSlots
        st[5] = ((float)AGI + 50) / 50;// MovementSpeed
        st[6] = 2 + ((float)AGI / 50);// RunRatio
        // second step
        st[7] = 1.0f + (STR / (100 + (float)STR));// HPGainRatio
        st[8] = 1.0f + ((STR + VIT) / (100 + (float)VIT));// HPRegenRatio
        st[9] = 1.0f + (INT / (100 + (float)INT));// SPGainRatio
        st[10] = 1.0f + (AGI / (100 + (float)AGI));// SPRegenRatio
        // third step
        st[11] = Mathf.RoundToInt(((15 * Mathf.Sqrt(VIT + Lv)) + (5 * (float)VIT) + 30) * st[7]);// MAXHP
        st[12] = Mathf.RoundToInt((VIT + MND + 58) * st[9]);// MAXSP
        // fourth step
        st[13] = Mathf.RoundToInt((st[11] / 60) * st[8]);// HPRegen
        st[14] = Mathf.RoundToInt((st[12] / 60) * st[10]);// SPRegen
        st[15] = st[11];// HP
        st[16] = st[12];// SP
        st[17] = (st[11] * MND) / (100 + (float)MND);// Poise
        return st;
    }
    protected void setStatus(float[] status)
    {
        int fix = statusGenus;
        AD = Mathf.RoundToInt(status[status.Length - fix + 0]); MD = Mathf.RoundToInt(status[status.Length - fix + 1]);
        AR = Mathf.RoundToInt(status[status.Length - fix + 2]); MR = Mathf.RoundToInt(status[status.Length - fix + 3]);
        MindSlots = Mathf.RoundToInt(status[status.Length - fix + 4]);
        MovementSpeed = status[status.Length - fix + 5];
        RunRatio = status[status.Length - fix + 6];
        HPGainRatio = status[status.Length - fix + 7]; HPRegenRatio = status[status.Length - fix + 8];
        SPGainRatio = status[status.Length - fix + 9]; SPRegenRatio = status[status.Length - fix + 10];
        MAXHP = Mathf.RoundToInt(status[status.Length - fix + 11]); MAXSP = Mathf.RoundToInt(status[status.Length - fix + 12]);
        HPRegen = Mathf.RoundToInt(status[status.Length - fix + 13]); SPRegen = Mathf.RoundToInt(status[status.Length - fix + 14]);
        HP = Mathf.RoundToInt(status[status.Length - fix + 15]); SP = Mathf.RoundToInt(status[status.Length - fix + 16]);
        Poise = Mathf.RoundToInt(status[status.Length - fix + 17]);
    }

    // ActionManagement
    protected float CD = 0.15f;
    public bool IsCD = false;
    public Vector3 DIR = new Vector3();
    public Vector3 nextPOS = new Vector3();
    public Vector3 nextnextPOS = new Vector3();
    private bool IsActing = false;
    private AAction[] actionStack = new AAction[3];

    public void AddAction(AAction AnyAction)
    {
        IsCD = true;
        if (actionStack[0] == null)
        {
            actionStack[0] = AnyAction;
            actionStack[1] = new IdleAction();
            actionStack[2] = null;
        }
        else
        {
            actionStack[1] = AnyAction;
            actionStack[2] = new IdleAction();
        }
    }
    public void DoAction()
    {
        if (!IsActing)
        {
            if (actionStack[0] == null) { }
            else
            {
                doRotate();
                actionStack[0].Action(this);
            }
        }
    }
    private void doRotate()
    {
        iTween.RotateTo(this.gameObject, iTween.Hash("y", Quaternion.LookRotation(DIR).eulerAngles.y, "time", CD*2));
    }
    public bool CanMoveToThere()
    {
        Vector3 dir2 = new Vector3(nextnextPOS.x - nextPOS.x, 0, nextnextPOS.z - nextPOS.z);
        dir2 = RoundToIntVector3XZ(dir2);
        float maxd = 1.0f;
        if (dir2.x != 0 && dir2.z != 0) { maxd = 1.5f; }
        RaycastHit hitFront; RaycastHit hitDown;
        Ray rayFront = new Ray(nextPOS + new Vector3(0, 1.5f, 0), dir2);
        Ray rayDown = new Ray(nextnextPOS + new Vector3(0, 1.5f, 0), new Vector3(0, -1, 0));
        if (Physics.Raycast(nextPOS + new Vector3(0, 1.5f, 0), dir2, out hitFront, maxd))
        {
            //            Debug.Log("hitsFront:" + hitFront.distance);
            if (hitFront.collider.tag == "Terrain" ||
                hitFront.collider.tag == "Environment" ||
                hitFront.collider.tag == "LifeSeed")
            {
                return false;
            }
        }
        if (Physics.Raycast(nextnextPOS + new Vector3(0, 1.5f, 0), -Vector3.up, out hitDown, 3.0f))
        {
            //            Debug.Log("hitDown:" + hitDown.distance);
            if (hitDown.collider.tag == "Environment" ||
                hitDown.collider.tag == "LifeSeed")
            {
                nextnextPOS.y = nextnextPOS.y + 1.5f - hitDown.distance;
            }
            else if (hitDown.collider.tag == "Terrain")
            {
                nextnextPOS.y = Terrain.activeTerrain.SampleHeight(nextnextPOS);
            }
            return true;
        }
        return false;
    }
    private void endAction()
    {
        IsActing = false;
        if (actionStack[1] != null)
        {
            actionStack[0] = actionStack[1];
            actionStack[1] = actionStack[2];
            actionStack[2] = null;
            DoAction();
        }
        else
        {
            actionStack[0] = null;
        }
    }
    public IEnumerator DoingAction(float waittime)
    {
        IsActing = true;
        yield return new WaitForSeconds(waittime);
        endAction();
    }

    // Utility
    public void TakeDamage(int value, bool AorM)
    {
        HP = 1;
    }
    public abstract void YouDied();
}
