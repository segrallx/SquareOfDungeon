using UnityEngine;
using System.Collections.Generic;

// obsolete!!
public class MonsterChar : Char
{
    public MonsterChar(Vector3 pos) : base(pos, "")
    {
        //mMoveSpeed = 2.2f;
        //mCls = string.Format("slime {0} ", id);
        //id += 1;

        // mDistanceFindRole = 3;
        // mDistanceLeaveRole = 5;
    }
}

// public class MonsterChar : Char
// {
//     static int id = 1;
//     // Use this for initialization

//     public MonsterChar(Vector3 pos) : base(pos,1,1, "")
//     {
//         mMoveSpeed = 2.2f;
//         mCls = string.Format("slime {0} ", id);
//         id += 1;

//         mDistanceFindRole = 3;
//         mDistanceLeaveRole = 5;
//     }

//     public bool mRandomMove
//     {
//         get;
//         set;
//     }

//     private float mIdleSecs = 3.0f;

// 	// 重载，1.增加主动寻找Role逻辑 2.增加随机移动逻辑
//     protected override bool StateIdleFixedUpdate()
//     {
//         bool stateChange = false;
//         mIdleSecs -= Time.deltaTime;

//         if (mIdleSecs < 0 && mRandomMove)
//         {
//             mIdleSecs = 3f;
//             var s = RandomDirection();
//             var moveVecTmp = Unit.MoveVector3(s);
//             var curIdx = Unit.TileIdx(mMovePos);
//             var nextIdx = Unit.NextTiledIdx(mMovePos, moveVecTmp);
//             var tile = mMaze.TiledIndex(nextIdx);
//             if (mMaze.CheckMoveAble(nextIdx.x, nextIdx.y, this))
//             {
//                 //Debug.LogFormat(mCls + "idle move true  {0}, {1}, {2}", curIdx, nextIdx, s);
//                 //tile.Used = true;
//                 //mMaze.SetPointUsed(nextIdx, this);
//                 MoveSet(s);
//                 return true;
//             }
//             else
//             {
//                 //Debug.Log(mCls + "idle move failed");
//             }
//         }

//         var role = FindRole();
//         if (role != null)
//         {
//             ChaseSet(role);
//             return true;
//         }

//         return stateChange;
//     }

// 	// 重载，增加主动寻找Role逻辑
//     protected override bool StateMoveFixedUpdate()
//     {
// 		if(base.StateIdleFixedUpdate()) {
// 			return true;
// 		}

//         var role = FindRole();
//         if (role != null)
//         {
//             ChaseSet(role);
//             return true;
//         }


//         return false;
//     }

//     // 查找玩家.
//     private Role FindRole()
//     {
//         var size = mDistanceFindRole;
//         for (var i = mCurIdx.x - size; i < mCurIdx.x + size; i++)
//         {
//             for (var j = mCurIdx.y - size; j < mCurIdx.y + size; j++)
//             {
//                 if (mMaze.CheckPosValid(i, j))
//                 {
//                     var tile = mMaze.TiledIndex(i, j);
//                     if (tile.OccupiedChar() != null && tile.OccupiedChar().IsRole())
//                     {
//                         return (Role)tile.OccupiedChar();
//                     }
//                 }
//             }
//         }
//         return null;
//     }


//     public void IdxChangeStateChase(Vector2Int oldIdx, Vector2Int newIdx)
//     {
//         if (mIdxChxCnt % 3 != 0)
//         {
//             mChasePath = Astar.FindPath(mMaze, newIdx, mChaseChar.mCurIdx, this);
//         }

//         mMovePos = Unit.TileIdxPos(newIdx, mMovePos.y);
//         ChaseDirectionUpdate();
//     }


//     public override void IdxChange(Vector2Int oldIdx, Vector2Int newIdx)
//     {
//         // //Debug.LogFormat("{0} idx change {1} {2}", mCls, oldIdx, newIdx);
//         switch (mState)
//         {
//             case State.Chase:
//                 IdxChangeStateChase(oldIdx, newIdx);
//                 break;
//             default:
//                 return;
//         }
//     }


//     public override bool IsMonster()
//     {
//         return true;
//     }


//     public override bool Moveable(Tiled tile)
//     {
//         if (!base.Moveable(tile))
//         {
//             return false;
//         }

//         if (mState == Char.State.Chase)
//         {
//             //return true;
//         }

//         switch (tile.mCls)
//         {
//             case Tiled.Cls.Trap:
//                 return false;
//             default:
//                 return true;
//         }
//     }

// }


