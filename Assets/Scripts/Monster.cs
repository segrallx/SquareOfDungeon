using UnityEngine;

public class Monster : Char
{
    internal MonsterCsvLine mCsv;

    static int id = 1;
    // Use this for initialization
    public Monster(Vector3 pos, int monsterId) : base(pos, "")
    {
        id += 1;
        mCls = string.Format("slime {0} ", id);
        mCsv = CsvAgent.Instance().GetMonsterCsvByMonsterId(monsterId);
        if (mCsv != null)
        {
            mFight.CopyAttr(mCsv.GetFight());
        }
    }


    public override bool IsMonster()
    {
        return true;
    }


    public override string CharType()
    {
        return "monster";
    }



    public override int GetExp()
    {
        return mCsv.Exp;
    }

}


