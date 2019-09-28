using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Adsorb : MonoBehaviour
{
    private float mSleepTime = 0.5f;
    private GameObject mDest;
    public delegate void Event();
    private event Event mEvent;

    void Update()
    {
        Debug.LogFormat("update ");
        mSleepTime -= Time.deltaTime;
        if (mSleepTime < 0 && mDest != null)
        {
            var curPos = gameObject.transform.position;
            var desPos = mDest.transform.position;
            if (Vector3.Distance(curPos, desPos) < 0.7f)
            {
                Destroy(gameObject);
                if (mEvent != null)
                {
                    mEvent();
                }
            }
            else
            {
                gameObject.transform.position = Vector3.Lerp(curPos, desPos, 0.2f);
            }
        }
    }

    public void SetDest(GameObject obj, Event t)
    {
        mDest = obj;
        mEvent += t;
    }

}
