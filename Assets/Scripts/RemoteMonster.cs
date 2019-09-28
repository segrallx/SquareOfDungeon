using UnityEngine;
using System.Collections.Generic;

public class RemoteMonster : Monster
{
    // Use this for initialization
    public RemoteMonster(Vector3 pos, int monsterId) : base(pos, monsterId)
    {
        mState = new RemoteMonsterState(mCls, pos, mCsv.Speed);
    }



}


