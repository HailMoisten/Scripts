using UnityEngine;
using System.Collections;

public abstract class AAnimal : MonoBehaviour {
    // Necessary Public Methods
    public Animator GetAnimator() { return GetComponent<Animator>(); }
    public Vector3 RoundToIntVector3XZ(Vector3 pos) { return new Vector3(Mathf.RoundToInt(pos.x), pos.y, Mathf.RoundToInt(pos.z)); }

    // Status
    private const int statusGenus = 19;
    private int lv = 1; public int Lv { get { return lv; } }
    private int vit = 1; public int VIT { get { return vit; } }
    private int str = 1; public int STR { get { return str; } }
    private int agi = 1; public int AGI { get { return agi; } }
    private int @int = 1; public int INT { get { return @int; } }
    private int mnd = 1; public int MND { get { return mnd; } }
    private int ad = 1; public int AD { get { return ad; } }
    private int md = 1; public int MD { get { return md; } }
    private int ar = 1; public int AR { get { return ar; } }
    private int mr = 1; public int MR { get { return mr; } }
    private int mindslots = 1; public int MindSlots { get { return mindslots; } }
    private float movementspeed = 1.0f; public float MovementSpeed { get { return movementspeed; } }
    private float runratio = 2.0f; public float RunRatio { get { return runratio; } }
    private float HPGainBonus = 1.0f, HPRegenBonus = 1.0f, SPGainBonus = 1.0f, SPRegenBonus = 1.0f;
    private int maxhp = 1; public int MaxHP { get { return maxhp; } }
    private int maxsp = 1; public int MaxSP { get { return maxsp; } }
    private float hpregen = 1; public float HPRegen { get { return hpregen; } }
    private float spregen = 1; public float SPRegen { get { return spregen; } }
    private float hp = 1; public float HP { get { return hp; } }
    private float sp = 1; public float SP { get { return sp; } }
    private int vitalpoise = 0; public int VitalPoise { get { return vitalpoise; } }
    private int mentalpoise = 0; public int MentalPoise { get { return mentalpoise; } }
    protected int[] LevelUpReward = {0, 0, 0, 0, 0};
    public int[] GetMainStatus()
    {
        int[] mains = new int[5];
        mains[0] = vit; mains[1] = str; mains[2] = agi; mains[3] = @int; mains[4] = mnd;
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
        subs[0] = ad; subs[1] = md; subs[2] = ar; subs[3] = mr;
        subs[4] = mindslots; subs[5] = movementspeed; subs[6] = runratio;
        subs[7] = HP; subs[8] = HPRegen; subs[9] = MaxHP;
        subs[10] = SP; subs[11] = SPRegen; subs[12] = MaxSP;
        subs[13] = vitalpoise; subs[14] = mentalpoise;
        return subs;
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
        if (lv >= 165) { }
        else
        {
            lv++;
            vit = vit + LevelUpReward[0]; str = str + LevelUpReward[1]; agi = agi + LevelUpReward[2]; @int = @int + LevelUpReward[3]; mnd = mnd + LevelUpReward[4];
            int sum = 0;
            for (int i = 0; i < LevelUpReward.Length; i++) { sum = sum + LevelUpReward[i]; }
            if (sum == 0) { vit++; vit++; mnd++; } else if (sum == 1) { vit++; mnd++; } else if (sum == 2) { vit++; }
            setStatus(calcStatus(GetMainStatus()));
        }
    }

