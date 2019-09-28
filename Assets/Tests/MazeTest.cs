using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;


public class MazeTest
{
    [Test]
    public void MazeGenShow()
    {
        // var room1 = new Maze(1,1);
        // var room2 = new Maze(1,3);
        // var room3 = new Maze(5,1);
        var room4 = new Maze(21, 21);
        room4.GenRandomMaze(4, 3);
    }


    [Test]
    public void MazeAstar()
    {
        // var room1 = new Maze(1,1);
        // var room2 = new Maze(1,3);
        // var room3 = new Maze(5,1);
        var room4 = new Maze(21, 21);
        room4.GenRandomMaze(4, 3);
        var p1 = room4.RandomUnUsedPoint();
        room4.SetPointOccupy(p1, null);

        var p2 = room4.RandomUnUsedPoint();
        var c = new Role(Vector3.zero);
        room4.SetPointOccupy(p2, null);
        Astar.FindPath(room4, p1, p2, c);
    }

    [Test]
    public void MazeJson()
    {
        // var room1 = new Maze(1,1);
        // var room2 = new Maze(1,3);
        // var room3 = new Maze(5,1);
        var t1 = new Tiled(new Vector2Int(0, 1), Tiled.Cls.Block, 1, 0);
        Debug.Log(JsonUtility.ToJson(t1));

        var r1 = new Maze(21, 21);
        Debug.Log(JsonUtility.ToJson(r1));
    }


    [Test]
    public void MazeCfgJson()
    {
        var x = new MazeCfg();
        var data = JsonUtility.ToJson(x, true);
        Debug.Log(data);
        var y = JsonUtility.FromJson<MazeCfg>(data);
        //var JsonUtility.FromJson(x, MazeCfg)
    }

    delegate void TestDelegate(string s);

    static void TestM(string s)
    {
        Debug.Log(s);
    }

    [Test]
    public void MazeLambda()
    {
        TestDelegate t1 = new TestDelegate(TestM);
        t1("hello,World");

        TestDelegate t2 = delegate (string s) { Debug.LogFormat("good night {0}", s); };
        t2("good");
        TestDelegate t3 = (s) => { Debug.LogFormat("I am sorry {0}", s); };

        t3("good");

    }




    // A MazeyTest behaves like a coroutine in PlayMode
    // and allows you to yield null to skip a frame in EditMode
    [UnityTest]
    public IEnumerator MazeTestWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // yield to skip a frame
        yield return null;
    }
}

