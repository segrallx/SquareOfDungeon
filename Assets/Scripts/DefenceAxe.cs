using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenceAxe : MonoBehaviour, ITrapCsvCfg
{
    public GameObject mAxe;
    public GameObject mSpark;
    private float mSleepTime = 2.0f;

    void Start()
    {
        mSpark.SetActive(false);
    }

    // update
    void Update()
    {
        if (mSleepTime > 0)
        {
            mSleepTime -= Time.deltaTime;
            if (mSleepTime < 0)
            {
                mSpark.SetActive(false);
            }
        }
    }

    // 发射
    void Fire()
    {
        //Debug.LogFormat("Axe Fire");
        mSpark.SetActive(true);
        mSpark.GetComponent<ParticleSystem>().Play();
        mSleepTime = 0.8f;
    }


    // Use this for initialization
    public int CsvCfgId()
    {
        return CsvConst.ToTrapCfgId(CsvConst.TrapCfgTypeDefence, TrapConst.DefenceAxe);
    }


}
