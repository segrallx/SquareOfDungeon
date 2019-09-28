using UnityEngine;

public struct EffectArg
{
    public bool IsInfectAdj;  //是否传染周围的人
    public bool IsSource;  //是否为源头

    public static EffectArgFun InfectAdj()
    {
        return delegate (EffectArg efArgs)
        {
            efArgs.IsInfectAdj = true;
            return efArgs;
        };
    }

    public static EffectArgFun SetSource()
    {
        return delegate (EffectArg efArgs)
        {
            efArgs.IsSource = true;
            return efArgs;
        };
    }
}

public delegate EffectArg EffectArgFun(EffectArg efArgs);

public class CharRender : MonoBehaviour
{
    protected Transform mTransform;
    protected Animator mAnimator;
    protected Vector3 mYvec;
    private int mTwinkTime;
    private Color mColor;
    //private LineRenderer lineRenderer;
    //private Dictionary<int, Char> mAdjacentChars;
    private Char _mChar;
    private GameObject _mEffectVfx;
    public Transform mEffectVfxTrans;

    public Char mChar
    {
        get { return _mChar; }
        set { _mChar = value; }
    }

    public GameObject mMesh;
    protected HpBarUI mHPBarUI;
    private Camera mCamera;

    // NPC模型高度
    private float mHigh = 1.5f;

    [SerializeField]
    Vector3 MovePos
    {
        get
        {
            return mChar.mState.mMovePos;
        }
    }

    [SerializeField]
    Vector2Int Idx
    {
        get
        {
            return mChar.mState.mCurIdx;
        }
    }

    private CharEffect mEffect;
    private Char mEffectChar;


    public void MazeSet(Maze maze)
    {
        mChar.MazeSet(maze);
    }

    public Char CharGet()
    {
        return mChar;
    }

    public void CharSet(Char c)
    {
        mChar = c;
        mChar.mCharRender = this;
    }


