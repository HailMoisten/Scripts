using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public abstract class AAnimal : MonoBehaviour {
    public abstract void Awake();
    // Necessary Public Methods
    public Animator GetAnimator() { return GetComponent<Animator>(); }
    public Vector3 RoundToIntVector3XZ(Vector3 pos) { return new Vector3(Mathf.RoundToInt(pos.x), pos.y, Mathf.RoundToInt(pos.z)); }

    // Status
    private const int statusGenus = 19;
    private int lv = 1; public int Lv { get { return lv; } }
    private int vit = 1; public int VIT { get { return vit; } }
    private int str = 1; public int STR { get { return str; } }
    private int agi = 1; public int AGI { get { return agi; } }
    private int _int = 1; public int INT { get { return _int; } }
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
        mains[0] = vit; mains[1] = str; mains[2] = agi; mains[3] = _int; mains[4] = mnd;
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
            vit = vit + LevelUpReward[0]; str = str + LevelUpReward[1]; agi = agi + LevelUpReward[2]; _int = _int + LevelUpReward[3]; mnd = mnd + LevelUpReward[4];
            int sum = 0;
            for (int i = 0; i < LevelUpReward.Length; i++) { sum = sum + LevelUpReward[i]; }
            if (sum == 0) { vit++; vit++; mnd++; } else if (sum == 1) { vit++; mnd++; } else if (sum == 2) { vit++; }
            setSubStatus(calcSubStatus());
            hp = MaxHP; sp = MaxSP;
        }
    }

    /// <summary>
    /// For debug or Reallocation(Soul Vessel)
    /// </summary>
    /// <returns></returns>
    protected void setMainStatus(int lv, int v, int s, int a, int i, int m)
    {
        this.lv = lv; vit = v; str = s; agi = a; _int = i; mnd = m;
        setSubStatus(calcSubStatus());
        hp = MaxHP; sp = MaxSP;
    }

    /// <summary>
    /// Calculate sub states. In addition, calculate buffs.
    /// </summary>
    /// <returns></returns>
    protected float[] calcSubStatus()
    {
        int[] mains = { VIT, STR, AGI, INT, MND };
        // Calculate buffs against to mains for sub states. Main status must not be buffed.
        ABuff buff;
        for (int i = 0; i < Buffs.transform.childCount; i++ )
        {
            buff = Buffs.transform.GetChild(i).GetComponent<ABuff>();
            mains = buff.BuffToMainStatus(mains);
        }
        // Calculate sub states.
        float[] subs = new float[statusGenus];
        // first step
        subs[0] = (2 * mains[1]) + (mains[2] / 2);// AD
        subs[1] = (2 * mains[3]) + (mains[2] / 2);// MD
        subs[2] = mains[1] + ((float)mains[0] / 2);// AR
        subs[3] = mains[3] + ((float)mains[0] / 2);// MR
        subs[4] = 1 + (mains[4] / 10);// MindSlots
        subs[5] = ((float)mains[2] + 100) / 100;// MovementSpeed
        subs[6] = 2 + 3*((float)mains[2] / 100);// RunRatio
        // second step
        subs[7] = 1.0f + (mains[1] / (100 + (float)mains[1]));// HPGainRatio
        subs[8] = 1.0f + ((mains[1] + mains[0]) / (100 + (float)mains[0]));// HPRegenRatio
        subs[9] = 1.0f + (mains[3] / (100 + (float)mains[3]));// SPGainRatio
        subs[10] = 1.0f + (mains[2] / (100 + (float)mains[2]));// SPRegenRatio
        // third step
        subs[11] = Mathf.RoundToInt(((15 * Mathf.Sqrt(mains[0] + Lv)) + (5 * (float)mains[0]) + 30) * subs[7]);// MaxHP
        subs[12] = Mathf.RoundToInt((mains[0] + mains[4] + 58) * subs[9]);// MaxSP
        // fourth step
        subs[13] = (subs[11] / 60) * subs[8];// HPRegen
        subs[14] = (subs[12] / 60) * subs[10];// SPRegen
        subs[15] = (subs[11] * mains[4]) / (100 + (float)mains[4]);// VitalPoise
        subs[16] = (subs[12] * ((10 - ((float)mains[4] / 10)) / 100));// MentalPoise
        subs[17] = HP;// HP
        subs[18] = SP;// SP

        // Calculate buffs against to subs and HPSP.
        for (int i = 0; i < Buffs.transform.childCount; i++)
        {
            buff = Buffs.transform.GetChild(i).GetComponent<ABuff>();
            subs = buff.BuffToSubStatus(subs);
            subs[17] = buff.BuffToHP(subs[17]);
            subs[18] = buff.BuffToSP(subs[18]);
            if (buff.IsUsed) { }
            else { subs[17] = buff.BuffToHPOnlyOnce(subs[17]); subs[18] = buff.BuffToSPOnlyOnce(subs[18]);
                buff.Used(); }
        }

        return subs;
    }
    protected void setSubStatus(float[] subs)
    {
        int fix = statusGenus;
        ad = Mathf.RoundToInt(subs[subs.Length - fix + 0]); md = Mathf.RoundToInt(subs[subs.Length - fix + 1]);
        ar = Mathf.RoundToInt(subs[subs.Length - fix + 2]); mr = Mathf.RoundToInt(subs[subs.Length - fix + 3]);
        mindslots = Mathf.RoundToInt(subs[subs.Length - fix + 4]);
        movementspeed = subs[subs.Length - fix + 5];
        runratio = subs[subs.Length - fix + 6];
        HPGainBonus = subs[subs.Length - fix + 7]; HPRegenBonus = subs[subs.Length - fix + 8];
        SPGainBonus = subs[subs.Length - fix + 9]; SPRegenBonus = subs[subs.Length - fix + 10];
        maxhp = Mathf.RoundToInt(subs[subs.Length - fix + 11]); maxsp = Mathf.RoundToInt(subs[subs.Length - fix + 12]);
        hpregen = subs[subs.Length - fix + 13]; spregen = subs[subs.Length - fix + 14];
        vitalpoise = Mathf.RoundToInt(subs[subs.Length - fix + 15]);
        mentalpoise = Mathf.RoundToInt(subs[subs.Length - fix + 16]);
        hp = Mathf.RoundToInt(subs[subs.Length - fix + 17]); sp = Mathf.RoundToInt(subs[subs.Length - fix + 18]);
    }

    // ActionManagement
    private float gcd = 0.09f; public float GCD { get { return gcd; } }
    protected bool isInput = false;
    protected IEnumerator InputCD()
    {
        yield return new WaitForSeconds(gcd);
        isInput = false;
    }
    public Vector3 DIR = new Vector3();
    public Vector3 POS = new Vector3();
    public Vector3 nextPOS = new Vector3();
    public Vector3 nextnextPOS = new Vector3();
    public Vector3 targetPOS = new Vector3();
    private bool isActing = false;
    public bool Interrupting { get; set; }
    public AAction[] actionStack = new AAction[3];
    protected GameObject mainActionPool;
    protected void setMainActionPool()
    {
        mainActionPool = new GameObject("MainActionPool");
        mainActionPool.transform.SetParent(transform);
        mainActionPool.AddComponent<Idle>();
        mainActionPool.AddComponent<Walk>();
        mainActionPool.AddComponent<Run>();
        mainActionPool.AddComponent<Attack>();
        mainActionPool.AddComponent<Guard>();
        mainActionPool.AddComponent<Stunned>();
        mainActionPool.AddComponent<PickUp>();
    }

    public void AddAction(AAction AnyAction)
    {
        isInput = true;
        if (Interrupting) { }
        else {
            if (actionStack[0] == null)
            {
                actionStack[0] = AnyAction;
                actionStack[1] = mainActionPool.GetComponent<Idle>();
                actionStack[2] = null;
            }
            else
            {
                actionStack[1] = AnyAction;
                actionStack[2] = mainActionPool.GetComponent<Idle>();
            }
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
                else
                {
                    actionStack[0] = mainActionPool.GetComponent<Idle>();
                    GameObject ecanvas = Instantiate((GameObject)Resources.Load("Prefabs/GUI/ErrorTextCanvas"));
                    ecanvas.GetComponent<ErrorTextCanvasManager>().SetAndDestroy(3);
                }
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
        transform.position = nextPOS;
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
    public Transform Inventory { get; set; }
    public Transform ItemBag { get; set; }
    public Transform WeaponBag { get; set; }
    public Transform RingBag { get; set; }
    public Transform MindBag { get; set; }
    public Transform Equipment { get; set; }
    public Transform Weapon { get; set; }
    public Transform Ring { get; set; }
    public Transform Mind { get; set; }
    public Transform Buffs { get; set; }
    public AAction[] actionShortcuts;
    protected abstract void setUtilities();
    protected abstract void setActionShortcuts();
    public AAction SubmitAction;
    protected AAnimal targetAnimal = null;

    private bool isPassed = false;
    private IEnumerator passedCD()
    {
        isPassed = true;
        yield return new WaitForSeconds(0.5f);
        isPassed = false;
    }
    /// <summary>
    /// Calc regens and buffs.
    /// </summary>
    private void passed()
    {
        // Regens
        hp = HP + (HPRegen/2); if (HP > MaxHP) { hp = MaxHP; }
        sp = SP + (SPRegen/2); if (SP > MaxSP) { sp = MaxSP; }
        // Buffs
        if (SP < MentalPoise)
        {
            bool isExhausted = false;
            foreach(Transform b in Buffs.transform)
            {
                if (b.gameObject.GetComponent<ABuff>().Name == "Exhausted") { isExhausted = true; }
            }
            if (isExhausted) { }
            else {
                GameObject buff = (GameObject)Instantiate(Resources.Load("Prefabs/Buffs/Exhausted"), Vector3.zero, Quaternion.identity);
                buff.transform.SetParent(Buffs.transform);
            }
        }
        setSubStatus(calcSubStatus());
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
        int a = Mathf.RoundToInt(attackdamage * (1 - (AR / (100 + (float)AR))));
        int m = Mathf.RoundToInt(magicdamage * (1 - (MR / (100 + (float)MR))));
        if (a + m >= VitalPoise)
        {
            AddAction(mainActionPool.GetComponent<Stunned>());
            Interrupting = true;
        } 
        hp = HP - (a + m);
        if (HP < 0) { hp = 0; }
        GameObject dcanvas = Instantiate((GameObject)Resources.Load("Prefabs/GUI/DamageTextCanvas"));
        dcanvas.GetComponent<DamageTextCanvasManager>().SetAndDestroy(a, m, transform.position);

    }
    public void UseHPSP(int hpcost, int spcost, int hppercentcost, int sppercentcost)
    {
        hp = HP - hpcost; sp = SP - spcost;
        hp = HP - (MaxHP * (hppercentcost / 100)); sp = SP - (MaxSP * (sppercentcost / 100));
        if (HP < 0) { hp = 0; }
    }
    /// <summary>
    /// kowai
    /// </summary>
    /// <param name="colliderInfo"></param>
    protected void OnTriggerEnter(Collider colliderInfo)
    {
        if (colliderInfo.gameObject.tag == "Animal")
        {
            targetAnimal = colliderInfo.gameObject.GetComponent<AAnimal>();
        }
        else if (colliderInfo.gameObject.tag == "Item")
        {
            mainActionPool.GetComponent<PickUp>().TargetItem = colliderInfo.gameObject.GetComponent<AItem>();
            SubmitAction = mainActionPool.GetComponent<PickUp>();
        }
        else if (colliderInfo.gameObject.tag == "DamageField")
        {
            targetAnimal = colliderInfo.gameObject.GetComponent<ADamageField>().Creator;
        }
    }
    protected void OnTriggerExit(Collider colliderInfo)
    {
        if (colliderInfo.gameObject.tag == "Item")
        {
            SubmitAction = null;
        }
    }
    protected void OnCollisionEnter(Collision collisionInfo)
    {
        if (collisionInfo.gameObject.tag == "Animal")
        {
            AAnimal target = collisionInfo.gameObject.GetComponent<AAnimal>();
            target.TakeDamage(AD, 0);
            transform.position = POS;
        }
    }

}
