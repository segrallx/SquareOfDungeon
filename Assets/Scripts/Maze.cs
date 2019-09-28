using System;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

//[System.Serializable]
public class Maze
{
    public TextAsset mtxtAsset;
    public int mSizeX, mSizeY;
    public Tiled[][] mTiles;
    public Vector2Int mStart;
    public bool mActive = false;

    public delegate void TileEvent(string eventName, Tiled til, Char c);
    private event TileEvent mTileEvent;
    public List<Char> mMonsters;
    public List<FracBox> mFracBox;

    private Role mRole;
    public void SetRole(Role role)
    {
        mRole = role;
    }

    public void TileEventRegister(TileEvent t)
    {
        mTileEvent += t;
    }

    public void TileEventUnRegister(TileEvent t)
    {
        mTileEvent -= t;
    }


    // 所有参数必须为奇数
    public Maze(int x, int y)
    {
        mSizeX = x;
        mSizeY = y;
        mTiles = new Tiled[mSizeX + 2][];
        for (var i = 0; i < mSizeX + 2; i++)
        {
            mTiles[i] = new Tiled[mSizeY + 2];
        }

        GenWallAround();
        mMonsters = new List<Char>();
        mFracBox = new List<FracBox>();
        //GenFloor();

        // GenBlockInit();
        // GenRoom(120);
        // GenRoad();
        // GenRoomRoadDoor();
        // GenLandthorn();
        // //GenCoins();
        // GenMonsters();

        // //RandomRoomGenSawtooth(2);
        // GenStartAndEnd();
        // AssciiShow();
    }

    // 所有参数必须为奇数
    public Maze(TextAsset txtAsset)
    {
        mtxtAsset = txtAsset;
        var json = txtAsset.text;
        FromJson(json);
        mMonsters = new List<Char>();
        mFracBox = new List<FracBox>();
    }

    public void GenRandomMaze(int roomSize, int roomExtraSize)
    {
        GenBlockInit();

        List<Room> mRooms;
        int mRoomSize;
        int mRoomExtraSize;
        //List<DeadEnd> mDeadEnds;

        mRoomSize = roomSize;
        mRoomExtraSize = roomExtraSize;
        mRooms = new List<Room>();
        GenRoom(mRooms, mRoomSize, mRoomExtraSize, 120);

        GenRoad();
        GenRoomRoadDoor(mRooms, 3);
        //GenLandthorn();
        //GenCoins();
        //GenMonsters();

        GenStartAndEnd();
    }

    public void GenFromCfg(MazeCfg cfg)
    {

        if (cfg.RoomGenTries > 0)
        {
            GenBlockInit();
            List<Room> mRooms = new List<Room>();
            GenRoom(mRooms, cfg.RoomSize, cfg.RoomExtraSize, cfg.RoomGenTries);
            GenRoad();
            GenRoomRoadDoor(mRooms, cfg.RoomDoorCnt);
        }
        else
        {
            GenBlockInit();
            GenRoad();
        }

        List<DeadEnd> deadEnds;
        List<Vector2Int> paths;
        SpPointList(out deadEnds, out paths);
        DecreasePath(paths);
        GenMonsters(cfg.Monsters);
        GenNpcs(cfg.Npcs);
        GenTiles(cfg.Tiles);
        //GenItems(cfg.Items);

        GenStartAndEnd();
    }



    public void SetPointOccupy(Vector2Int vec, Char c)
    {
        var tile = mTiles[vec.x][vec.y];

        if (tile.OccupiedChar() != null && tile.OccupiedChar() != c)
        {
            //Debug.LogErrorFormat("occupy error {0}:{1}  new:{2} old:{3}", vec.x, vec.y, c.mCls,  tile.mOccupiedChar.mCls);
            return;
        }

        tile.SetOccupied(c);
        var cls = "null";
        if (c != null)
        {
            cls = c.mCls;
        }
        //Debug.LogFormat("maze set idx {0}:{1} used char {2}", vec.x, vec.y, cls);
        if (mTileEvent != null)
        {
            mTileEvent("tile.occupy", tile, c);
        }
    }

    public void SetPointUnOccupy(Vector2Int vec, Char c1)
    {
        var tile = mTiles[vec.x][vec.y];
        if (tile.OccupiedChar() != null && tile.OccupiedChar() != c1)
        {
            Debug.LogErrorFormat("unoccupy error {0}:{1}  new:{2} old:{3}",
                                 vec.x, vec.y, c1.mCls, tile.OccupiedChar().mCls);
            //return;
        }

        var c = tile.ClearOccupied();
        var cls = "null";
        if (c != null)
        {
            cls = c.mCls;
        }
        //Debug.LogFormat("maze set idx {0}:{1} unused char {2}", vec.x, vec.y, cls);
        if (mTileEvent != null)
        {
            mTileEvent("tile.occupy", tile, c1);
        }
    }

