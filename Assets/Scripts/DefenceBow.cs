using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenceBow : MonoBehaviour, ITrapCsvCfg
{
    public GameObject mArrow;
    public FlyObject mcurArrow;
    public Direction mDir;
    public Maze mMaze;
    public GameObject mSpark;

    private float mSleepTime = -1.0f;
    private float mPrepareTime = -1.0f;
    private int mStatus = 0;

    // Use this for initialization
    void Start()
    {
        mArrow.SetActive(false);
        Prepare();
        mStatus = 1;
        mSleepTime = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (mMaze.mActive == false)
        {
            //return;
        }

        if (mStatus == 0)
        {
            mPrepareTime -= Time.deltaTime;
            if (mPrepareTime < 0)
            {
                mPrepareTime = 2.0f;
                Prepare();
                mSleepTime = 1.0f;
                mStatus = 1;
            }
        }

        if (mStatus == 1)
        {
            mSleepTime -= Time.deltaTime;
            if (mSleepTime < 0)
            {
                Fire();
                mPrepareTime = 1.0f;
                mStatus = 0;
            }
        }

    }

    // 发射
    void Prepare()
    {
        var fireArrow = Instantiate(mArrow, transform);
        var oldPos = fireArrow.transform.position;
        var fatherPos = transform.position;
        fireArrow.transform.position = new Vector3(fatherPos.x, oldPos.y, fatherPos.z);
        fireArrow.transform.rotation = transform.rotation;
        fireArrow.SetActive(true);
        var arrow = fireArrow.AddComponent<FlyObject>();
        arrow.SetDirection(mDir, 0);
        arrow.mOwner = gameObject;
        mcurArrow = arrow;
        mSpark.SetActive(false);
    }

    void Fire()
    {
        mSpark.SetActive(true);
        mcurArrow.SetSpeed(16);
        mSpark.GetComponent<ParticleSystem>().Play();
    }

    // Use this for initialization
    public int CsvCfgId()
    {
        return CsvConst.ToTrapCfgId(CsvConst.TrapCfgTypeDefence, TrapConst.DefenceBow);
    }


}
