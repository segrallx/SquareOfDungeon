using UnityEngine;
using System.Collections.Generic;

public class CharState
{
    public enum State
    {
        Idle = 0,
        Move,  // 移动
        Chase,  // 追寻.
        Atk,  // 追寻.
        Dizzy,  // 眩晕
        Pause,  // 眩晕
    };


    public State mState
    {
        get;
        set;
    }

    protected State mPreState = 0;
    private Direction mDirection = Direction.Up;
    private Vector3 _mMovePos;

    public Vector3 mMovePos
    {
        get { return _mMovePos; }
        set
        {
            Debug.LogFormat("{0} set pos {1}", mCls, value);
            _mMovePos = value;
            //mCurIdx = Unit.TileIdx(_mMovePos);
        }
    }
    //protected Maze mMaze;

    protected Char mChaseChar;
    protected List<Vector2Int> mChasePath;
    internal int mChaseLeaveDistance = 100;

    private float mDizzyTime;
    private float mPauseTime;

    protected int mIdxChxCnt = 0;
    internal int mDistanceFindRole = 2;

    public bool mIsAtking;
    private Vector2Int _mCurIdx = Vector2Int.zero;
    public Vector2Int mCurIdx
    {
        get { return _mCurIdx; }
        set
        {
            Debug.LogFormat("{0} set idx {1}", mCls, value);
            _mCurIdx = value;
        }
    }

    protected string mCls;
    internal Char mChar;
    internal Maze mMaze;


    // 返回当前朝向.
    public Direction DirectionGet()
    {
        return mDirection;
    }


    // 设置朝向
    public void DirectionSet(Direction dir)
    {
        mDirection = dir;

        Vector2Int x;
        if (mCurIdx == Vector2Int.zero)
        {
            x = Unit.TileIdx(mMovePos);
        }
        else
        {
            x = mCurIdx;
        }

        switch (dir)
        {
            case Direction.Up:
            case Direction.Down:
                _mMovePos.z = x.y * Unit.mUnitSize;
                break;
            case Direction.Left:
            case Direction.Right:
                _mMovePos.x = x.x * Unit.mUnitSize;
                break;
        }
    }

    // public void MovePosSet(Vector3 pos)
    // {
    //     Debug.LogFormat("{0} set pos {1}", mCls, pos);
    //     mMovePos = pos;
    // }


    public void MClsSet(string cls)
    {
        mCls = cls;
    }


    float _mMoveSpeed = 0.2f;

    public float mMoveSpeed
    {
        get
        {
            return _mMoveSpeed;
        }
        set
        {
            _mMoveSpeed = value;
        }
    }

    public CharState(string cls, Vector3 position, float moveSpeed)
    {
        mState = State.Idle;
        mCls = cls;
        mMovePos = position;
        mMoveSpeed = moveSpeed;
    }

    public State GetState()
    {
        return mState;
    }


    public virtual bool FixedUpdate()
    {
        UpdateIdx();
        bool stateChange = false;
        switch (mState)
        {
            case State.Idle:
                stateChange = StateIdleFixedUpdate();
                break;
            case State.Move:
                stateChange = StateMoveFixedUpdate();
                break;
            case State.Chase:
                stateChange = StateChaseFixedUpdate();
                break;
            case State.Atk:
                stateChange = StateAtkFixedUpdate();
                break;
            case State.Dizzy:
                stateChange = StateDizzyFixedUpdate();
                break;
            case State.Pause:
                StatePauseFixedUpdate();
                //stateChange = StatePauseFixedUpdate();
                break;
        }
        return stateChange;
    }


    protected virtual bool StateIdleFixedUpdate()
    {
        return false;
    }



    protected virtual bool StateMoveFixedUpdate()
    {
        var moveVec = Unit.MoveVector3(mDirection);
        var nextIdx = Unit.NextTiledIdx(mMovePos, moveVec);
        var tile = mMaze.TiledIndex(nextIdx);
        //bool stateChange = false;
        if (!mMaze.CheckMoveAble(nextIdx.x, nextIdx.y, this))
        {
            //Debug.LogFormat(mCls + "move idle cur {0} next {1} tile.used {2}", mCurIdx, nextIdx, tile.Used);
            IdleSet();
            return true;
        }
        else
        {
            mMovePos = mMovePos + (mMoveSpeed * Time.deltaTime * moveVec);
            //mMaze.SetPointUsed(nextIdx, this);
        }

        return false;

    }

    protected virtual bool StateChaseFixedUpdate()
    {

        var distance = ChaseCharDistance();
        if (distance == 0 || distance > mChaseLeaveDistance)
        {
            mChaseChar = null;
            IdleSet();
            return true;
        }

        if (mChaseChar == null)
        {
            IdleSet();
            return true;
        }

        //检查是否可以攻击
        if (CheckCanAtk())
        {
            AtkSet(mChaseChar);
            return true;
        }

        var moveVec = Unit.MoveVector3(mDirection);
        var nextIdx = Unit.NextTiledIdx(mMovePos, moveVec);

        mMovePos = mMovePos + (mMoveSpeed * Time.deltaTime * moveVec);
        var tile = mMaze.TiledIndex(nextIdx);
        if (!mMaze.CheckMoveAble(nextIdx.x, nextIdx.y, this))
        {
            IdleSet();
            return true;
        }

        return false;
    }