    // 获取一个随机点.
    public Vector2Int RandomUnUsedPoint()
    {
        while (true)
        {
            var x = UnityEngine.Random.Range(1, mSizeX + 1);
            var y = UnityEngine.Random.Range(1, mSizeY + 1);
            var tile = mTiles[x][y];
            if (tile.mCls == Tiled.Cls.Floor && tile.Used == false)
            {
                return new Vector2Int(x, y);
            }
        }
    }

    // 在第几行随机获取一个点
    public Vector2Int RandomUnUsedPointOnX(int fromX, int fromY)
    {
        List<Tiled> list = new List<Tiled>();
        for (int i = fromX; i <= fromY; i++)
        {
            for (int j = 0; j < mSizeY; j++)
            {
                //Debug.LogFormat("x {0}  y {1}", i,j);
                var tile = mTiles[i][j];
                if (tile.mCls == Tiled.Cls.Floor && tile.Used == false)
                {
                    list.Add(tile);
                }
            }
        }
        Shuffle.X1(list);
        var p = list[0];
        p.Used = false;
        return p.mIdx;
    }

    // 寻找Maze中的死胡同.
    // mDeadEnds:死胡同，三面都是墙的Floor
    // mPaths:两面都是floor的Block
    public void SpPointList(out List<DeadEnd> mDeadEnds, out List<Vector2Int> mPaths)
    {
        // if (mDeadEnds != null)
        // {
        // 	return mDeadEnds;
        // }
        mDeadEnds = new List<DeadEnd>();
        mPaths = new List<Vector2Int>();

        for (var i = 1; i < mSizeX + 1; i++)
        {
            for (var j = 1; j < mSizeY + 1; j++)
            {
                int cnt = 0;
                var tile = mTiles[i][j];
                if (tile.mCls == Tiled.Cls.Floor)
                {
                    var tileUp = mTiles[i][j + 1];
                    var tileDown = mTiles[i][j - 1];
                    var tileLeft = mTiles[i + 1][j];
                    var tileRight = mTiles[i - 1][j];
                    int tileFloorX = 0;
                    int tileFloorY = 0;
                    if (tileUp.mCls == Tiled.Cls.Block)
                    {
                        cnt++;
                    }
                    else
                    {
                        tileFloorX = i;
                        tileFloorY = j + 1;
                    }
                    if (tileDown.mCls == Tiled.Cls.Block)
                    {
                        cnt++;
                    }
                    else
                    {
                        tileFloorX = i;
                        tileFloorY = j - 1;
                    }

                    if (tileLeft.mCls == Tiled.Cls.Block)
                    {
                        cnt++;
                    }
                    else
                    {
                        tileFloorX = i + 1;
                        tileFloorY = j;
                    }

                    if (tileRight.mCls == Tiled.Cls.Block)
                    {
                        cnt++;
                    }
                    else
                    {
                        tileFloorX = i - 1;
                        tileFloorY = j;
                    }

                    if (cnt == 3)
                    {
                        mDeadEnds.Add(new DeadEnd(i, j, tileFloorX, tileFloorY));
                    }
                }
                else if (tile.mCls == Tiled.Cls.Block)
                {
                    var tileUp = mTiles[i][j + 1];
                    var tileDown = mTiles[i][j - 1];
                    var tileLeft = mTiles[i + 1][j];
                    var tileRight = mTiles[i - 1][j];
                    int tileFloorX = 0;
                    int tileFloorY = 0;
                    if (tileUp.mCls == Tiled.Cls.Floor)
                    {
                        cnt++;
                    }
                    else
                    {
                        tileFloorX = i;
                        tileFloorY = j + 1;
                    }
                    if (tileDown.mCls == Tiled.Cls.Floor)
                    {
                        cnt++;
                    }
                    else
                    {
                        tileFloorX = i;
                        tileFloorY = j - 1;
                    }

                    if (tileLeft.mCls == Tiled.Cls.Floor)
                    {
                        cnt++;
                    }
                    else
                    {
                        tileFloorX = i + 1;
                        tileFloorY = j;
                    }

                    if (tileRight.mCls == Tiled.Cls.Floor)
                    {
                        cnt++;
                    }
                    else
                    {
                        tileFloorX = i - 1;
                        tileFloorY = j;
                    }

                    if (cnt == 2)
                    {
                        mPaths.Add(new Vector2Int(i, j));
                    }
                }



            }
        }

    }

