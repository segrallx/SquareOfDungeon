using System;

public class HeroExp
{
    private int mUpLevelPending = 0;  //待选择升级的等级数
    private int mLevel = 0;
    private int mExp = 0;
    private int mLevelExp = 0; //升级所需经验

    // 增加经验，返回上升等级.
    public void AddExp(int exp)
    {
        var cfg = CsvAgent.Instance().GetHeroExpByLevel(mLevel);
        mLevelExp = cfg.Exp;
        mExp += exp;
        var mOldLevel = mLevel;
        while (mExp > cfg.Exp)
        {
            mLevel += 1;
            mExp -= cfg.Exp;
            cfg = CsvAgent.Instance().GetHeroExpByLevel(mLevel);
        }

        mUpLevelPending += (mLevel - mOldLevel);
    }

    // 当前升级经验进度
    public float LevelExpRate()
    {
        if (mLevelExp == 0)
        {
            var cfg = CsvAgent.Instance().GetHeroExpByLevel(mLevel);
            mLevelExp = cfg.Exp;
        }

        return (float)mExp / (float)mLevelExp * 100;
    }

    public int Level()
    {
        return mLevel + 1;
    }


    public int PendingUpLv()
    {
        return mUpLevelPending;
    }

    public void PendingUpLvClear()
    {
        mUpLevelPending = 0;
    }

    public void Reset()
    {
        mUpLevelPending = 0;
        mLevelExp = 0;
        mLevel = 0;
        mExp = 0;
    }



}