    /// <summary>
    /// For debug or Reallocation(Soul Vessel)
    /// </summary>
    /// <returns></returns>
    protected void setMainStatus(int lv, int v, int s, int a, int i, int m)
    {
        this.lv = lv; vit = v; str = s; agi = a; @int = i; mnd = m;
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
        subs[0] = (2 * str) + (agi/2);// AD
        subs[1] = (2 * @int) + (agi/2);// MD
        subs[2] = Mathf.RoundToInt(str * Mathf.Sqrt(vit / 50));// AR
        subs[3] = Mathf.RoundToInt(@int * Mathf.Sqrt(vit / 50));// MR
        subs[4] = 1 + (mnd / 10);// MindSlots
        subs[5] = ((float)agi + 100) / 100;// MovementSpeed
        subs[6] = 2 + ((float)agi / 25);// RunRatio
        // second step
        subs[7] = 1.0f + (str / (100 + (float)str));// HPGainRatio
        subs[8] = 1.0f + ((str + vit) / (100 + (float)vit));// HPRegenRatio
        subs[9] = 1.0f + (@int / (100 + (float)@int));// SPGainRatio
        subs[10] = 1.0f + (agi / (100 + (float)agi));// SPRegenRatio
        // third step
        subs[11] = Mathf.RoundToInt(((15 * Mathf.Sqrt(vit + lv)) + (5 * (float)vit) + 30) * subs[7]);// MaxHP
        subs[12] = Mathf.RoundToInt((vit + mnd + 58) * subs[9]);// MaxSP
        // fourth step
        subs[13] = (subs[11] / 60) * subs[8];// HPRegen
        subs[14] = (subs[12] / 60) * subs[10];// SPRegen
        subs[15] = subs[11];// HP
        subs[16] = subs[12];// SP
        subs[17] = (subs[11] * mnd) / (100 + (float)mnd);// VitalPoise
        subs[18] = (subs[12] * ((10 - ((float)mnd / 10)) / 100));// MentalPoise

        // Calculate buffs against to subs.

        return subs;
    }
    protected void setStatus(float[] status)
    {
        int fix = statusGenus;
        ad = Mathf.RoundToInt(status[status.Length - fix + 0]); md = Mathf.RoundToInt(status[status.Length - fix + 1]);
        ar = Mathf.RoundToInt(status[status.Length - fix + 2]); mr = Mathf.RoundToInt(status[status.Length - fix + 3]);
        mindslots = Mathf.RoundToInt(status[status.Length - fix + 4]);
        movementspeed = status[status.Length - fix + 5];
        runratio = status[status.Length - fix + 6];
        HPGainBonus = status[status.Length - fix + 7]; HPRegenBonus = status[status.Length - fix + 8];
        SPGainBonus = status[status.Length - fix + 9]; SPRegenBonus = status[status.Length - fix + 10];
        maxhp = Mathf.RoundToInt(status[status.Length - fix + 11]); maxsp = Mathf.RoundToInt(status[status.Length - fix + 12]);
        hpregen = status[status.Length - fix + 13]; spregen = status[status.Length - fix + 14];
        hp = Mathf.RoundToInt(status[status.Length - fix + 15]); sp = Mathf.RoundToInt(status[status.Length - fix + 16]);
        vitalpoise = Mathf.RoundToInt(status[status.Length - fix + 17]);
        mentalpoise = Mathf.RoundToInt(status[status.Length - fix + 18]);
    }

    // ActionManagement
    private float gcd = 0.15f; public float GCD { get { return gcd; } }
    protected bool isInput = false;
    protected IEnumerator InputCD()
    {
        yield return new WaitForSeconds(gcd);
        isInput = false;
    }
    public Vector3 DIR = new Vector3();
    public Vector3 nextPOS = new Vector3();
    public Vector3 nextnextPOS = new Vector3();
    private bool isActing = false;
    private AAction[] actionStack = new AAction[3];
    protected GameObject actiondummy;

    public void AddAction(AAction AnyAction)
    {
        isInput = true;
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
        // Calc. buffs and regens. Check DOA.
        if (isPassed) {  }
        else { passed(); StartCoroutine(passedCD()); }
        if (checkDOA()) { YouDied(); }

        // Do action.
        if (isActing) { }
        else
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
    public IEnumerator DoingAction(float waittime)
    {
        isActing = true;
        yield return new WaitForSeconds(waittime);
        endAction();
    }

    private void doRotate()
    {
        iTween.RotateTo(this.gameObject, iTween.Hash("y", Quaternion.LookRotation(DIR).eulerAngles.y, "time", gcd*2));
    }

    private void endAction()
    {
        isActing = false;
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

    // Utility
    private bool isPassed = false;
    private IEnumerator passedCD()
    {
        isPassed = true;
        yield return new WaitForSeconds(0.1f);
        isPassed = false;
    }
    /// <summary>
    /// Calc regens and buffs.
    /// </summary>
    private void passed()
    {
        // Regens
        hp = HP + (HPRegen/10); if (HP > MaxHP) { hp = MaxHP; }
        sp = SP + (SPRegen/10); if (SP > MaxSP) { sp = MaxSP; }
        // Buffs
    }
    /// <summary>
    /// Return Dead or Alive.
    /// Dead: return true; Alive: return false;
    /// </summary>
    /// <returns></returns>
    private bool checkDOA()
    {
        if (HP <= 0) { return true; }
        return false;
    }
    public abstract void YouDied();

    public void TakeDamage(int attackdamage, int magicdamage)
    {
        int damage = Mathf.RoundToInt((attackdamage * (1 - (AR / (100 + (float)AR)))) + (magicdamage * (1 - (MR / (100 + (float)MR)))));
        Debug.Log(damage);
        hp = HP - damage;
        if (HP < 0) { hp = 0; }
    }
    public void UseHPSP(int hpcost, int spcost, int hppercentcost, int sppercentcost)
    {
        hp = HP - hpcost; sp = SP - spcost;
        hp = HP - (MaxHP * (hppercentcost / 100)); sp = SP - (MaxSP * (sppercentcost / 100));
        if (HP < 0) { hp = 0; }
    }
}