    // // 获取房间中获取一个随机线
    // public Vector2Int RandomRoomGenSawtooth(int limit)
    // {
    //	while (true)
    //	{
    //		var mRoomIdx = UnityEngine.Random.Range(0, mRooms.Count);
    //		var room = mRooms[mRoomIdx];
    //		var roomXPadding = room.mRect.width - (limit*2+1);
    //		if (roomXPadding>=0){
    //			var randomY = UnityEngine.Random.Range(room.mRect.y, room.mRect.y + room.mRect.height+1);
    //			int xpad;
    //			if(roomXPadding==0) {
    //				xpad = 0;
    //			}else{
    //				xpad = UnityEngine.Random.Range(0, roomXPadding);
    //			}

    //			var match = true;
    //			for(var i=0; i<=(limit*2+1);i++) {
    //				var idx = room.mRect.x+xpad+i;
    //				var tiled = mTiles[i][randomY];
    //				if(tiled.mCls == Tiled.Cls.Door){
    //					match = false;
    //					break;
    //				}
    //			}

    //			if(match){
    //				var tiled = mTiles[room.mRect.x][randomY];
    //				tiled.SetCls(Tiled.Cls.Block);
    //				tiled.SetGenX(1);

    //				tiled = mTiles[room.mRect.x+ (limit*2+1)][randomY];
    //				tiled.SetCls(Tiled.Cls.Block);
    //				tiled.SetGenX(1);
    //			}
    //		}

    //		// var mRandomX = UnityEngine.Random.Range(room.mRect.x, room.mRect.x + room.mRect.width+1);
    //		// var mRandomY = UnityEngine.Random.Range(room.mRect.y, room.mRect.y + room.mRect.height+1);

    //	}
    // }