    // Use this for initialization
    public void InitComponent()
    {
        mYvec = new Vector3(0, transform.position.y, 0);
        mTransform = GetComponent<Transform>();
        mAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    // 主要进行UI显示插值计算.
    public virtual void Update()
    {
        Ui3DUpdate();
        RotationUpdate();
        PositionUpdate();
        EffectUpdate();
    }


    // 方向更新
    protected void RotationUpdate()
    {
        if (mChar == null)
        {
            return;
        }

        var rotate = mChar.MoveRotationY();
        //transform.rotation = rotate;
        if (Vector3.Distance(transform.rotation.eulerAngles,
                             rotate.eulerAngles) > 0.1f)
        {
            transform.rotation =
                Quaternion.Slerp(transform.rotation,
                                 rotate, Time.deltaTime * 3.0f * 3);
        }
    }


    // 位置更新
    protected void PositionUpdate()
    {
        var distance = Vector3.Distance(mTransform.position, mChar.mState.mMovePos);
        if (distance > 0.1f)
        {
            mTransform.position = Vector3.Lerp(mChar.mState.mMovePos,
                                               mTransform.position, 0.8f);
        }
        else
        {
            mTransform.position = mChar.mState.mMovePos;
        }
        //Debug.LogFormat("pos {0}  move pos {1}", mTransform.position , mChar.MovePos());
    }




    // 主要进行业务逻辑计算
    public virtual void FixedUpdate()
    {
        if (mChar == null)
        {
            return;
        }

        if (mTwinkTime > 0)
        {
            mTwinkTime -= 2;
            switch (Random.Range(0, 2))
            {
                case 0:
                    mMesh.GetComponent<SkinnedMeshRenderer>().material.color = Color.white;
                    break;
                case 1:
                    mMesh.GetComponent<SkinnedMeshRenderer>().material.color = Color.red;
                    break;
            }
        }
        else if (mTwinkTime < 0)
        {
            mMesh.GetComponent<SkinnedMeshRenderer>().material.color = Color.white;
            mTwinkTime = 0;
        }
    }


    public void PositionSet(Vector2Int idx)
    {
        transform.position = Unit.TileIdxPos(idx, mYvec.y);
        Debug.LogFormat("{0} set pos {1} {2}", mChar.mCls, transform.position, idx);
        mChar.mState.mMovePos = (transform.position);
        mChar.mState.mCurIdx = idx;
    }

    public virtual void AtkBegin()
    {
        //Debug.LogFormat("{0} atk begin", mChar.mCls);
        //mChar.mIsAtking = true;
    }

    public virtual void AtkEnd()
    {
        //Debug.LogFormat("{0} atk end", mChar.mCls);
        //mChar.mIsAtking = false;
    }

    public bool IsAtking()
    {
        //return mChar.mIsAtking;
        return false;
    }

    void Start()
    {
        var hpBarPrefab = Resources.Load<GameObject>("Char/HPBarUI");
        var ui3D = GameObject.FindGameObjectWithTag("3DUI");
        //mCamera = GameObject.FindGameObjectWithTag("UICamera").GetComponent<Camera>();
        mCamera = Camera.main;
        mHPBarUI = Instantiate(hpBarPrefab, ui3D.transform).GetComponent<HpBarUI>();
        var gameObjPos = mCamera.WorldToScreenPoint(transform.position);
        mHPBarUI.SetPostion(gameObjPos);
        mHigh = mMesh.GetComponent<Collider>().bounds.size.y * mTransform.localScale.y;
        //Debug.LogFormat("mhigh {0} {1}", mHigh, mChar.IsMonster());
        if (mChar.IsMonster())
        {
            mHPBarUI.SetColor(Color.red);
            mHPBarUI.HiddenHpText();
        }
        else
        {
            mHPBarUI.SetActive(false);
        }
    }

    void Ui3DUpdate()
    {
        Vector3 worldPosition = new Vector3(transform.position.x,
                                            transform.position.y + mHigh + 3f,
                                            transform.position.z);
        //var gameObjPos = RectTransformUtility.WorldToScreenPoint(Camera.main, transform.position);
        var gameObjPos = mCamera.WorldToScreenPoint(worldPosition);
        mHPBarUI.SetPostion(gameObjPos);
    }

    public void Destory()
    {
        mHPBarUI.Destory(); ;
    }

    protected void HpUIUpdate()
    {
        var mf = mChar.mFight;
        if (mHPBarUI != null)
        {
            mHPBarUI.SetCurHp(mf.Hp, mf.MaxHp);
        }
    }

    public virtual void Dead()
    {
        mChar.Dead();
    }

    public virtual bool BeAttacked(Char c)
    {
        var isDead = mChar.BeAttacked(c);
        HpUIUpdate();
        Debug.LogFormat("{0} be attack ", mChar.mCls);

        if (isDead)
        {
            Dead();
            GetComponent<AudioSource>().Play();
            Invoke("DestorySelf", 0.1f);
        }

        return isDead;
    }


    public virtual bool BeTraped(Fight f)
    {
        var isDead = mChar.BeTraped(f);
        HpUIUpdate();
        Debug.LogFormat("{0} be attack ", mChar.mCls);

        if (isDead)
        {
            Dead();
            GetComponent<AudioSource>().Play();
            Invoke("DestorySelf", 0.1f);
        }

        return isDead;
    }


    public virtual bool BeEffected(Char c)
    {
        var isDead = mChar.BeEffected(c);
        HpUIUpdate();
        Debug.LogFormat("{0} be effected ", mChar.mCls);


        if (isDead)
        {
            Dead();
            GetComponent<AudioSource>().Play();
            Invoke("DestorySelf", 0.1f);
        }

        return isDead;
    }


    public virtual void OnTriggerEnter(Collider coll)
    {
        if (!mChar.MazeActive())
        {
            return;
        }

        switch (coll.tag)
        {
            case "Block":
                return;
            case "Player":
                return;
        }

        Debug.LogFormat("{0} trigger enter {1}", mChar.mCls, coll.tag);
        ITrapCsvCfg itrap;
        Fight fight;

        switch (coll.tag)
        {
            case "Thorn":
                itrap = coll.gameObject.transform.parent.GetComponent<LandThorn>();
                fight = CsvAgent.Instance().GetFightByTrapId(itrap.CsvCfgId());
                BeTraped(fight);
                break;
            case "Axe":
                itrap = coll.GetComponent<DefenceAxeBody>().mOwner;
                fight = CsvAgent.Instance().GetFightByTrapId(itrap.CsvCfgId());
                BeTraped(fight);
                break;
            case "MonsterWeapon":
                fight = coll.GetComponent<FightOwner>().mOwner.mChar.mFight;
                BeTraped(fight);
                break;
            case "Sword":
                var swordObj = coll.GetComponent<Sword>();
                var isDead = BeAttacked(swordObj.Owner.mChar);
                if (isDead)
                {
                    //swordObj.Owner.ExpAdd(mChar.GetExp());
                }
                break;

            case "FlyObject":
                var caster = coll.GetComponent<FlyObject>().mOwner;
                coll.gameObject.SetActive(false);
                switch (caster.tag)
                {
                    case "DefenceBow":
                        itrap = caster.GetComponent<DefenceBow>();
                        fight = CsvAgent.Instance().GetFightByTrapId(itrap.CsvCfgId());
                        BeTraped(fight);
                        break;
                    case "Monster":
                        var c = caster.GetComponent<MonsterRender>().mChar;
                        BeAttacked(c);
                        break;
                }
                break;
        }
    }


    ////////////////////////////////////////
    // efffect


    protected void EffectUpdate()
    {
        if (mEffect == CharEffect.None)
        {
            return;
        }

        if (mEffectTime < 0)
        {
            return;
        }

        mEffectTime -= Time.deltaTime;
        if (mEffectTime < 0)
        {
            EffectLeave();
            return;
        }

        switch (mEffect)
        {
            case CharEffect.Ice:
                effectUpdateIce();
                break;
            case CharEffect.Fire:
                effectUpdateFire();
                break;
            case CharEffect.Electro:
                effectUpdateElectro();
                break;
        }
    }


    protected void EffectLeave()
    {
        if (mEffect == CharEffect.None)
        {
            return;
        }

        switch (mEffect)
        {
            case CharEffect.Ice:
                effectLeaveIce();
                break;
            case CharEffect.Fire:
                effectLeaveFire();
                break;
            case CharEffect.Electro:
                effectLeaveElectro();
                break;
        }


        mEffect = CharEffect.None;
        if (_mEffectVfx != null)
        {
            _mEffectVfx.SetActive(false);
        }
    }


    protected void EffectEnter(EffectArg arg)
    {
        if (mEffect == CharEffect.None)
        {
            return;
        }

        switch (mEffect)
        {
            case CharEffect.Ice:
                effectEnterIce(arg);
                break;
            case CharEffect.Fire:
                effectEnterFire(arg);
                break;
            case CharEffect.Electro:
                effectEnterElectro(arg);
                break;
        }
    }


    ////////////////////
    // 冰冻效果 -- 暂停行动
    private float mEffectTime = 0;
    private float mEffectTimeSpan = 0;
    protected void effectEnterIce(EffectArg arg)
    {
        Debug.LogFormat("effectEnterIce");
        //mEffectTime = 3.0f;
        mAnimator.speed = 0;
        mChar.mState.PauseSet(3.0f);

    }

    protected void effectLeaveIce()
    {
        Debug.LogFormat("effectLeaveIce");
        mAnimator.speed = 1;
    }

    protected void effectUpdateIce()
    {
    }

    ////////////////////
    // 燃烧效果 -- 点燃后，持续掉血
    protected void effectEnterFire(EffectArg arg)
    {
        Debug.LogFormat("{0} effectEnterFire", mChar.mCls);
        //mEffectTime = 3.0f;
        //mMesh.GetComponent<SkinnedMeshRenderer>().material.color = Color.red;
    }

    protected void effectLeaveFire()
    {
        Debug.LogFormat("{0} effectLeaveFire", mChar.mCls);
        //mMesh.GetComponent<SkinnedMeshRenderer>().material.color = Color.white;
    }

    protected void effectUpdateFire()
    {
        mEffectTimeSpan += Time.deltaTime;
        if (mEffectTimeSpan > 1.0f)
        {
            mEffectTimeSpan = 0.0f;
            BeEffected(mEffectChar);
        }
    }

    ////////////////////
    // 闪电效果
    protected void effectEnterElectro(EffectArg arg)
    {

        Debug.LogFormat("{0} effectEnterElectro", mChar.mCls);
        //mEffectTime = 0.3f;
        mEffectTimeSpan = 0.1f;
        //mMesh.GetComponent<SkinnedMeshRenderer>().material.color = Color.yellow;

        // if (_mEffectVfx != null)
        // {
        //     _mEffectVfx.SetActive(true);
        // }
        // else
        // {
        //     _mEffectVfx = Instantiate(StaffHolder.Instance().mVfxElectroic, transform);
        // }

        // // 传染相邻
        // if (arg.IsInfectAdj)
        // {
        //     //checkInitLineRender();
        //     //lineRenderer.enabled = true;

        //     var maze = StaffHolder.Instance().mMaze;
        //     var thunder = StaffHolder.Instance().mThunder;
        //     List<Transform> transList = new List<Transform>();
        //     transList.Add(transform);

        //     var charList = maze.GetAdjacentChar(mChar);
        //     Debug.LogFormat("{0} effect IsInfectAdj Count {1}", mChar.mCls, charList.Count);
        //     foreach (KeyValuePair<int, Char> kv in charList)
        //     {
        //         if (kv.Value.mCharRender.mChar.IsDead())
        //         {
        //             continue;
        //         }
        //         kv.Value.mCharRender.EffectSet(CharEffect.Electro, mEffectChar);
        //         transList.Add(kv.Value.mCharRender.transform);
        //     }
        //     mAdjacentChars = charList;
        //     if (transList.Count > 1)
        //     {
        //         var thunderVfx = Instantiate(thunder, StaffHolder.Instance().mMazeRender.transform);
        //         thunderVfx.GetComponent<Thunder>().SetPointList(transList, mEffectTime);
        //     }
        // }
        // else
        // {
        //     Debug.LogFormat("{0} effect No InfectAdj", mChar.mCls);
        // }


    }


    protected void effectLeaveElectro()
    {
        Debug.LogFormat("{0} effectLeaveElectro", mChar.mCls);
        //mMesh.GetComponent<SkinnedMeshRenderer>().material.color = Color.white;
        // if (mAdjacentChars != null)
        // {
        //     mAdjacentChars.Clear();
        // }

        // if (_mEffectVfx != null)
        // {
        //     _mEffectVfx.SetActive(false);
        // }

    }

    protected void effectUpdateElectro()
    {
        if (mEffectTimeSpan > 0.0f)
        {
            mEffectTimeSpan = 0.0f;
            BeEffected(mEffectChar);
        }
    }


    private GameObject EffectVfx()
    {
        var staff = StaffHolder.Instance();
        switch (mEffect)
        {
            case CharEffect.Ice:
                return staff.mVfxIce;
            case CharEffect.Fire:
                return staff.mVfxFire;
            case CharEffect.Electro:
                return staff.mVfxElectroic;
        }
        return null;
    }

    protected void EffectSet(CharEffect eff, Char c, params EffectArgFun[] argsFunList)
    {
        if (mChar.IsDead())
        {
            return;
        }


        EffectArg arg = new EffectArg();
        foreach (var fun in argsFunList)
        {
            arg = fun(arg);
        }

        var preEffect = mEffect;
        EffectLeave();

        mEffect = eff;
        mEffectChar = c;
        mEffectTimeSpan = 0.0f;
        mEffectTime = 3.0f;
        if (preEffect != mEffect)
        {
            var vfxPrefeb = EffectVfx();
            _mEffectVfx = Instantiate(EffectVfx(), mEffectVfxTrans);
        }
        else
        {
            _mEffectVfx.SetActive(true);
        }
        EffectEnter(arg);

    }

}
