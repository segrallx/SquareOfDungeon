using UnityEngine;

[System.Serializable]
public class Tiled
{

    // 类型枚举
    [System.Serializable]
    public enum Cls
    {
        Floor = 1,
        Block = 2,
        Door = 3,
        Out = 4,
        Start = 5,
        Trap = 6,      //可以移入
        Workbench = 7, //工作台
        End = 8,
        Defence = 9,  //不可以移入机关台
        Water = 10,  // 流动水
        FracBox = 11,  // 封印，需要打算所有的封印才能进入下一层
                       //Defence = 9,  //不可以移入机关台
    }

    public Cls mCls;
    public int mId;
    public bool Used;

    // 高度是否为0.
    public bool Stable;

    //private bool mOccupied;
    private Char mOccupiedChar;

    public GameObject mObj;

    public Vector2Int mIdx;
    public Direction mFace;



    // 附加类型
    public ItemCls mItemCls;
    public MonsterCls mMonsterCls;
    public NpcCls mNpcCls;


    // public bool Occupied()
    // {
    //     return mOccupied;
    // }


    public Char OccupiedChar()
    {
        return mOccupiedChar;
    }

    public void SetOccupied(Char c)
    {
        mOccupiedChar = c;
        //mOccupied = true;
    }

    public Char ClearOccupied()
    {
        var c = mOccupiedChar;
        //mOccupied = false;
        mOccupiedChar = null;
        return c;
    }


    //迷宫生产过程中的标识符
    public int mGenx
    {
        get;
        set;
    }

    public int Cnt
    {
        get;
        set;
    }


    public int mX
    {
        get
        {
            return mIdx.x;
        }
    }

    public int mY
    {
        get
        {
            return mIdx.y;
        }
    }


    public Tiled(Vector2Int idx, Cls type, int id, int x)
    {
        mIdx = idx;
        mCls = type;
        mId = id;
        mGenx = x;
        mFace = Direction.None;
    }


    // IsDangrouse
    public bool IsDangerouse()
    {
        switch (mCls)
        {
            case Cls.Trap:
                return true;
        }
        return false;
    }


    public void SetUsed(Cls cls, int id)
    {
        mCls = cls;
        mGenx = 1;
        mId = id;
        Used = true;
    }


}
