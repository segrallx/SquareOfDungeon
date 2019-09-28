/*
  主要包含了对玩家角色的控制
 */
// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.UI;
// using UnityEngine.SceneManagement;

public class RoleRender : CharRender
{
    // UI refs
    public DeadUI mDeadUI;
    //public CoinUI mCoinUI;
    public BattleUI mBattleUI;
    public UpgradeUI mUpgradeUI;

    //fight component
    [SerializeField]
    internal Role mRole;

    public Sword mSword;
    public Shield mShield;
    private AudioSource mAudioSource;
    private bool mIsInit = false;

    public void RestartRole(Maze maze, int level)
    {
        mRole.Reset(mTransform.position);
        mRole.MazeSet(maze);
        mChar = mRole;
        mBattleUI.SetRole(mRole);
        Debug.LogFormat("pending upgrade {0}", mRole.LevelUpPending());

        if (level == 0 && mIsInit)
        {
            mRole.ClearFight();
            mIsInit = false;
        }
        else if (level > 0 && mIsInit == false)
        {
            mRole.InitFight();
            mHPBarUI.SetCurHp(mRole.mFight.Hp, mRole.mFight.MaxHp);
            mIsInit = true;
            mUpgradeUI.Show();
        }
    }


    public Role GetRole()
    {
        return mRole;
    }

    // Use this for initialization
    void Awake()
    {
        Debug.LogFormat("role awake ->");
        base.InitComponent();
        mAudioSource = GetComponent<AudioSource>();
        mRole = new Role(mTransform.position);
        mChar = mRole;
        mRole.mCharRender = this;
        mBattleUI.SetRole(mRole);
        Debug.LogFormat("role awake <-");
    }


    public bool IsDead()
    {
        return mRole.mFight.IsDead();
    }

    public override bool BeAttacked(Char c)
    {
        var isDead = mChar.BeAttacked(c);
        HpUIUpdate();
        Debug.LogFormat("{0} be attack ", mChar.mCls);

        if (isDead)
        {
            mDeadUI.SetActive(true);
        }

        return isDead;
    }


    public override bool BeTraped(Fight f)
    {
        var isDead = mChar.BeTraped(f);
        HpUIUpdate();
        Debug.LogFormat("{0} be attack ", mChar.mCls);

        if (isDead)
        {
            mDeadUI.SetActive(true);
        }

        return isDead;
    }