    protected bool StateAtkFixedUpdate()
    {
        // 状态切换.
        if (mIsAtking)
        {
            return false;
        }

        if (mChaseChar == null)
        {
            IdleSet();
            return true;
        }

        var distance = ChaseCharDistance();
        if (distance > mChaseLeaveDistance)
        {
            mChaseChar = null;
            IdleSet();
            return true;
        }
        else if (!CheckCanAtk())
        {
            ChaseSet(mChaseChar);
            return true;
        }


        // else if (distance > 1 && distance <= mDistanceFindRole)
        // {
        //     ChaseSet(mChaseChar);
        //     return true;
        // }else if(!CheckCanAtk()) {
        // 	ChaseSet(mChaseChar);
        //     return true;
        // }

        return false;
    }

    protected bool StateDizzyFixedUpdate()
    {
        mDizzyTime -= Time.deltaTime;
        if (mDizzyTime <= 0)
        {
            IdleSet();
            return true;
        }
        return false;
    }


    protected bool StatePauseFixedUpdate()
    {
        mPauseTime -= Time.deltaTime;
        if (mPauseTime <= 0)
        {
            mState = mPreState;
            return true;
        }
        return false;
    }

    // 根据跟踪路径来不断的调整朝向
    protected void ChaseDirectionUpdate()
    {
        mMovePos = Unit.TileIdxPos(mCurIdx, mMovePos.y);
        var nextP = Vector2Int.zero;
        if (mChasePath.Count > 0)
        {
            nextP = mChasePath[0];
            if (nextP.x == mCurIdx.x)
            {
                if (nextP.y > mCurIdx.y)
                {
                    DirectionSet(Direction.Left);
                }
                else
                {
                    DirectionSet(Direction.Right);
                }

            }
            else if (nextP.y == mCurIdx.y)
            {
                if (nextP.x > mCurIdx.x)
                {
                    DirectionSet(Direction.Up);
                }
                else
                {
                    DirectionSet(Direction.Down);
                }
            }
            mChasePath.RemoveAt(0);
        }
        //Debug.LogFormat("{0} chase set direction {1} cur {2} next{3}", mCls, DirectionGet(), mCurIdx, nextP);
    }



    protected void ChaseSet(Char c)
    {
        if (mState == State.Chase)
        {
            return;
        }

        Debug.Log(mCls + "Chase Set Role");
        mState = State.Chase;
        mChaseChar = c;
        mChasePath = ChasePathFind(mChaseChar);
        ChaseDirectionUpdate();
    }


    internal void PauseSet(float t)
    {

        if (mState == State.Pause)
        {
            return;
        }

        Debug.Log(mCls + "Pause Set Role");

        mPreState = mState;
        mState = State.Pause;
        mPauseTime = t;
    }



    // 设置移动下一个位置为停止
    public void IdleSet()
    {
        mState = State.Idle;
        var moveVec = Unit.MoveVector3(mDirection);
        //var pos = Unit.TileCenter(mMovePos);
        var pos = Unit.TileIdxPos(mCurIdx, mMovePos.y);
        //var pos = Unit.TileIdxPos(Idx);
        Debug.Log(mCls + " IdleSet Pos " + mMovePos + "To :" + pos);
        mMovePos = pos;
    }


    // 设置移动
    public void MoveSet(Direction direct)
    {
        if (mState == State.Move && mDirection == direct)
        {
            return;
        }

        mState = State.Move;
        DirectionSet(direct);


        //mMovePos = Unit.TileCenter(mMovePos);
        Debug.Log(mCls + " MoveSet Pos" + mMovePos);
    }

    private Char mAtkChar;
    public void AtkSet(Char c)
    {
        // if (mState == State.Atk)
        // {
        //     return;
        // }

        Debug.Log(mCls + "AtkSet c");
        mState = State.Atk;
        mAtkChar = c;
        var pos = Unit.TileIdxPos(mCurIdx, mMovePos.y);
        mMovePos = pos;
        if (c != null)
        {
            DirectionSet(DirectionCheck(c.mState.mCurIdx));
        }
    }


    // 计算下自己和cur的朝向.
    public Direction DirectionCheck(Vector2Int cur)
    {
        if (mCurIdx.x == cur.x)
        {
            if (mCurIdx.y > cur.y)
            {
                return Direction.Right;
            }
            else
            {
                return Direction.Left;
            }
        }
        else
        {
            if (mCurIdx.x > cur.x)
            {
                return Direction.Down;
            }
            else
            {
                return Direction.Up;
            }
        }
    }




    // 设置移动
    public void DizzySet()
    {
        Debug.Log(mCls + "DizzySet");
        mState = State.Dizzy;
        var oldPos = mMovePos;
        var pos = Unit.TileIdxPos(mCurIdx, mMovePos.y);
        mMovePos = pos;
        mIsAtking = false;
        mDizzyTime = 2f;
    }


