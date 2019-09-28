using UnityEngine;
using System.Collections.Generic;

public class MeleeMonster : Monster
{
    // Use this for initialization
    public MeleeMonster(Vector3 pos, int monsterId) : base(pos, monsterId)
    {
        mState = new MeleeMonsterState(mCls, pos, mCsv.Speed);
    }

}


