using UnityEngine;

public class Unit
{
    public static readonly float mUnitSize = 2.0f;
    public static readonly int mFrame = 25;
    public static readonly int mMaxBlockId = 3;


    public static Tiled MakeRandomTiled(Tiled.Cls cls, int genX, Vector2Int idx)
    {
        int id = 1;
        switch (cls)
        {
            case Tiled.Cls.Block:
                id = UnityEngine.Random.Range(1, mMaxBlockId + 1);
                break;
        }
        var ret = new Tiled(idx, cls, id, genX);
        return ret;
    }

    // 返回8方向位置.
    public delegate void AdjacentTraverseFun(Vector2Int x);
    public static void AdjacentIdxList(Vector2Int mCurIdx, AdjacentTraverseFun f)
    {
        int[][] jaggedArray = new int[8][];
        jaggedArray[0] = new int[] { 0, 1 };
        jaggedArray[1] = new int[] { -1, 1 };
        jaggedArray[2] = new int[] { -1, -1 };
        jaggedArray[3] = new int[] { -1, 0 };
        jaggedArray[4] = new int[] { 1, 1 };
        jaggedArray[5] = new int[] { 1, -1 };
        jaggedArray[6] = new int[] { 1, 0 };
        jaggedArray[7] = new int[] { 0, -1 };
        int[] x;

        for (var i = 0; i < 8; i++)
        {
            x = jaggedArray[i];
            var pos1 = new Vector2Int(mCurIdx.x + x[0], mCurIdx.y + x[1]);
            f(pos1);
        }
    }

    // 寻找最近的整数点.
    public static Vector3 NearestCenter(Vector3 pos)
    {
        //(pos.x % mUnitSize)
        var originX = (int)(pos.x / mUnitSize) * mUnitSize;
        var originZ = (int)(pos.z / mUnitSize) * mUnitSize;

        int[][] jaggedArray = new int[7][];
        jaggedArray[0] = new int[] { 0, 0 };
        jaggedArray[1] = new int[] { -1, 1 };
        jaggedArray[2] = new int[] { -1, -1 };
        jaggedArray[3] = new int[] { -1, 0 };
        jaggedArray[4] = new int[] { 1, 1 };
        jaggedArray[5] = new int[] { 1, -1 };
        jaggedArray[6] = new int[] { 1, 0 };

        var matchIdx = 0;
        var matchDistance = 100f;
        int[] x;

        for (var i = 0; i < 7; i++)
        {
            x = jaggedArray[i];
            var pos1 = new Vector3(originX + x[0] * mUnitSize, pos.y, originZ + x[1] * mUnitSize);
            var distance = (pos1 - pos).magnitude;
            //Debug.Log("Idx:" + i + "Pos" + pos1 + "Dis:" + distance);
            if (distance < matchDistance)
            {
                matchIdx = i;
                matchDistance = distance;
            }
        }

        //Debug.Log("match Idx:" + matchIdx+ " originX:"+originX + " originZ:" + originZ);
        x = jaggedArray[matchIdx];
        return new Vector3(originX += x[0] * mUnitSize, pos.y, originZ += x[1] * mUnitSize);
    }

    // 寻找最近的整数点.
    public static Vector3 TileCenter(Vector3 pos)
    {
        var idx = TileIdx(pos);
        return new Vector3(idx.x * mUnitSize, pos.y, idx.y * mUnitSize);
    }

    public static Vector3 TileIdxPos(Vector2Int pos, float y)
    {
        return new Vector3(pos.x * mUnitSize, y, pos.y * mUnitSize);
    }


    // 计算出当前点所属的格子，当处于边界时，向下取整.
    public static Vector2Int TileIdx(Vector3 pos)
    {
        int unitX = ((int)pos.x / (int)mUnitSize) * (int)mUnitSize;
        var leftX = pos.x - unitX;
        if (leftX > 1)
        {
            unitX += (int)mUnitSize;
        }// else if (leftX <-1) {
         // 	unitX+=(int)mUnitSize;
         // }

        int unitZ = ((int)pos.z / (int)mUnitSize) * (int)mUnitSize;
        var leftZ = pos.z - unitZ;
        if (leftZ > 1)
        {
            unitZ += (int)mUnitSize;
        }// else if (leftZ<-1){
         // 	unitZ+=(int)mUnitSize;
         // }

        return new Vector2Int(unitX / 2, unitZ / 2);
    }

    public static Vector2Int NextTiledIdx(Vector3 pos, Vector3 dir)
    {
        // var unitX = (int)(pos.x/2) + (int)dir.x;
        // var unitZ = (int)(pos.z/2) + (int)dir.z;
        var idx = TileIdx(pos);
        return new Vector2Int(idx.x + (int)dir.x, idx.y + (int)dir.z);
    }

    public static Vector2Int NextTiledIdx(Vector2Int idx, Direction d)
    {
        // var unitX = (int)(pos.x/2) + (int)dir.x;
        // var unitZ = (int)(pos.z/2) + (int)dir.z;
        var dir = MoveVector3(d);
        return new Vector2Int(idx.x + (int)dir.x, idx.y + (int)dir.z);
    }


    public static Vector3 MoveVector3(Direction mDirection)
    {
        switch (mDirection)
        {
            case Direction.Up:
                return new Vector3(1, 0, 0);
            case Direction.Down:
                return new Vector3(-1, 0, 0);
            case Direction.Left:
                return new Vector3(0, 0, 1);
            case Direction.Right:
                return new Vector3(0, 0, -1);
        }
        return Vector3.zero;
    }

    public static Direction[] Direction4 = new Direction[] { Direction.Up, Direction.Down,
        Direction.Left, Direction.Right };


    public static float DirectionRotation(Direction dir)
    {
        switch (dir)
        {
            case Direction.Up:
                return 0;
            case Direction.Left:
                return 270;
            case Direction.Down:
                return 180;
            case Direction.Right:
                return 90;
        }
        return 0;
    }

    // 根据方向计算正向向量
    public static Vector3 DirectionForward(Direction dir)
    {
        switch (dir)
        {
            case Direction.Up:
                return new Vector3(1, 0, 1);
            case Direction.Left:
                return new Vector3(0, 0, 1);
            case Direction.Down:
                return new Vector3(-1, 0, 0);
            case Direction.Right:
                return new Vector3(0, 0, -1);
        }
        return new Vector3(0, 0, 1);
    }

    // 计算周围随机的一个点
    public static Vector3 RandomAround(Vector3 pos, float range)
    {
        var x = Random.Range(-range, range);
        return new Vector3(pos.x + x, pos.y + x, pos.z + x);
    }




}

