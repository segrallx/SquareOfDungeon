using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcRender : CharRender
{
    private CharLookAround charLookAround;
    private float mLookAroundSleep;
    private GameObject mLookAroundTarget;

    public void Start()
    {
        charLookAround = GetComponent<CharLookAround>();
        charLookAround.SetParams(10, 120, 7, Color.green);
    }


    public override void FixedUpdate()
    {
        if (mChar == null)
        {
            return;
        }

        base.FixedUpdate();

        mLookAroundSleep -= Time.deltaTime;
        if (mLookAroundSleep < 0)
        {
            var role = charLookAround.LookAround("Player", mChar.DirectionForward(), Color.green);
            mLookAroundSleep = -0.2f;

            if (role != null && mLookAroundTarget == null)
            {
                Debug.LogFormat("find role {0}", role);
                mLookAroundTarget = role;
            }// else{
             // 	mLookAroundTarget = null;
             // }
        }
    }


    public override void Update()
    {
        base.PositionUpdate();

        if (mLookAroundTarget != null)
        {
            RotationUpdateToTarget();
        }
        else
        {
            base.RotationUpdate(); ;
        }
    }


    // 方向更新
    void RotationUpdateToTarget()
    {
        var x = mLookAroundTarget.transform.position - transform.position;
        if (x == Vector3.zero)
        {
            return;
        }

        var dis = Vector3.Distance(mLookAroundTarget.transform.position, transform.position);
        if (dis > 10)
        {
            Debug.LogFormat("distance {0}", dis);
            mLookAroundTarget = null;
        }

        var fromVector = new Vector3(1, 0, 0);
        var y = Vector3.Angle(fromVector, x);
        Vector3 normal = Vector3.Cross(fromVector, x);
        y *= Mathf.Sign(Vector3.Dot(normal, new Vector3(0, 1, 0)));

        if (y == 0)
        {
            return;
        }

        var target = Quaternion.Euler(0, y, 0);
        //Debug.LogFormat("angle {0} y {1}", Quaternion.Angle(transform.rotation, target), y);
        if (Mathf.Abs(Quaternion.Angle(transform.rotation, target)) < 4)
        {
            transform.rotation = target;
        }
        else
        {
            transform.rotation = Quaternion.Lerp(transform.rotation,
                                                 Quaternion.Euler(0, y, 0), Time.deltaTime * 6);
        }
    }



    public override void OnTriggerEnter(Collider coll)
    {
        base.OnTriggerEnter(coll);
    }




}