    public override bool BeEffected(Char c)
    {
        var isDead = mChar.BeEffected(c);
        HpUIUpdate();
        if (isDead)
        {
            mDeadUI.SetActive(true);
        }
        return isDead;
    }



    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        //KeyBoardInput();
        //MouseInput();
    }


    public override void FixedUpdate()
    {
        if (mChar == null)
        {
            return;
        }

        base.FixedUpdate();

        var preState = mChar.mState.mState;

        // 设置所有的角色的动画状态.
        var stateChange = mChar.FixedUpdate();
        if (stateChange)
        {
            switch (mChar.mState.mState)
            {
                case CharState.State.Move:
                    mAnimator.SetInteger("State", 1);
                    break;
                case CharState.State.Chase:
                    mAnimator.SetInteger("State", 1);
                    break;
                case CharState.State.Atk:
                    mAnimator.SetInteger("State", 0);
                    break;
                case CharState.State.Dizzy: // neverused
                    mAnimator.SetInteger("State", 0);
                    break;
                default:
                    mAnimator.SetInteger("State", 0);
                    break;
            }
        }

        AnimatorStateInfo stateinfo = mAnimator.GetCurrentAnimatorStateInfo(0);
        if (stateinfo.IsName("Attack"))
        {
            mSword.mIsAtking = true;
        }
        else
        {
            mSword.mIsAtking = false;
        }



    }


    public void MoveSet(Direction direct)
    {
        mRole.mState.MoveSet(direct);
        mAnimator.SetInteger("State", 1);
        DefenseLeave();

    }

    public void IdleSet()
    {
        // if (mRole.CharState() == Char.State.Idle)
        // {
        // 	mAnimator.SetInteger("State", 0);
        //     return;
        // }
        mRole.mState.IdleSet();
        mAnimator.SetInteger("State", 0);
        DefenseLeave();
    }

    public void DirectionSet(Direction mdir)
    {
        mRole.DirectionSet(mdir);
    }

    public void AtkSet(Char c = null)
    {
        mRole.mState.AtkSet(c);
        if (c != null)
        {
            mRole.DirectionSet(mRole.DirectionCheck(c.mState.mCurIdx));
        }

        mAnimator.SetTrigger("Attack");
        mAnimator.SetInteger("State", 0);
        DefenseLeave();
        mSword.mIsAtking = true;
        //mAnimator.SetInteger("State", 2);
    }

    public void DefenseLeave()
    {
        mAnimator.SetBool("Defence", false);
        mShield.GetComponent<Collider>().enabled = false;
    }

    public void DefenseSet()
    {
        mRole.mState.IdleSet();
        mAnimator.SetBool("Defence", true);
        mAnimator.SetInteger("State", 0);
        mShield.GetComponent<Collider>().enabled = true;
        // mRole.mIsShield = false;
    }


    // public void KeyBoardInput()
    // {
    //     if (Input.GetKeyDown(KeyCode.W))
    //     {
    //         //Debug.Log("w is down");
    //         MoveSet(Direction.Up);
    //         mRole.SetMazeActive();

    //     }
    //     else if (Input.GetKeyDown(KeyCode.S))
    //     {
    //         //Debug.Log("S is down");
    //         MoveSet(Direction.Down);
    //         mRole.SetMazeActive();
    //     }
    //     else if (Input.GetKeyDown(KeyCode.A))
    //     {
    //         //Debug.Log("A is down");
    //         MoveSet(Direction.Left);
    //         mRole.SetMazeActive();
    //     }
    //     else if (Input.GetKeyDown(KeyCode.D))
    //     {
    //         //Debug.Log("D is down");
    //         MoveSet(Direction.Right);
    //         mRole.SetMazeActive();
    //     }
    //     else if (Input.GetKeyDown(KeyCode.Space))
    //     {
    //         //Debug.Log("Space is down");
    //         mRole.SetMazeActive();
    //         var c = mRole.CanAtkChar();
    //         AtkSet(c);
    //         // if(c!=null) {
    //         // 	AtkSet(c);
    //         // }else{
    //         // 	IdleSet();
    //         // }

    //     }
    //     else if (Input.GetKeyDown(KeyCode.F))
    //     {
    //         //Debug.Log("F is down");
    //     }


    //     if (Input.GetKeyDown(KeyCode.H))
    //     {
    //         mRole.SetMazeActive();
    //         //Debug.Log("H is down");
    //         mAnimator.SetBool("Defence", true);
    //         mRole.mIsShield = true;
    //         mRole.SetSpeed(3f);

    //         if (mRole.CharState() == Char.State.Move)
    //         {
    //             IdleSet();
    //         }

    //     }
    //     else if (Input.GetKeyUp(KeyCode.H))
    //     {
    //         mRole.SetMazeActive();
    //         //Debug.Log("H is up");
    //         mAnimator.SetBool("Defence", false);
    //         mRole.mIsShield = false;
    //         mRole.SetSpeed(5.5f);
    //     }
    // }

    Vector3 mMoveDownPos;
    bool mTrackingPos;
    float mMouseDownTime;

    public void MouseInput()
    {

        //Debug.LogFormat("touch input ");
        // if(Input.touchCount <=0 ) {
        // 	return;
        // }
        // var touch = Input.GetTouch(0);
        // Debug.LogFormat("touch phase {0} {1} {2}", touch.phase, touch.position,touch.pressure);
        if (Input.GetMouseButtonDown(0))
        {
            mTrackingPos = true;
            mMouseDownTime = 0f;
            mRole.SetMazeActive();
            mMoveDownPos = Input.mousePosition;
            Debug.LogFormat("MoveDown pos {0}", mMoveDownPos);

            mAnimator.SetBool("Defence", true);
            // mRole.mIsShield = true;

            if (mRole.mState.mState == CharState.State.Move)
            {
                IdleSet();
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            mTrackingPos = false;
            // var c = mRole.CanAtkChar();
            // AtkSet(c);

            Debug.LogFormat("MoveUp pos {0}", Input.mousePosition);
            var c = mRole.mState.CanAtkChar();
            if (c != null)
            {
                AtkSet(c);
            }
            else if ((Input.mousePosition - mMoveDownPos).magnitude < 2 && mMouseDownTime < 0.2f)
            {
                AtkSet(c);
            }

            mAnimator.SetBool("Defence", false);
            // mRole.mIsShield = false;

        }
        else if (mTrackingPos)
        {
            mMouseDownTime += Time.deltaTime;
            var curMoveDownPos = Input.mousePosition;

            var magnitude = (Input.mousePosition - mMoveDownPos).magnitude;
            if (3 < magnitude && magnitude < 150)
            {
                var absX = Mathf.Abs(Input.mousePosition.x - mMoveDownPos.x);
                var absY = Mathf.Abs(Input.mousePosition.y - mMoveDownPos.y);
                if (absX > absY)
                {
                    if (Input.mousePosition.x > mMoveDownPos.x)
                    {
                        mRole.DirectionSet(Direction.Right);
                    }
                    else
                    {
                        mRole.DirectionSet(Direction.Left);
                    }
                }
                else
                {
                    if (Input.mousePosition.y > mMoveDownPos.y)
                    {
                        mRole.DirectionSet(Direction.Up);
                    }
                    else
                    {
                        mRole.DirectionSet(Direction.Down);
                    }
                }

                //mMoveDownPos = Input.mousePosition;
                Debug.LogFormat("MoveChange pos {0}", mMoveDownPos);
            }
            else if (magnitude > 150)
            {
                var absX = Mathf.Abs(Input.mousePosition.x - mMoveDownPos.x);
                var absY = Mathf.Abs(Input.mousePosition.y - mMoveDownPos.y);
                if (absX > absY)
                {
                    if (Input.mousePosition.x > mMoveDownPos.x)
                    {
                        MoveSet(Direction.Right);
                    }
                    else
                    {
                        MoveSet(Direction.Left);
                    }
                }
                else
                {
                    if (Input.mousePosition.y > mMoveDownPos.y)
                    {
                        MoveSet(Direction.Up);
                    }
                    else
                    {
                        MoveSet(Direction.Down);
                    }
                }
                //mMoveDownPos = Input.mousePosition;
                Debug.LogFormat("MoveChange pos {0}", mMoveDownPos);
            }
        }


    }

    // public bool IsShield()
    // {
    //     return mRole.mIsShield;
    // }


    public override void AtkBegin()
    {
        base.AtkBegin();
        //Debug.LogFormat("{0} role overide atk begin", mChar.mCls);
        //mSword.mIsAtking = true;
    }

    public override void AtkEnd()
    {
        base.AtkEnd();
        //Debug.LogFormat("{0} atk overide end", mChar.mCls);
        //mSword.mIsAtking = false;
    }


    public void HpReset()
    {
        mRole.mFight.HpReset();
        HpUIUpdate();
        mRole.SaveRoleData();
    }


    public void ExpAdd(int exp)
    {
        mRole.ExpAdd(exp);
        ShowUpgrade();
        UIBattleUpdate();
    }

    public void CoinAdd(int cnt)
    {
        mRole.CoinAdd(cnt);
        UIBattleUpdate();
    }


    public void LevelReset()
    {
        Debug.LogFormat("LevelReset");
        mRole.LevelReset();
        UIBattleUpdate();
    }


    public void HpBarSetActive(bool active)
    {
        if (mHPBarUI != null)
        {
            mHPBarUI.SetActive(active);
        }
    }

    public void UIBattleUpdate()
    {
        var lv = mRole.Level();
        var expRate = mRole.LevelExpRate();
        Debug.LogFormat("role level {0} expRate {1}", lv, expRate);
        mBattleUI.SetLevelBar(lv, expRate);
        mBattleUI.ShowCoin(mRole);
    }


    public void ShowUpgrade()
    {
        if (mRole.LevelUpPending() > 0)
        {
            mRole.LevelUpPendingClear();
            mUpgradeUI.Show();
        }
    }

    public override void OnTriggerEnter(Collider coll)
    {
        switch (coll.tag)
        {
            case "Shield":
                return;
            case "Sword":
                return;
        }

        base.OnTriggerEnter(coll);
    }


    // 升级能力
    public void AbilityUpgrade(int id)
    {
        var lv = mRole.AbililtyUpgrade(id);
        switch (id)
        {
            case HeroAbility.TypeHp:
                mHPBarUI.SetCurHp(mRole.mFight.Hp, mRole.mFight.MaxHp);
                break;
            case HeroAbility.TypeAtk:
                break;
            case HeroAbility.TypeDef:
                break;
            case HeroAbility.TypeExp:
                break;
            case HeroAbility.TypeCoin:
                break;
        }
    }


    public string InspectInfo()
    {
        var s = "";
        if (mRole.mFight != null)
        {
            s += string.Format("Hp {0}\n", mRole.mFight.Hp);
            s += string.Format("MaxHp {0}\n", mRole.mFight.MaxHp);
            s += string.Format("Atk {0}\n", mRole.mFight.Atk);
            s += string.Format("Def {0}\n", mRole.mFight.Def);
            s += string.Format("Crit {0}\n", mRole.mFight.Crit);
            s += string.Format("-----\n");
        }


        if (mRole.mState != null)
        {
            s += string.Format("MoveSpeed {0}\n", mRole.mState.mMoveSpeed);
            s += string.Format("AtkSpeed {0}\n", "x");
            s += string.Format("Idx {0}\n", mRole.mState.mCurIdx);
        }


        return s;


    }

}


