using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandThorn : MonoBehaviour, ITrapCsvCfg
{
    // 两个运动组件
    Transform mLand;
    Transform mThorn;
    public GameObject mSpark;

    // 初始sleep时间
    const float mInitSleepTime = 1.5f;

    // 速度
    public float mSpeed = 1f;

    float mSleepTime;
    //移动速度

    enum LandThornStatus
    {
        None = 0,
        Up = 1,
        Down = 2,
    };

    LandThornStatus mLandThorn = LandThornStatus.None;

    void Awake()
    {
        mLand = transform.Find("Land");
        mThorn = transform.Find("Thorn");
        mSpark.SetActive(false);
        mThorn.transform.localPosition = new Vector3(0, -2.5f, 0);
        //Invoke("BeginThorn", Random.Range(0, 2));
        Invoke("BeginThorn", 1);
    }

    // Use this for initialization
    void BeginThorn()
    {
        mLandThorn = LandThornStatus.Up;
    }

    // Use this for initialization
    public int CsvCfgId()
    {
        return CsvConst.ToTrapCfgId(CsvConst.TrapCfgTypeTrap, TrapConst.TrapLandThornId);
    }

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (mLandThorn == LandThornStatus.None)
        {
            return;
        }

        if (mSleepTime > 0)
        {
            mSleepTime -= Time.deltaTime;
            return;
        }

        switch (mLandThorn)
        {
            case LandThornStatus.Up:
                mThorn.localPosition = mThorn.localPosition + new Vector3(0, mSpeed, 0);
                if (mThorn.localPosition.y >= 0)
                {
                    mLandThorn = LandThornStatus.Down;
                    mSleepTime = mInitSleepTime;
                    mSpark.SetActive(true);
                    mSpark.GetComponent<ParticleSystem>().Play();
                }
                break;
            case LandThornStatus.Down:
                mThorn.localPosition = mThorn.localPosition - new Vector3(0, mSpeed, 0);
                if (mThorn.localPosition.y <= -2.5f)
                {
                    mLandThorn = LandThornStatus.Up;
                    mSleepTime = mInitSleepTime;
                    mSpark.SetActive(false);
                }
                break;
        }

    }
}
