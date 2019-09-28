using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{

    // 盖子
    public GameObject mLid;

    // 状态
    public enum ChestState
    {
        Close = 0,
        Open = 1,
    }

    //private Vector3 mRotate = new Vector3(5, 0, 0);
    private ChestState mState = ChestState.Close;


    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

		// var x =    mLid.transform.localRotation.eulerAngles.x;
		// if(Mathf.Abs(x - 0) < 1) {
		// 	x = 0;
		// }

		// Debug.LogFormat("z {0}", x);
		// x = x/5*5;
		// if(mState == ChestState.Open) {
		// 	if( (x==0) || (x >270)  ) {
		// 		mLid.transform.Rotate(-mRotate);
		// 	}
		// }else if (mState == ChestState.Close){
		// 	if( x >0 ) {
		// 		mLid.transform.Rotate(mRotate);
		// 	}
		// }

    }

    public void ChestOpen()
    {
        Debug.Log("ChestOpen");
        if (mState == ChestState.Open)
        {
            return;
        }

        mState = ChestState.Open;
		mLid.GetComponent<Animation>().Play("ChestLipOpen");
    }

	public void ChestClose()
    {
        Debug.Log("ChestClose");
    }
}
