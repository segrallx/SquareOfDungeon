using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// obsolete
public class ValueUI : MonoBehaviour
{
    public Text mTxt;
    public int mRealCnt;
    public int mShowCnt;
    public int mTrigger = 0;


    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (mTrigger == 1 && mRealCnt != mShowCnt)
        {
            var val = Mathf.CeilToInt((mRealCnt - mShowCnt) / 20);
            if (val == 0 && mRealCnt > mShowCnt)
            {
                val = 1;
            }
            else if (val == 0 && mRealCnt < mShowCnt)
            {
                val = -1;
            }
            Debug.LogFormat("trigger coin show {0}", val);
            mShowCnt += val;
            mTxt.text = string.Format("{0}", mShowCnt);
        }
        else if (mTrigger == 1 && mRealCnt == mShowCnt)
        {
            mTrigger = 0;
        }
    }



    public void SetValue(int val)
    {
        mRealCnt = val;
        mShowCnt = val;
        mTxt.text = string.Format("{0}", mShowCnt);
        Debug.LogFormat("value set {0}", val);
    }

    public void ShowValue(int val)
    {
        mTrigger = 1;
        Debug.LogFormat("value show {0}", val);
        mRealCnt = val;
    }
}
