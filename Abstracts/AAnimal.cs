using UnityEngine;
using System.Collections;

public abstract class AAnimal : MonoBehaviour {
    // Necessary Public Methods
    public Animator GetAnimator() { return GetComponent<Animator>(); }
    public Vector3 RoundToIntVector3XZ(Vector3 pos) { return new Vector3(Mathf.RoundToInt(pos.x), pos.y, Mathf.RoundToInt(pos.z)); }

    // Status
    private const int statusGenus = 19;
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
    protected int VitalPoise = 0, MentalPoise = 0;
    protected int[] LevelUpReward = {0, 0, 0, 0, 0};
    public int GetLevel() { return Lv; }
    public int[] GetMainStatus()
    {
        int[] mains = new int[5];
        mains[0] = VIT; mains[1] = STR; mains[2] = AGI; mains[3] = INT; mains[4] = MND;
        return mains;
    }
    /// <summary>
    /// Return Sub States.
    /// 0: AD; 1: MD; 2: AR; 3: MR;
    /// 4: MindSlots; 5: MovementSpeed; 6: RunRatio;
    /// 7: HP; 8: HPRegen; 9: MAXHP;
    /// 10: SP; 11: SPRegen; 12: MAXSP;
    /// 13: VitalPoise; 14:MentalPoise;
    /// </summary>
    public float[] GetSubStatus()
    {
        float[] subs = new float[15];
        subs[0] = AD; subs[1] = MD; subs[2] = AR; subs[3] = MR;
        subs[4] = MindSlots; subs[5] = MovementSpeed; subs[6] = RunRatio;
        subs[7] = HP; subs[8] = HPRegen; subs[9] = MAXHP;
        subs[10] = SP; subs[11] = SPRegen; subs[12] = MAXSP;
        subs[13] = VitalPoise; subs[14] = MentalPoise;
        return subs;
    }
    public int GetHP(){ return HP; }
    public int GetSP() { return SP; }

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
            setStatus(calcStatus(GetMainStatus()));
        }
    }

    /// <summary>
    /// For debug or Reallocation(Soul Vessel)
    /// </summary>
    /// <returns></returns>
    protected void setMainStatus(int lv, int v, int s, int a, int i, int m)
    {
        Lv = lv; VIT = v; STR = s; AGI = a; INT = i; MND = m;
        setStatus(calcStatus(GetMainStatus()));
    }

    /// <summary>
    /// Calculate sub states. In addition, calculate buffs.
    /// </summary>
    /// <returns></returns>
    protected float[] calcStatus(int[] mains)
    {
        // Calculate buffs against to mains for sub states. Main status must not be buffed.

        // Calculate sub states.
        float[] subs = new float[statusGenus];
        // first step
        subs[0] = (4 * STR) + (AGI);// AD
        subs[1] = (4 * INT) + (AGI);// MD
        subs[2] = Mathf.RoundToInt(STR * Mathf.Sqrt(VIT / 50));// AR
        subs[3] = Mathf.RoundToInt(INT * Mathf.Sqrt(VIT / 50));// MR
        subs[4] = 1 + (MND / 10);// MindSlots
        subs[5] = ((float)AGI + 100) / 100;// MovementSpeed
        subs[6] = 2 + ((float)AGI / 25);// RunRatio
        // second step
        subs[7] = 1.0f + (STR / (100 + (float)STR));// HPGainRatio
        subs[8] = 1.0f + ((STR + VIT) / (100 + (float)VIT));// HPRegenRatio
        subs[9] = 1.0f + (INT / (100 + (float)INT));// SPGainRatio
        subs[10] = 1.0f + (AGI / (100 + (float)AGI));// SPRegenRatio
        // third step
        subs[11] = Mathf.RoundToInt(((15 * Mathf.Sqrt(VIT + Lv)) + (5 * (float)VIT) + 30) * subs[7]);// MAXHP
        subs[12] = Mathf.RoundToInt((VIT + MND + 58) * subs[9]);// MAXSP
        // fourth step
        subs[13] = Mathf.RoundToInt((subs[11] / 60) * subs[8]);// HPRegen
        subs[14] = Mathf.RoundToInt((subs[12] / 60) * subs[10]);// SPRegen
        subs[15] = subs[11];// HP
        subs[16] = subs[12];// SP
        subs[17] = (subs[11] * MND) / (100 + (float)MND);// VitalPoise
        subs[18] = (subs[12] * ((10 - ((float)MND / 5)) / 100));// MentalPoise

        // Calculate buffs against to subs.

        return subs;
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
        VitalPoise = Mathf.RoundToInt(status[status.Length - fix + 17]);
        MentalPoise = Mathf.RoundToInt(status[status.Length - fix + 18]);
    }

    // ActionManagement
    protected float CD = 0.15f;
    public bool IsCD = false;
    public Vector3 DIR = new Vector3();
    public Vector3 nextPOS = new Vector3();
    public Vector3 nextnextPOS = new Vector3();
    private bool IsActing = false;
    private AAction[] actionStack = new AAction[3];
    protected GameObject actiondummy;

    public void AddAction(AAction AnyAction)
    {
        IsCD = true;
        if (actionStack[0] == null)
        {
            actionStack[0] = AnyAction;
            actionStack[1] = actiondummy.AddComponent<IdleAction>();
            actionStack[2] = null;
        }
        else
        {
            actionStack[1] = AnyAction;
            actionStack[2] = actiondummy.AddComponent<IdleAction>();
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
                if (actionStack[0].CanDoAction(this)) { }
                else { actionStack[0] = actiondummy.AddComponent<IdleAction>(); Debug.Log("I can NOT do it."); }
                actionStack[0].Action(this);
            }
        }
    }
    private void doRotate()
    {
        iTween.RotateTo(this.gameObject, iTween.Hash("y", Quaternion.LookRotation(DIR).eulerAngles.y, "time", CD*2));
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
    public void TakeDamage(int value, bool isAnotM)
    {
        int damage = 0;
        if (isAnotM) { damage = value * (1 - (AR / (100 + AR))); }
        else { damage = value * (1 - (MR / (100 + MR))); }
        HP = HP - damage;
    }
    public void UseHPSP(int hpcost, int spcost, int hppercentcost, int sppercentcost)
    {
        HP = HP - hpcost; SP = SP - spcost;
        HP = HP - (MAXHP * (hppercentcost / 100)); SP = SP - (MAXSP * (sppercentcost / 100));
    }
    public abstract void YouDied();
}