    // 返回和追逐目标之间的Manhattan距离
    protected int ChaseCharDistance()
    {
        if (mChaseChar == null)
        {
            return 0;
        }

        return Mathf.Abs(mChaseChar.mState.mCurIdx.x - mCurIdx.x) +
            Mathf.Abs(mChaseChar.mState.mCurIdx.y - mCurIdx.y);
    }

    // 近距离攻击和远程攻击的达成条件不同
    protected virtual bool CheckCanAtk()
    {
        var distance = ChaseCharDistance();
        if (mChasePath.Count == 0 && distance == 1)
        {
            return true;
        }
        return false;
    }


    // 寻找合适的目的地并前往
    // 默认为四周攻击.
    protected virtual List<Vector2Int> ChasePathFind(Char c)
    {
        return Astar.FindPath(mMaze, mCurIdx, mChaseChar.mState.mCurIdx, c);
    }


    // public void UpdateIdxRaw()
    // {
    //     var idx = Unit.TileIdx(mMovePos);
    //     mCurIdx = idx;
    // }



    // 更新Char当前的地块位置，如果无法移入，设置为Idle状态
    public bool UpdateIdx()
    {
        var idx = Unit.TileIdx(mMovePos);
        var tile = mMaze.mTiles[idx.x][idx.y];
        if (mCurIdx != idx)
        {
            Debug.LogFormat("{0} update idx {1} {2}", mCls, mCurIdx, idx);
            if (tile.OccupiedChar() == null)
            {
                var oldIdx = mCurIdx;
                mCurIdx = idx;
                mIdxChxCnt++;
                mMaze.SetPointUnOccupy(oldIdx, mChar);
                mMaze.SetPointOccupy(mCurIdx, mChar);
                IdxChange(oldIdx, idx);
            }
            else
            {
                IdleSet();
                return true;
            }
        }
        return false;

    }

    // 格子发生改变后执行
    protected virtual void IdxChange(Vector2Int oldIdx, Vector2Int newIdx)
    {
    }



    // 往后退一个位置.
    public void SetStepBack()
    {
        var moveVec = Unit.MoveVector3(mDirection);
        var idx = Unit.NextTiledIdx(mMovePos, -moveVec);
        var tile = mMaze.mTiles[idx.x][idx.y];
        //Debug.LogFormat("SetStepBack {0} {1} occupy {2}", tile.mCls, idx, tile.mOccupied);
        if (tile.mCls == Tiled.Cls.Floor && tile.OccupiedChar() == null)
        {
            mMovePos = Unit.TileIdxPos(idx, mMovePos.y);
        }
    }



    // 是否可以攻击Npc
    public Char CanAtkChar()
    {
        var mv = Unit.MoveVector3(mDirection);
        var faceX = mCurIdx.x + (int)mv.x;
        var faceY = mCurIdx.y + (int)mv.z;
        var tile = mMaze.mTiles[faceX][faceY];
        if (tile.OccupiedChar() != null && (tile.OccupiedChar().IsMonster()))
        {
            return tile.OccupiedChar();
        }

        int[][] jaggedArray = new int[4][];
        jaggedArray[0] = new int[] { 1, 0 };
        jaggedArray[1] = new int[] { -1, 0 };
        jaggedArray[2] = new int[] { 0, -1 };
        jaggedArray[3] = new int[] { 0, 1 };

        for (var i = 0; i < 4; i++)
        {
            var jag = jaggedArray[i];
            if (jag[0] == faceX && jag[1] == faceY)
            {
                continue;
            }

            if (!mMaze.CheckPosValid(mCurIdx.x + jag[0], mCurIdx.y + jag[1]))
            {
                continue;
            }

            tile = mMaze.mTiles[mCurIdx.x + jag[0]][mCurIdx.y + jag[1]];
            if (tile.OccupiedChar() != null && (tile.OccupiedChar().IsMonster()))
            {
                return tile.OccupiedChar();
            }

        }


        return null;
    }

    public virtual void Reset()
    {
        mState = State.Idle;
        //mMoveSpeed = 5.5f;
        mCurIdx = Vector2Int.zero;
    }


    public virtual bool Moveable(Tiled tile)
    {
        switch (tile.mCls)
        {
            case Tiled.Cls.Block:
                return false;
            case Tiled.Cls.Water:
                return false;
            case Tiled.Cls.Defence:
                return false;
            case Tiled.Cls.FracBox:
                return tile.mObj.GetComponent<FracBox>().IsBroken();
            case Tiled.Cls.Floor:
                if (tile.mItemCls != null)
                {
                    if (tile.mItemCls.mCls == ItemCls.Cls.Chest)
                    {
                        return false;
                    }
                }
                break;
            default:
                return true;
        }

        return true;
    }


    public Direction RandomDirection()
    {
        var s = UnityEngine.Random.Range(0, 4);
        return (Direction)s;
    }


}
