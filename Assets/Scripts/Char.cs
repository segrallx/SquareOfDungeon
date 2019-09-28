/*
  负责地图上的任意生物的状态逻辑.
 */

using UnityEngine;

public class Char
{
    public string mCls;
    public int mId;
    protected Maze mMaze;

    // 战斗数值
    internal Fight mFight;

    internal CharRender mCharRender;

    // 基础状态
    private CharState _mState;
    public CharState mState
    {
        get { return _mState; }
        set { _mState = value; }
    }

    // 标识.
    static private int mIdx = 1;
    private Vector3 _pos;


    public Char(Vector3 position, string cls)
    {
        mId = mIdx;
        mIdx += 1;
        _pos = position;

        mFight = new Fight();
        mCls = cls;
    }

    public void MazeSet(Maze maze)
    {
        mMaze = maze;
        mState.mMaze = mMaze;
        mState.mChar = this;
    }

    // xx
    public Quaternion MoveRotationY()
    {
        switch (mState.DirectionGet())
        {
            case Direction.Up:
                return Quaternion.Euler(0, 0, 0);
            case Direction.Down:
                return Quaternion.Euler(0, 180, 0);
            case Direction.Left:
                return Quaternion.Euler(0, 270, 0);
            case Direction.Right:
                return Quaternion.Euler(0, 90, 0);
        }
        return Quaternion.Euler(0, 0, 0);
    }

    // 获取当前Char的正向朝向
    public Vector3 DirectionForward()
    {
        return Unit.DirectionForward(mState.DirectionGet());
    }

    public virtual bool FixedUpdate()
    {

        if (mMaze == null)
        {
            return false;
        }

        if (!mMaze.mActive)
        {
            return false;
        }

        return mState.FixedUpdate();

    }

    public virtual bool IsRole()
    {
        return false;
    }


    public virtual bool IsMonster()
    {
        return false;
    }


    public virtual string CharType()
    {
        return "char_type_no";
    }


    // 返回当前朝向.
    public Direction DirectionGet()
    {
        return mState.DirectionGet();
    }

    // 设置朝向
    public void DirectionSet(Direction dir)
    {
        mState.DirectionSet(dir);
    }

    public void Dead()
    {
        //mMaze.SetPointUnOccupy(mCurIdx, this);
        mMaze.CharDead(this);
    }

    public bool IsDead()
    {
        return mFight.IsDead();
    }


    // op是不是面对我.
    public bool IsFaceToMe(Char op)
    {
        var dir = op.DirectionCheck(mState.mCurIdx);
        if (dir == op.DirectionGet())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public Direction DirectionCheck(Vector2Int cur)
    {
        return mState.DirectionCheck(cur);
    }

    public bool MazeActive()
    {
        if (mMaze == null)
        {
            return false;
        }
        return mMaze.mActive;
    }


    public virtual bool BeAttacked(Char c)
    {
        return mFight.BeAttacked(c.mFight);
    }

    public virtual bool BeTraped(Fight f)
    {
        return mFight.BeAttacked(f);
    }


    public virtual bool BeEffected(Char c)
    {
        return mFight.BeEffected(c.mFight);
    }


    // 杀死后的经验.
    public virtual int GetExp()
    {
        return 0;
    }

}
