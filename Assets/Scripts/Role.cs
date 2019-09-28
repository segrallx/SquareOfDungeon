using UnityEngine;
using System;


[System.Serializable]
public class Role : Char
{
    private HeroExp mExp;
    private HeroAbility mAbility;
    private Char mAtkChar;
    public float mCoin;  //持有金币数量

    public Role(Vector3 position) : base(position, "role ")
    {
        LoadRoleData();
        mState = new RoleState(mCls, position, 5.5f);
        mExp = new HeroExp();
        mAbility = new HeroAbility();
    }

    // 初始化战斗数值
    public void InitFight()
    {
        var cfg = CsvAgent.Instance();
        mAbility.Reset();
        mFight.Reset();

        mFight.Hp = cfg.GetHeroAbilityByIdAndLevel(HeroAbility.TypeHp,
                                                   mAbility.Level(HeroAbility.TypeHp)).Value;
        mFight.MaxHp = mFight.Hp;
        mFight.Atk = cfg.GetHeroAbilityByIdAndLevel(HeroAbility.TypeAtk,
                                                    mAbility.Level(HeroAbility.TypeAtk)).Value;
        mFight.Def = cfg.GetHeroAbilityByIdAndLevel(HeroAbility.TypeDef,
                                                    mAbility.Level(HeroAbility.TypeDef)).Value;


        var moveSpeed = cfg.GetHeroAbilityByIdAndLevel(HeroAbility.TypeMoveSpeed,
                                                       mAbility.Level(HeroAbility.TypeMoveSpeed)).Value / 10f;
        mState.mMoveSpeed = moveSpeed;
    }

    public void ClearFight()
    {
    }

    public void Reset(Vector3 position)
    {
        mState.Reset();
        mState.mMovePos = position;
    }



    public override bool IsRole()
    {
        return true;
    }

    public override string CharType()
    {
        return "role";
    }

    public void SetMazeActive()
    {
        mMaze.mActive = true;
    }

    private void LoadRoleData()
    {
        // mMaxHP = PlayerPrefs.GetInt("HeartMax", 1);
        // mHP = PlayerPrefs.GetInt("HeartCur", 1);
        // mCoin = PlayerPrefs.GetInt("Coins", 0);
    }


    public void SaveRoleData()
    {
        // PlayerPrefs.SetInt("HeartMax", mMaxHP);
        // PlayerPrefs.SetInt("HeartCur", mHP);
        // PlayerPrefs.SetInt("Coins", mCoin);
    }

    public override bool BeAttacked(Char c)
    {
        var ret = base.BeAttacked(c);
        SaveRoleData();
        return ret;
    }

    public int ExpAdd(int exp)
    {
        exp = (int)(mAbility.expRate * (float)exp);
        mExp.AddExp(exp);
        Debug.LogFormat("role add exp {0}", exp);
        return mExp.PendingUpLv();
    }

    public void CoinAdd(int cnt)
    {
        mCoin += (mAbility.coinRate * (float)cnt);
    }

    public int AbililtyUpgrade(int id)
    {
        var cfg = CsvAgent.Instance();
        var lv = mAbility.Upgrade(id);
        switch (id)
        {
            case HeroAbility.TypeHp:
                var csvHp = cfg.GetHeroAbilityByIdAndLevel(HeroAbility.TypeHp, lv);
                var addHp = csvHp.Value - mFight.MaxHp;
                mFight.MaxHp = csvHp.Value;
                mFight.Hp += addHp;
                Debug.LogFormat("upgrade up curhp {0} maxhp{1} csvvalue{2}",
                                mFight.Hp, mFight.MaxHp, csvHp.Value);
                break;
            case HeroAbility.TypeAtk:
                var csvAtk = cfg.GetHeroAbilityByIdAndLevel(HeroAbility.TypeAtk, lv);
                mFight.Atk = csvAtk.Value;
                break;
            case HeroAbility.TypeDef:
                var csvDef = cfg.GetHeroAbilityByIdAndLevel(HeroAbility.TypeDef, lv);
                mFight.Def = csvDef.Value;
                break;
            case HeroAbility.TypeExp:
                var csvExp = cfg.GetHeroAbilityByIdAndLevel(HeroAbility.TypeExp, lv);
                mAbility.expRate = (float)csvExp.Value / 100.0f;
                break;
            case HeroAbility.TypeCoin:
                var csvCoin = cfg.GetHeroAbilityByIdAndLevel(HeroAbility.TypeCoin, lv);
                mAbility.coinRate = (float)csvCoin.Value / 100.0f;
                break;
            case HeroAbility.TypeCrit:
                var csvCrit = cfg.GetHeroAbilityByIdAndLevel(HeroAbility.TypeCoin, lv);
                mFight.Crit = (float)csvCrit.Value / 100.0f;
                break;
            case HeroAbility.TypeMoveSpeed:
                var csvMoveApeed = cfg.GetHeroAbilityByIdAndLevel(HeroAbility.TypeMoveSpeed, lv);
                mState.mMoveSpeed = csvMoveApeed.Value / 10f;
                break;
        }
        return lv;
    }

    public int LevelUpPending()
    {
        return mExp.PendingUpLv();
    }


    public void LevelUpPendingClear()
    {
        mExp.PendingUpLvClear();
    }


    public int Level()
    {
        return mExp.Level();
    }


    public void LevelReset()
    {
        mExp.Reset();
    }


    public float LevelExpRate()
    {
        return mExp.LevelExpRate();
    }


}

