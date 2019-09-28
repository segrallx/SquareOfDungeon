using UnityEngine;

public class RemoteMonsterState : MonsterState
{
    public RemoteMonsterState(string cls, Vector3 position, float moveSpeed) : base(cls, position, moveSpeed)
    {
    }

    // 近距离攻击和远程攻击的达成条件不同
    // 远程攻击：怪物和玩家在一条直线，并且中间没有遮挡则进入攻击状态.
    protected override bool CheckCanAtk()
    {
        var destCharIdx = mChaseChar.mState.mCurIdx;
        var selfIdx = this.mCurIdx;

        if (destCharIdx.x == selfIdx.x)
        {
            var x = destCharIdx.y - selfIdx.y;
            var step = 1;
            if (x < 0)
            {
                step = -1;
            }
            else if (x > 0)
            {
                step = 1;
            }
            else
            {
                return false;
            }

            for (var i = selfIdx.y + step; i != destCharIdx.y; i += step)
            {
                if (!mMaze.CheckFlyAble(destCharIdx.x, i, this.mChar))
                {
                    return false;
                }
                //Debug.LogFormat("can atk ok {0}:{1}", destCharIdx.x, i);
            }
            //Debug.LogFormat("can atk from {0} to {1}", selfIdx, destCharIdx);

            return true;
        }
        else if (destCharIdx.y == selfIdx.y)
        {
            var x = destCharIdx.x - selfIdx.x;
            var step = 1;
            if (x < 0)
            {
                step = -1;
            }
            else if (x > 0)
            {
                step = 1;
            }
            else
            {
                return false;
            }

            for (var i = selfIdx.x + step; i != destCharIdx.x; i += step)
            {
                if (!mMaze.CheckFlyAble(i, destCharIdx.y, this.mChar))
                {
                    return false;
                }
                Debug.LogFormat("can atk ok {0}:{1}", i, destCharIdx.y);
            }

            Debug.LogFormat("can atk from {0} to {1}", selfIdx, destCharIdx);
            return true;
        }
        else
        {
            return false;
        }

        //return false;
    }


}
