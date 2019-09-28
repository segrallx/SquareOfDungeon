using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// obsolete
public class CoinUI : MonoBehaviour
{
    public Text mTxt;
    public int mRealCnt;
    public int mShowCnt;
    public int mTrigger = 0;
    private Role mRole;

    // Use this for initialization
    void Start()
    {
        //mTxt.text = string.Format("{0}", 100);
    }

    // Update is called once per frame
    void Update()
    {
        // if (mTrigger == 1 && mRealCnt != mShowCnt)
        // {
        //     var val = Mathf.CeilToInt((mRealCnt - mShowCnt) / 20);
        //     if (val == 0 && mRealCnt > mShowCnt)
        //     {
        //         val = 1;
        //     }
        //     else if (val == 0 && mRealCnt < mShowCnt)
        //     {
        //         val = -1;
        //     }
        //     Debug.LogFormat("trigger coin show {0}", val);
        //     mShowCnt += val;
        //     mTxt.text = string.Format("{0}", mShowCnt);
        // }
        // else if (mTrigger == 1 && mRealCnt == mShowCnt)
        // {
        //     mTrigger = 0;
        // }
    }

    public void SetRole(Role role)
    {
        // mRole = role;
        // mRealCnt = role.mCoin;
        // mShowCnt = mRealCnt;
        // mTxt.text = string.Format("{0}", mShowCnt);
    }

    public void Show()
    {
        // //mTrigger = 1;
        // Debug.LogFormat("show coin {0}", mRole.mCoin);
        // mRealCnt = mRole.mCoin;
        // mTxt.text = string.Format("{0}", mRole.mCoin);
        // //mTxt.text = "fxxx";
    }

}
