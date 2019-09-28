using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UltimateFracturing;

// 管理所有的可以破碎物品
public class FracBox : MonoBehaviour
{

    public GameObject mStone;
    private FracturedObject mFb;
    private bool mIsBroken = false;
    private Maze mMaze;

    // Use this for initialization
    void Start()
    {
        mFb = mStone.GetComponent<FracturedObject>();
    }

    public void MazeSet(Maze maze)
    {
        mMaze = maze;
    }

    public Maze MazeGet()
    {
        return mMaze;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnTriggerEnter(Collider coll)
    {
        //Debug.LogFormat("FracBox Trigger {0}", coll.tag);
        switch (coll.tag)
        {
            case "Sword":
                break;
            // case "Arrow":
            // 	break;
            default:
                return;
        }


        if (mStone != null)
        {
            mFb.Explode(mStone.transform.position, 30);
            Destroy(mStone);
            mIsBroken = true;
            mMaze.FracBoxBroken(this);

            // var rssEffect = GameObject.FindGameObjectWithTag("RssEffect").GetComponent<RssEffect>();
            // rssEffect.PlayCoin(transform.position, 10);
            gameObject.GetComponent<BoxCollider>().enabled = false;

            var staff = StaffHolder.Instance();
            staff.AddCoinToRole(5, transform.position);
        }

    }

    public bool IsBroken()
    {
        return mIsBroken;
    }

}