    // 检查一个点是否合法。
    public bool CheckPosValid(int rx1, int ry1)
    {
        if (rx1 >= 1 && rx1 < mSizeX + 1 && ry1 >= 1 && ry1 < mSizeY + 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CheckPosUsed(int x, int y)
    {
        return mTiles[x][y].Used;
    }




    // 检查一个点是否可以被移入.。
    public bool CheckMoveAble(int rx1, int ry1, CharState c)
    {
        var tile = mTiles[rx1][ry1];
        if (c.Moveable(tile) == false)
        {
            return false;
        }
        return tile.OccupiedChar() == null || tile.OccupiedChar().IsDead();
        //return true;
    }

    // 检查一个点是否飞过
    public bool CheckFlyAble(int rx1, int ry1, Char c)
    {
        var tile = mTiles[rx1][ry1];

        switch (tile.mCls)
        {
            case Tiled.Cls.Block:
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
        }


        return tile.OccupiedChar() == null;
        //return true;
    }



    // 迷宫初始化设置为01交叉.
    private void GenBlockInit()
    {
        for (var i = 1; i < mSizeX + 1; i++)
        {
            for (var j = 1; j < mSizeY + 1; j++)
            {
                if (i % 2 == 1)
                {
                    if (j % 2 == 1)
                    {
                        mTiles[i][j] = Unit.MakeRandomTiled(Tiled.Cls.Floor, 2, new Vector2Int(i, j));
                    }
                    else
                    {
                        mTiles[i][j] = Unit.MakeRandomTiled(Tiled.Cls.Block, 2, new Vector2Int(i, j));
                    }
                }
                else
                {
                    mTiles[i][j] = Unit.MakeRandomTiled(Tiled.Cls.Block, 2, new Vector2Int(i, j));
                }
            }
        }
    }

    // 生成迷宫外网的墙壁.
    private void GenWallAround()
    {
        var Head = mTiles[0];
        var Bottom = mTiles[mSizeX + 1];
        for (var j = 1; j < mSizeY + 1; j++)
        {
            // //Debug.Log("GenWallAround j:"+j);
            Head[j] = Unit.MakeRandomTiled(Tiled.Cls.Block, 1, new Vector2Int(0, j));
            Bottom[j] = Unit.MakeRandomTiled(Tiled.Cls.Block, 1, new Vector2Int(mSizeX + 1, j));
        }

        for (var i = 1; i < mSizeX + 1; i++)
        {
            mTiles[i][0] = Unit.MakeRandomTiled(Tiled.Cls.Block, 1, new Vector2Int(i, 0));
            mTiles[i][mSizeY + 1] = Unit.MakeRandomTiled(Tiled.Cls.Block, 1, new Vector2Int(i, mSizeY + 1));
        }

        mTiles[0][0] = Unit.MakeRandomTiled(Tiled.Cls.Block, 1, new Vector2Int(0, 0));
        mTiles[0][mSizeY + 1] = Unit.MakeRandomTiled(Tiled.Cls.Block, 1, new Vector2Int(0, mSizeY + 1));
        mTiles[mSizeX + 1][0] = Unit.MakeRandomTiled(Tiled.Cls.Block, 1, new Vector2Int(mSizeX + 1, 0));
        mTiles[mSizeX + 1][mSizeY + 1] = Unit.MakeRandomTiled(Tiled.Cls.Block, 1,
                                                              new Vector2Int(mSizeX + 1, mSizeY + 1));
    }

    // 将迷宫内部用地板填充.
    public void GenFloor()
    {
        for (var i = 1; i < mSizeX + 1; i++)
        {
            for (var j = 1; j < mSizeY + 1; j++)
            {
                mTiles[i][j] = Unit.MakeRandomTiled(Tiled.Cls.Floor, 1, new Vector2Int(i, j));
            }
        }
    }

    private void AssciiShow(List<Room> mRooms)
    {
        var ret = "";
        //Debug.Log("size:" + mTiles.Length);
        for (var i = 0; i < mSizeX + 2; i++)
        {
            for (var j = 0; j < mSizeY + 2; j++)
            {
                ////Debug.Log("x:" + i + " y:" + j);
                var tile = mTiles[i][j];

                if (tile == null)
                {
                    ret += String.Format("Nx ");
                }
                else
                {
                    switch (tile.mCls)
                    {
                        case Tiled.Cls.Block:
                            ret += String.Format("B{0} ", tile.mGenx);
                            break;
                        case Tiled.Cls.Floor:
                            ret += String.Format("F{0} ", tile.mGenx);
                            break;
                        case Tiled.Cls.Door:
                            ret += String.Format("D{0} ", tile.mGenx);
                            break;
                        case Tiled.Cls.Start:
                            ret += String.Format("S{0} ", tile.mGenx);
                            break;
                        case Tiled.Cls.End:
                            ret += String.Format("E{0} ", tile.mGenx);
                            break;
                    }
                }
            }
            ret += "\n";
        }

        // //Debug.Log("room:" + mRooms.Count);
        foreach (var room in mRooms)
        {
            ret += room.Info();
        }

        // //Debug.Log("\n" + ret);
    }

    // 生成房间以外的联通迷宫区域
    private void GenRoad()
    {
        //return;
        int startX;
        int startY;

        while (true)
        {
            int x = UnityEngine.Random.Range(1, mSizeX);
            int y = UnityEngine.Random.Range(1, mSizeY);
            var tile = mTiles[x][y];
            if (tile.mCls == Tiled.Cls.Floor && tile.mGenx == 2)
            {
                startX = x;
                startY = y;
                break;
            }
        }

        ////Debug.Log(string.Format("Start x:{0} y:{1}", startX, startY));
        Stack<RoadPoint> stack = new Stack<RoadPoint>();
        stack.Push(new RoadPoint(startX, startY));
        mTiles[startX][startY].mGenx = 4;

        var cnt = 0;
        while (stack.Count > 0)
        {
            cnt += 1;
            if (cnt > 1000)
            {
                throw new Exception("xxx");
            }
            var status = 0;
            var rd = stack.Pop();
            var match = false;
            // //Debug.Log(string.Format("pop x:{0} y:{1}", rd.X, rd.Y));
            var rdTiled = mTiles[rd.X][rd.Y];

            rdTiled.Cnt++;
            if (rdTiled.Cnt > 4)
            {
                // //Debug.Log(string.Format("error x:{0} y:{1}", rd.X, rd.Y));
            }

            while (rd.DoesHaveLeftDirection() && !match)
            {
                int rx = 0, ry = 0;
                int x = rd.GeDirectionUnUsedRandom();
                int y = 0;
                rd.SetDirectionUsed(x);
                switch (x)
                {
                    case 1:
                        rx += 2;
                        y = 2;
                        break;
                    case 2:
                        rx -= 2;
                        y = 1;
                        break;
                    case 3:
                        ry += 2;
                        y = 4;
                        break;
                    case 4:
                        ry -= 2;
                        y = 3;
                        break;
                    default:
                        throw new Exception("xxx");
                }
                var rx1 = rd.X + rx;
                var ry1 = rd.Y + ry;
                //if (rx1 >= 1 && rx1 < mSizeX + 1 && ry1 >= 1 && ry1 < mSizeY + 1)
                if (CheckPosValid(rx1, ry1))
                {
                    var tile = mTiles[rx1][ry1];
                    if (tile.mCls == Tiled.Cls.Floor && (tile.mGenx == 2))
                    {
                        rdTiled.mGenx = (4);
                        stack.Push(rd);
                        status = 1;
                        // //Debug.Log(string.Format("push old x:{0} y:{1} dir:{2}", rd.X, rd.Y, x));
                        var nrp = new RoadPoint(rx1, ry1);
                        nrp.SetDirectionUsed(y);
                        stack.Push(nrp);
                        mTiles[nrp.X][nrp.Y].mGenx = (4);

                        // //Debug.Log(string.Format("push x:{0} y:{1} dir:{2}", rx1, ry1, y));
                        var xMiddle = (rx1 + rd.X) / 2;
                        var yMiddle = (ry1 + rd.Y) / 2;
                        mTiles[xMiddle][yMiddle] =
                            Unit.MakeRandomTiled(Tiled.Cls.Floor, 5, new Vector2Int(xMiddle, yMiddle));
                        match = true;
                    }
                }
            }

            if (status == 0)
            {
                rdTiled.mGenx = (4);
            }
        }
    }



    // 在迷宫中生成房间
    private void GenRoom(List<Room> mRooms, int mRoomSize, int mRoomExtraSize, int numRoomTries)
    {

        Debug.LogFormat("GenRoom try {0}", numRoomTries);
        for (int i = 0; i < numRoomTries; i++)
        {
            int size = UnityEngine.Random.Range(mRoomSize / 2, (mRoomExtraSize + mRoomExtraSize) / 2) * 2;
            int rectangularity = UnityEngine.Random.Range(1, mRoomSize / 2) * 2;
            int w = size;
            int h = size;

            if (0 == UnityEngine.Random.Range(0, 100) % 2)
            {
                w += rectangularity;
            }
            else
            {
                h += rectangularity;
            }

            int x = UnityEngine.Random.Range(1, (mSizeX + 1 - w) / 2) * 2 + 1;
            int y = UnityEngine.Random.Range(1, (mSizeY + 1 - h) / 2) * 2 + 1;
            Room room = new Room(new RectInt(x, y, w, h));
            bool overlaps = false;

            if (!room.IsInside(mSizeX, mSizeY))
            {
                continue;
            }

            foreach (Room other in mRooms)
            {
                if (room.IsOverlap(other))
                {
                    overlaps = true;
                    break;
                }
            }

            if (overlaps)
            {
                continue;
            }

            //  将新房间加入房间列表
            mRooms.Add(room);

            for (int k = x; k <= x + w; ++k)
            {
                for (int j = y; j <= y + h; ++j)
                {
                    Debug.LogFormat("tile floor {0} {1} room {2}", k, j, room.mRect);
                    mTiles[k][j] = Unit.MakeRandomTiled(Tiled.Cls.Floor, 3, new Vector2Int(k, j));
                }
            }
        }
    }

    // 生成房间和迷宫之间的门
    private void GenRoomRoadDoor(List<Room> mRooms, int roomDoorCnt)
    {
        foreach (var room in mRooms)
        {
            var DoorCnt = UnityEngine.Random.Range(roomDoorCnt, roomDoorCnt + roomDoorCnt / 2);
            while (DoorCnt > 0)
            {
                DoorCnt--;
                var edge = room.RandomEdge();
                var edgeOut = room.OutEdgePoint(edge, 2);
                if (CheckPosValid(edgeOut.x, edgeOut.y))
                {
                    var tile = mTiles[edgeOut.x][edgeOut.y];
                    if (tile.mCls == Tiled.Cls.Floor)
                    {
                        var middleX = (edge.x + edgeOut.x) / 2;
                        var middleY = (edge.y + edgeOut.y) / 2;
                        mTiles[middleX][middleY] = Unit.MakeRandomTiled(Tiled.Cls.Floor, 1,
                                                                        new Vector2Int(middleX, middleY));
                    }
                }
            }
        }
    }

    // 随机找到结束点和玩家生成点.
    public void GenStartAndEnd()
    {
        var start = RandomUnUsedPointOnX(2, 4);
        Vector2Int end = RandomUnUsedPointOnX(mSizeX - 1, mSizeX + 1);
        var tile = Unit.MakeRandomTiled(Tiled.Cls.End, 1, new Vector2Int(end.x, end.y));
        mTiles[end.x][end.y] = tile;
        tile.Used = true;
        mStart = new Vector2Int(start.x, start.y);
    }


    public void GenTileReplaceRandomFloor(Tilefg cfg)
    {
        for (var j = 0; j < cfg.Cnt; j++)
        {
            GenTileCheck(Tiled.Cls.Floor, 100, delegate (Tiled tile)
                         {
                             if (tile.Used)
                             {
                                 return false;
                             }
                             tile.SetUsed((Tiled.Cls)cfg.Cls, cfg.Id);
                             return true;
                         });
        }
    }

    private delegate bool tileCheck(Tiled tile);

    // 检查一个tile某一个方向是否为连续的floor
    public bool checkFloorLine(Tiled tile, int cnt)
    {
        foreach (var dir in Unit.Direction4)
        {
            var matchCnt = 0;
            var matchSize = cnt;
            var ti = tile.mIdx;
            for (var i = 0; i < matchSize; i++)
            {
                ti = Unit.NextTiledIdx(ti, dir);
                if (CheckPosValid(ti.x, ti.y) && mTiles[ti.x][ti.y].mCls == Tiled.Cls.Floor)
                {
                    matchCnt += 1;
                }
            }

            if (matchCnt == matchSize)
            {
                tile.mFace = dir;
                return true;
            }
        }

        return false;
    }

    // check four direction fllor
    public bool checkFloor4Direction(Tiled tile)
    {
        var matchCnt = 0;
        foreach (var dir in Unit.Direction4)
        {
            var ti = Unit.NextTiledIdx(tile.mIdx, dir);
            var tileDir = mTiles[ti.x][ti.y];
            if (CheckPosValid(ti.x, ti.y) && tileDir.mCls == Tiled.Cls.Floor && tileDir.Used == false)
            {
                matchCnt += 1;
            }
        }

        Debug.LogFormat("checkFloor4Direction tile {0} {1} match cnt {2}", tile.mX, tile.mY, matchCnt);
        if (matchCnt >= 3)
        {
            foreach (var dir in Unit.Direction4)
            {
                var ti = Unit.NextTiledIdx(tile.mIdx, dir);
                var tileDir = mTiles[ti.x][ti.y];
                var ret = (CheckPosValid(ti.x, ti.y) && tileDir.mCls == Tiled.Cls.Floor && tileDir.Used == false);
                Debug.LogFormat("checkFloor4Direction tile {0} {1} {2} used {3}  valid {4} fllor {5}", ti.x, ti.y, ret, tileDir.Used,
                                CheckPosValid(ti.x, ti.y), tileDir.mCls);
            }
        }

        if (matchCnt == 4)
        {
            foreach (var dir in Unit.Direction4)
            {
                var ti = Unit.NextTiledIdx(tile.mIdx, dir);
                mTiles[ti.x][ti.y].Used = true;
            }

            return true;
        }

        return false;
    }


    private bool GenTileCheck(Tiled.Cls cls, int tries, tileCheck tc)
    {
        bool alwaysTry = false;
        if (tries < 0)
        {
            alwaysTry = true;
        }

        while (alwaysTry || tries > 0)
        {
            tries -= 1;

            var x = UnityEngine.Random.Range(1, mSizeX + 1);
            var y = UnityEngine.Random.Range(1, mSizeY + 1);
            var tile = mTiles[x][y];
            if (tile.mCls == cls && tc(tile))
            {
                return true;
            }
        }

        return false;
    }

    // 单独生产箭塔
    public void GenTileDefenceBow(Tilefg cfg)
    {
        Debug.LogFormat("GenTileDefenceBow {0} {1} {2}", cfg.Cls, cfg.Id, cfg.Cnt);
        tileCheck tc = delegate (Tiled tile)
            {
                if (tile.Used)
                {
                    return false;
                }
                Debug.LogFormat("GenTileDefenceBow tiled {0} {1} ", tile.mX, tile.mY);
                if (checkFloorLine(tile, 3))
                {
                    tile.SetUsed((Tiled.Cls)cfg.Cls, cfg.Id);
                    return true;
                }
                else
                {
                    return false;
                }
            };
        for (var j = 0; j < cfg.Cnt; j++)
        {
            GenTileCheck(Tiled.Cls.Block, 100, tc);
        }
    }

    // 单独生产箭塔
    public void GenTileDefenceAxe(Tilefg cfg)
    {
        //Debug.LogFormat("GenTileDefenceAxe {0} {1} {2}", cfg.Cls, cfg.Id, cfg.Cnt);
        tileCheck tc = delegate (Tiled tile)
            {
                if (tile.Used)
                {
                    return false;
                }
                //Debug.LogFormat("GenTileDefenceAxe tiled {0} {1} ", tile.mX, tile.mY);
                if (checkFloor4Direction(tile))
                {
                    tile.SetUsed((Tiled.Cls)cfg.Cls, cfg.Id);
                    return true;
                }
                else
                {
                    return false;
                }
            };

        for (var j = 0; j < cfg.Cnt; j++)
        {
            GenTileCheck(Tiled.Cls.Floor, 1000, tc);
        }
    }


    // 生成防御块
    public void GenTileDefence(Tilefg cfg)
    {
        switch (cfg.Id)
        {
            case 1:
                GenTileDefenceBow(cfg);
                break;
            case 2:
                GenTileDefenceAxe(cfg);
                break;
            default:
                break;
        }
    }


    // 生成水
    public void GenTileWater(Tilefg cfg)
    {
        Debug.LogFormat("GenTileWater {0}", cfg.Cnt);
        tileCheck tc = delegate (Tiled tile)
            {
                tile.SetUsed((Tiled.Cls)cfg.Cls, cfg.Id);
                return true;
            };

        for (var j = 0; j < cfg.Cnt; j++)
        {
            var ret = GenTileCheck(Tiled.Cls.Block, 1000, tc);
            Debug.LogFormat("GenTileWater idx {0} ret {1}", j, ret);
        }
    }


    // 生成封印之石
    public void GenTileFracBox(Tilefg cfg)
    {
        Debug.LogFormat("GenTileFracBox {0}", cfg.Cnt);
        tileCheck tc = delegate (Tiled tile)
            {
                tile.SetUsed((Tiled.Cls)cfg.Cls, cfg.Id);
                return true;
            };

        for (var j = 0; j < cfg.Cnt; j++)
        {
            var ret = GenTileCheck(Tiled.Cls.Block, 1000, tc);
            Debug.LogFormat("GenTileWater idx {0} ret {1}", j, ret);
        }
    }


    // 生成特殊地块
    public void GenTiles(Tilefg[] cfgs)
    {
        foreach (var cfg in cfgs)
        {
            switch ((Tiled.Cls)cfg.Cls)
            {
                case Tiled.Cls.Defence:
                    GenTileDefence(cfg);
                    break;
                case Tiled.Cls.Water:
                    GenTileWater(cfg);
                    break;
                case Tiled.Cls.FracBox:
                    GenTileFracBox(cfg);
                    break;
                default:
                    GenTileReplaceRandomFloor(cfg);
                    break;
            }
        }
    }

    // public void GenItems(List<DeadEnd> mDeadEnds)
    // {
    //     for (var i = 0; i < 10; i++)
    //     {
    //         var point = RandomUnUsedPoint();
    //         var tile = mTiles[point.x][point.y];
    //         tile.mItem = new Item(Item.Cls.Coin);
    //         tile.Used = true;
    //     }

    //     if (mDeadEnds.Count > 0)
    //     {
    //         for (var i = 0; i < 10; i++)
    //         {
    //             var deadEnd = mDeadEnds[UnityEngine.Random.Range(0, mDeadEnds.Count)];
    //             var tile = mTiles[deadEnd.mX][deadEnd.mY];
    //             if (!tile.Used)
    //             {
    //                 tile.Used = true;
    //                 var item = new Item(Item.Cls.Chest);
    //                 item.mFace = deadEnd.mFace;
    //                 tile.mItem = item;
    //             }
    //         }
    //     }
    // }


    public void GenItems(Itemfg[] cfgs)
    {
        if (cfgs == null)
        {
            return;
        }

        foreach (var cfg in cfgs)
        {
            Debug.LogFormat("Gen item {0} {1}", cfg.Id, cfg.Cnt);
            for (var j = 0; j < cfg.Cnt; j++)
            {
                var point = RandomUnUsedPoint();
                var tile = mTiles[point.x][point.y];
                tile.mItemCls = new ItemCls((ItemCls.Cls)cfg.Id);
                tile.Used = true;
            }
        }
    }



    public void GenMonsters(MonsterCfg[] cfgs)
    {
        if (cfgs == null)
        {
            return;
        }

        foreach (var cfg in cfgs)
        {
            Debug.LogFormat("Gen npc {0} {1}", cfg.Id, cfg.Cnt);
            for (var j = 0; j < cfg.Cnt; j++)
            {
                var point = RandomUnUsedPoint();
                var tile = mTiles[point.x][point.y];
                tile.mMonsterCls = new MonsterCls(cfg.Id);
                tile.Used = true;
            }
        }
    }



    public void GenNpcs(NpcCfg[] cfgs)
    {

        if (cfgs == null)
        {
            return;
        }

        foreach (var cfg in cfgs)
        {
            Debug.LogFormat("Gen npc {0} {1}", cfg.Id, cfg.Cnt);
            for (var j = 0; j < cfg.Cnt; j++)
            {
                var point = RandomUnUsedPoint();
                var tile = mTiles[point.x][point.y];
                tile.mNpcCls = new NpcCls(cfg.Id);
                tile.Used = true;
            }
        }
    }



    public Tiled TiledIndex(Vector2Int vec)
    {
        return mTiles[vec.x][vec.y];
    }

    public Tiled TiledIndex(int x, int y)
    {
        return mTiles[x][y];
    }

    [System.Serializable]

    // private class TiledItem
    // {
    // 	public Tiled.Cls mCls;
    // 	public int mId;
    // 	public bool Used;
    // }


    private class Wrapper
    {
        public Tiled[] Items;
        public int SizeX;
        public int SizeY;
        public Vector2Int Start;
    }

    public string ToJson()
    {
        var l = new Tiled[(mSizeX + 2) * (mSizeY + 2)];
        for (var i = 0; i < mTiles.Length; i++)
        {
            var iX = mTiles[i];
            for (var j = 0; j < iX.Length; j++)
            {
                l[i * (mSizeY + 2) + j] = iX[j];
            }
        }

        Wrapper wrapper = new Wrapper();
        wrapper.Items = l;
        wrapper.SizeX = mSizeX;
        wrapper.SizeY = mSizeY;
        wrapper.Start = mStart;

        return JsonUtility.ToJson(wrapper, true);
    }

    public void FromJson(string json)
    {
        Wrapper wrapper = JsonUtility.FromJson<Wrapper>(json);
        mSizeX = wrapper.SizeX;
        mSizeY = wrapper.SizeY;
        mStart = wrapper.Start;

        mTiles = new Tiled[mSizeX + 2][];
        for (var i = 0; i < mSizeX + 2; i++)
        {
            var iX = new Tiled[mSizeY + 2];
            for (var j = 0; j < mSizeY + 2; j++)
            {
                iX[j] = wrapper.Items[i * (mSizeY + 2) + j];
                iX[j].mObj = null;

                if (iX[j].mNpcCls != null && iX[j].mNpcCls.mId == 0)
                {
                    iX[j].mNpcCls = null;
                }

                if (iX[j].mNpcCls != null)
                {
                    Debug.LogFormat("npc ni not null {0}", iX[j].mNpcCls);
                }
            }
            mTiles[i] = iX;
        }

    }

    public void CharEvent(string eventName, Char c)
    {
        //Debug.LogFormat("tile event {0} {1}:{2}", eventName, tile.mX, tile.mY);
        switch (eventName)
        {
            case "char.dead":
                break;
        }
    }

    public void CharDead(Char c)
    {
        SetPointUnOccupy(c.mState.mCurIdx, c);
        Debug.LogFormat("Char dead {0} {1}", c.mCls, c.mId);
        mMonsters.Remove(c);
    }

    public void FracBoxBroken(FracBox ss)
    {
        mFracBox.Remove(ss);
        //mRole.mCoin += 10;
    }


    // 查找是否有npc在追玩家.
    public bool DoesMonsterChasingRole()
    {
        bool ret = false;
        foreach (var npc in mMonsters)
        {
            var state = npc.mState.mState;
            if (state == CharState.State.Chase || state == CharState.State.Atk)
            {
                ret = true;
                break;
            }
        }

        return ret;
    }

    // 减少中间.
    private void DecreasePath(List<Vector2Int> paths)
    {
        if (paths.Count == 0)
        {
            return;
        }

        Shuffle.X1(paths);
        //var cnt = paths.Count - paths.Count / 5;
        var cnt = paths.Count / 5;
        var i = 0;
        while (i < cnt)
        {
            var pt = paths[0];
            paths.RemoveAt(0);
            mTiles[pt.x][pt.y] = Unit.MakeRandomTiled(Tiled.Cls.Floor, 6, new Vector2Int(pt.x, pt.y));
            i += 1;
            Debug.LogFormat("decrease {0} paths", pt);
        }
    }

    // 是否摧毁了所有结界之石
    public bool IsAllFracBoxBroken()
    {
        return mFracBox.Count == 0;
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

    // 寻找与c相邻的所有的char ..
    public Dictionary<int, Char> GetAdjacentChar(Char c)
    {
        Dictionary<int, Char> dictRet = new Dictionary<int, Char>();
        Queue<Char> listChar = new Queue<Char>();
        listChar.Enqueue(c);
        while (listChar.Count > 0)
        {
            var c1 = listChar.Dequeue();
            // @ fix 数据有问题处理防御
            if (!dictRet.ContainsKey(c1.mId))
            {
                dictRet.Add(c1.mId, c1);
            }
            else
            {
                continue;
            }

            Unit.AdjacentIdxList(c1.mState.mCurIdx, delegate (Vector2Int idx)
                                 {
                                     if (!CheckPosValid(idx.x, idx.y))
                                     {
                                         return;
                                     }
                                     var tile = mTiles[idx.x][idx.y];
                                     var occupyChar = tile.OccupiedChar();
                                     if (occupyChar == null)
                                     {
                                         return;
                                     }

                                     if (c.CharType() != occupyChar.CharType())
                                     {
                                         return;
                                     }

                                     if (dictRet.ContainsKey(occupyChar.mId))
                                     {
                                         return;
                                     }
                                     listChar.Enqueue(occupyChar);
                                 });
        }
        return dictRet;
    }


}
