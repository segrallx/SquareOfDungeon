using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEditor;
using System.Text;

public class GamePlay : MonoBehaviour
{
    // public GameObject mDeadUI;
    //    public GameObject mDemo;

    public Text mLevelUI;
    public GameObject mHomeUI;
    public GameObject mBattleUI;
    public GameObject mDeadUI;
    public GameObject mUpgradeUI;

    ////////////////////////////////////////
    // maze ctl
    public RoleRender mRoleRender;
    public MazeRender mMazeRender;
    private Maze mMaze;
    public int mCurLevel = 0;
    public int mRestart = 0;

    // Update is called once per frame
    TextAsset ReadHomeMazeFile()
    {
        var txtAsset = Resources.Load<TextAsset>("Mazes/home");
        var x = txtAsset.text;
        Debug.LogFormat("xx {0}", x);
        return txtAsset;
    }

    void SaveHomeMazeFile(string content)
    {
        Debug.Log("SaveHomeMazeFile");
        FileStream file = new FileStream("./tmp_home.txt", FileMode.Create | FileMode.Open);
        file.Seek(0, SeekOrigin.Begin);
        var data = Encoding.ASCII.GetBytes(mMaze.ToJson());
        file.Write(data, 0, data.Length);
        file.Close();
    }


    Maze MazeNew()
    {
        //mMaze = new Maze(mMazeSizeX, mMazeSizeY);
        //mMaze.GenRandomMaze(mMazeRoomSize, mMazeRoomVolocity);
        if (mCurLevel == 0)
        {
            var txtAsset = ReadHomeMazeFile();
            //mMaze = new Maze(mMazeSizeX, mMazeSizeY);
            mMaze = new Maze(txtAsset);
        }
        else
        {
            var cfg = ReadGameMazeFile();
            mMaze = new Maze(cfg.SizeX, cfg.SizeY);
            mMaze.GenFromCfg(cfg);
        }

        mMaze.TileEventRegister(TileEvent);
        return mMaze;
    }

    // 初始化Maze.
    void StartMaze()
    {
        Debug.LogFormat("game player start ->");
        //mDemo.SetActive(false);
        mMaze = MazeNew();
        StaffHolder.Instance().mMaze = mMaze;
        mMazeRender.Render(mMaze);
        mRoleRender.RestartRole(mMaze, 0);
        RoleSetToStartPoint(true);
        UIModeByLevel();
        // mDeadUI.SetActive(false);
        mLevelUI.text = string.Format("Lv.{0}", mCurLevel);
        Debug.LogFormat("game player start <-");
    }

    // Use this for initialization
    void Start()
    {
        StartMaze();
    }


    // Update is called once per frame
    void Update()
    {

        if (Input.anyKeyDown)
        {
            mMaze.mActive = true;
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            GameRestart(mCurLevel, false);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            mCurLevel = 1;
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            mCurLevel = -1;
            GameRestart(0, true);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            var jsonCxt = mMaze.ToJson();
            //Debug.Log(jsonCxt);
            SaveHomeMazeFile(jsonCxt);
        }

    }

    // 根据关卡来显示不同的UI 0-关卡主城 >0-战斗
    void UIModeByLevel()
    {
        Debug.LogFormat("UIModeByLevel {0}", mCurLevel);
        if (mCurLevel == 0)
        {
            mHomeUI.SetActive(true);
            mBattleUI.SetActive(false);
            mDeadUI.SetActive(false);
            mUpgradeUI.SetActive(false);
        }
        else
        {
            mHomeUI.SetActive(false);
            mBattleUI.SetActive(true);
        }
    }


    void RoleSetToStartPoint(bool isHome)
    {
        var start = mMaze.mStart;
        Debug.LogFormat("role start point {0}:{1} isHome:{2}", start.x, start.y, isHome);
        var role = mRoleRender.GetComponent<RoleRender>();
        role.IdleSet();
        role.PositionSet(start);
        mMaze.SetPointOccupy(start, role.mChar);


        if (isHome)
        {
            role.HpReset();
            role.LevelReset();
            role.HpBarSetActive(false);
        }
        else
        {
            role.HpBarSetActive(true);
        }

        mMaze.SetRole(role.GetRole());
    }


    public void GameRestart(int level, bool resetHp)
    {
        Debug.LogFormat("GameRestart");
        if (mMaze != null)
        {
            mMaze.TileEventUnRegister(TileEvent);
        }

        mCurLevel = level;
        mMaze = MazeNew();
        StaffHolder.Instance().mMaze = mMaze;
        mRoleRender.RestartRole(mMaze, mCurLevel);
        mMazeRender.RefreshRender(mMaze);
        mLevelUI.text = string.Format("Lv.{0}", mCurLevel);
        RoleSetToStartPoint(mCurLevel == 0);
        UIModeByLevel();
        Debug.LogFormat("current level {0}", mCurLevel);
        mRoleRender.ShowUpgrade();
    }

    public void GameOver()
    {
        // mDeadUI.SetActive(true);
    }


    // Update is called once per frame
    MazeCfg ReadGameMazeFile()
    {
        var levelCfg = string.Format("Mazes/gen{0}", mCurLevel);
        var x = Resources.Load<TextAsset>(levelCfg).text;
        Debug.LogFormat("level {0} xx {1}", mCurLevel, x);
        return JsonUtility.FromJson<MazeCfg>(x);

    }


    private void TileEvent(string eventName, Tiled tile, Char c)
    {
        //Debug.LogFormat("game play next {0} {1}:{2}", eventName, tile.mX, tile.mY);
        // && mMaze.IsAllSealStoneBroken()
        if (tile.mCls == Tiled.Cls.End && c.IsRole() &&
            !mMaze.DoesMonsterChasingRole())
        {
            mCurLevel += 1;
            GameRestart(mCurLevel, false);
        }
    }

}

