using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MazeRender : MonoBehaviour
{

    public GameObject[] mBlocks;
    public GameObject[] mWaters;
    public GameObject[] mFloor;
    public GameObject[] mDoor;
    public GameObject[] mEnd;
    public GameObject[] mTraps;
    public GameObject[] mDefences;
    public GameObject[] mItems;
    public GameObject[] mMonsters;
    public GameObject[] mNpcs;
    public GameObject[] mWorkbenchs;
    public GameObject[] mDecorators;
    public GameObject[] mFracBoxs;
    public Maze mMaze;

    // 维护所有Floor的引用.
    private GameObject[] mFloorObjs;
    public List<GameObject> mMonsterObjs;

    private int mFloorIdx;

    // Use this for initialization
    void Start()
    {
    }

    // 在当前位置渲染房间
    public void Render(Maze maze)
    {
        //var itemUp = new Vector3(0, 0.5f, 0);
        mMaze = maze;
        mMonsterObjs = new List<GameObject>();
        mFloorObjs = new GameObject[(maze.mSizeX + 2) * (maze.mSizeY + 2)];
        mFloorIdx = Random.Range(0, mFloor.Length);
        for (var i = 0; i < maze.mTiles.Length; i++)
        {
            var row = maze.mTiles[i];
            for (var j = 0; j < row.Length; j++)
            {
                var tile = row[j];
                var pos = new Vector3(i * 2, 0, j * 2);
                RenderTile(tile, pos);
                if (tile.mItemCls != null)
                {
                    renderItem(tile.mItemCls, pos);
                }

                if (tile.mMonsterCls != null)
                {
                    renderMonster(tile.mMonsterCls, pos);
                }

                if (tile.mNpcCls != null)
                {
                    var npc = renderNpc(tile.mNpcCls, pos);
                    tile.SetOccupied(npc);
                }

            }
        }

        //mMaze.mTileEvent += TileEvent;
        mMaze.TileEventRegister(TileEvent);
    }

    public void RenderTile(Tiled tile, Vector3 pos)
    {
        GameObject obj = null;
        switch (tile.mCls)
        {
            case Tiled.Cls.Block:
                obj = renderBlock(tile, pos);
                break;
            case Tiled.Cls.Floor:
                obj = renderFloor(tile, pos);
                break;
            case Tiled.Cls.Door:
                obj = renderDoor(tile, pos);
                break;
            case Tiled.Cls.Workbench:
                obj = renderWorkbench(tile, pos);
                break;
            case Tiled.Cls.Trap:
                obj = renderTrap(tile, pos);
                break;
            case Tiled.Cls.Defence:
                obj = renderDefence(tile, pos);
                break;
            case Tiled.Cls.Water:
                obj = renderWater(tile, pos);
                break;
            case Tiled.Cls.FracBox:
                obj = renderFracBox(tile, pos);
                break;
            // case Tiled.Cls.Start:
            //     obj = Instantiate(mEnd[0], gameObject.transform);
            //     break;
            case Tiled.Cls.End:
                obj = renderEnd(tile, pos);
                break;
            default:
                break;
        }

        if (obj == null)
        {
            return;
        }

        var tileRender = obj.GetComponent<TiledRender>();
        if (tileRender == null)
        {
            tileRender = obj.AddComponent<TiledRender>();
        }

        tile.mObj = obj;
        tileRender.mTiled = tile;
        tileRender.mMazeRender = this;
    }


    private GameObject renderBlock(Tiled tile, Vector3 pos)
    {

        //var obj = Instantiate(mBlocks[Random.Range(0, mBlocks.Length)], gameObject.transform);
        var idx = (tile.mId - 1) % mBlocks.Length;
        if (idx < 0)
        {
            idx = 0;
        }
        //Debug.LogFormat("tile id {0}", idx);
        var obj = Instantiate(mBlocks[idx], gameObject.transform);
        //obj.transform.localScale = new Vector3(1, 1, UnityEngine.Random.Range(1, 2.5f));
        if (UnityEngine.Random.Range(0, 4) == 0 || tile.Stable)
        {

        }
        else
        {
            obj.transform.localScale = new Vector3(1, 1, 1.7f);
        }

        obj.transform.position = pos;
        var x = Random.Range(0, 4);
        if (!tile.Stable)
        {
            obj.transform.localRotation = Quaternion.Euler(-90, x * 90, 0);
        }
        else
        {
            var faceRotate = Unit.DirectionRotation(tile.mFace);
            obj.transform.localRotation = Quaternion.Euler(-90, faceRotate, 0);
        }

        return obj;
    }



    private GameObject renderWater(Tiled tile, Vector3 pos)
    {
        var obj = Instantiate(mWaters[Random.Range(0, mWaters.Length)], gameObject.transform);
        obj.transform.localScale = new Vector3(1, 1, 0.2f);
        obj.transform.position = pos;
        var x = Random.Range(0, 4);
        obj.transform.localRotation = Quaternion.Euler(-90, 270, 0);
        //obj.transform.localRotation = Quaternion.Euler(-90, x * 90, 0);
        return obj;
    }



    private GameObject renderFracBox(Tiled tile, Vector3 pos)
    {
        //var obj = Instantiate(mFracBoxs[Random.Range(0, mWaters.Length)], gameObject.transform);
        var obj = Instantiate(mFracBoxs[tile.mId - 1], gameObject.transform);
        //obj.transform.localScale = new Vector3(1, 1, 1f);
        obj.transform.position = pos;
        var x = Random.Range(0, 4);
        obj.transform.localRotation = Quaternion.Euler(0, x * 90, 0);
        var ss = obj.GetComponent<FracBox>();
        ss.MazeSet(mMaze);
        mMaze.mFracBox.Add(ss);
        return obj;
    }


    private GameObject renderFloor(Tiled tile, Vector3 pos)
    {

        //var obj = Instantiate(mFloor[mFloorIdx], gameObject.transform);
        var idx = 0;
        if (tile.mId > 3)
        {
            idx = tile.mId - 1;
        }
        else
        {
            idx = Random.Range(0, 3);
        }
        var obj = Instantiate(mFloor[idx], gameObject.transform);
        obj.transform.position = pos;
        var y = Random.Range(0, 10);
        if (y == 0)
        {
            obj.transform.localScale = new Vector3(0.95f, 0.95f, 0.95f);
            // switch(Random.Range(0,4)){
            // 	case 0:
            // 		obj.transform.position += new Vector3(0.1f,0,-0.1f);
            // 		break;
            // 	case 1:
            // 		obj.transform.position += new Vector3(-0.1f,0,0.1f);
            // 		break;
            // 	case 2:
            // 		obj.transform.position += new Vector3(0.1f,0,0.1f);
            // 		break;
            // 	case 3:
            // 		obj.transform.position += new Vector3(-0.1f,0,-0.1f);
            // 		break;
            // }
        }

        var x = Random.Range(0, 4);
        obj.transform.localRotation = Quaternion.Euler(-90, x * 90, 0);
        //obj.transform.localRotation =Quaternion.Euler(-90, 0, 0);
        //obj.GetComponent<TiledRender>().mTiled = tile;
        mFloorObjs[tile.mX * (mMaze.mSizeY + 2) + tile.mY] = obj;

        // 有一定概率生成装饰
        if (Random.Range(0, 20) == 0)
        {
            var dec = Instantiate(mDecorators[Random.Range(0, mDecorators.Length)],
                                  gameObject.transform);
            dec.transform.position = pos + new Vector3(0, 0.4f, 0);
            dec.transform.localRotation = Quaternion.Euler(0, 0, 0);
            //dec.transform.localRotation = Quaternion.Euler(0, x*90, 0);
            //dec.transform.localScale = new Vector3(0.7, 0.7f, 0.7f);
        }

        return obj;

    }

    private GameObject renderDoor(Tiled tile, Vector3 pos)
    {
        var obj = Instantiate(mDoor[0], gameObject.transform);
        obj.transform.position = pos;
        return obj;
    }

    private GameObject renderWorkbench(Tiled tile, Vector3 pos)
    {
        var obj = Instantiate(mWorkbenchs[tile.mId], gameObject.transform);
        obj.transform.position = pos;
        return obj;
    }

    private GameObject renderTrap(Tiled tile, Vector3 pos)
    {
        var obj = Instantiate(mTraps[tile.mId - 1], gameObject.transform);
        obj.transform.position = pos;
        var x = Random.Range(0, 4);
        obj.transform.localRotation = Quaternion.Euler(0, x * 90, 0);
        return obj;
    }

    private GameObject renderDefence(Tiled tile, Vector3 pos)
    {
        var obj = Instantiate(mDefences[tile.mId - 1], gameObject.transform);
        obj.transform.position = pos;
        var r = Unit.DirectionRotation(tile.mFace);
        Debug.LogFormat("defence face {0} r {1}", tile.mFace, r);
        if (tile.mFace != Direction.None)
        {
            obj.transform.localRotation = Quaternion.Euler(0, r, 0);
        }

        switch (tile.mId)
        {
            case 1:
                obj.GetComponent<DefenceBow>().mDir = tile.mFace;
                obj.GetComponent<DefenceBow>().mMaze = mMaze;
                break;
        }

        return obj;
    }


    private GameObject renderEnd(Tiled tile, Vector3 pos)
    {
        var obj = Instantiate(mEnd[0], gameObject.transform);
        obj.transform.position = pos;
        return obj;
    }

    private void renderItem(ItemCls item, Vector3 pos)
    {
        var obj = Instantiate(mItems[(int)item.mCls - 1], gameObject.transform);
        obj.transform.position = pos + new Vector3(0, 0.4f, 0);
    }


    private void renderMonster(MonsterCls monsterCls, Vector3 pos)
    {
        // renderMonster(monster, pos);
        var obj = Instantiate(mMonsters[monsterCls.mCls - 1], gameObject.transform);
        obj.transform.position = pos + new Vector3(0, 0.4f, 0);
        Monster monster;
        switch (monsterCls.mCls)
        {
            case 1:
                monster = new MeleeMonster(obj.transform.position, monsterCls.mCls);
                break;
            case 2:
                monster = new RemoteMonster(obj.transform.position, monsterCls.mCls);
                break;
            default:
                throw new System.Exception("should not be here");
        }

        var monsterRender = obj.GetComponent<MonsterRender>();
        monsterRender.InitComponent();
        monsterRender.CharSet(monster);
        monsterRender.CharGet().MazeSet(mMaze);
        mMaze.mMonsters.Add(monsterRender.CharGet());
        mMonsterObjs.Add(obj);

    }


    private Char renderNpc(NpcCls npcCls, Vector3 pos)
    {
        // renderNpc(npc, pos);
        var obj = Instantiate(mNpcs[npcCls.mId - 1], gameObject.transform);
        obj.transform.position = pos + new Vector3(0, 0.7f, 0);
        Npc npc = new Npc(obj.transform.position);
        var npcRender = obj.GetComponent<NpcRender>();

        npcRender.InitComponent();
        npcRender.CharSet(npc);
        npcRender.CharGet().MazeSet(mMaze);
        npcRender.CharGet().mState.DirectionSet(npcCls.mFace);

        var r = Unit.DirectionRotation(npcCls.mFace);
        if (npcCls.mFace != Direction.None)
        {
            obj.transform.localRotation = Quaternion.Euler(0, r, 0);
        }
        return npc;
    }


    // private void renderMonster(MonsterCls monster, Vector3 pos)
    // {
    //     var obj = Instantiate(mMonsters[0], gameObject.transform);
    //     obj.transform.position = pos + new Vector3(0, 0.3f, 0);
    //     var monsterRender = obj.GetComponent<MonsterRender>();
    //     monsterRender.InitComponent();
    //     slimeRender.mChar.MazeSet(mMaze);
    //     mMaze.mMonsters.Add(slimeRender.mChar);
    // }

    private void TileEvent(string eventName, Tiled tile, Char c)
    {
        //Debug.LogFormat("tile event {0} {1}:{2}", eventName, tile.mX, tile.mY);
        switch (eventName)
        {
            case "tile.occupy":
                //  debug occupy color
                return;
                // var idx = tile.mX * (mMaze.mSizeY + 2) + tile.mY;
                // if (tile.mCls == Tiled.Cls.Floor)
                // {
                //     var floorObj = mFloorObjs[idx];
                //     if (floorObj == null)
                //     {
                //         Debug.LogErrorFormat("exception {0}", tile);
                //     }
                //     var floorColor = floorObj.GetComponent<FloorColor>();
                //     if (floorColor == null)
                //     {
                //         Debug.LogErrorFormat("exception {0}", tile);
                //     }
                //     if (tile.Occupied())
                //     {
                //         floorColor.SetColor(Color.red);
                //     }
                //     else
                //     {
                //         floorColor.RecoverColor(); ;
                //     }
                // }
                // break;
        }
    }

    // 重新渲染迷宫
    public void RefreshRender(Maze maze)
    {

        foreach (var monster in mMonsterObjs)
        {
            if (monster != null)
            {
                monster.GetComponent<CharRender>().Destory();
            }
        }

        for (var i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            Destroy(child.gameObject);
        }

        Render(maze);
    }


}
