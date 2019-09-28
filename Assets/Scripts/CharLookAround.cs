using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CharLookAround : MonoBehaviour
{
    private float mLookRange = 10.0f;
    private float mLookAngle = 180.0f;
    private int mLookAccurate = 20;
    private Color mColor = Color.green;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public float RangeGet()
    {
        return mLookRange;
    }


    // 设置修改默认参数
    public void SetParams(float lookRange, float lookAngle, int lookAccurate, Color color)
    {
        mLookRange = lookRange;
        mLookAngle = lookAngle;
        mLookAccurate = lookAccurate;
        mColor = color;
    }

    // 实际检查线.
    private GameObject LookAroundRay(Quaternion eulerAnger, Vector3 forward, string tag, Color color)
    {
        Vector3 Pos = transform.position + new Vector3(0, 1.0f, 0);
        //Debug.LogFormat("forward {0} {1}", forward, transform.forward.normalized);
        Debug.DrawRay(Pos, eulerAnger * (forward.normalized) * mLookRange, color);

        RaycastHit hit;
        if (Physics.Raycast(Pos, eulerAnger * (forward.normalized) * mLookRange, out hit, mLookRange))
        {

            if (hit.collider.tag == tag)
            {
                //Debug.LogFormat("meet {0}", new object[] { tag });
                return hit.collider.gameObject;
            }
        }

        return null;
    }

    // 返回检查到的Player
    public GameObject LookAround(string tag, Vector3 forward, Color color)
    {
        var gObj = LookAroundRay(Quaternion.identity, forward, tag, color);
        if (gObj != null)
        {
            return gObj;
        }

        float subAngle = (mLookAngle / 2) / mLookAccurate;

        for (int i = 0; i < mLookAccurate; i++)
        {
            gObj = LookAroundRay(Quaternion.Euler(0, -1 * subAngle * (i + 1), 0), forward, tag, color);
            if (gObj != null)
            {
                return gObj;
            }

            gObj = LookAroundRay(Quaternion.Euler(0, subAngle * (i + 1), 0), forward, tag, color);
            if (gObj != null)
            {
                return gObj;
            }
        }

        return null;
    }

}
