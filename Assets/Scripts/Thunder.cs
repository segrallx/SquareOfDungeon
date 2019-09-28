using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Thunder : MonoBehaviour
{

    private LineRenderer lineRenderer;
    private List<Transform> mTransfromList;
    private float mKeepTime;

    void Start()
    {
        //添加LineRenderer组件
        // lineRenderer = gameObject.AddComponent<LineRenderer>();
        // //设置材质
        // lineRenderer.material = new Material(Shader.Find("Custom/Additive"));
        // //设置颜色
        // lineRenderer.startColor = Color.red;
        // lineRenderer.endColor = Color.yellow;

        lineRenderer = GetComponent<LineRenderer>();
    }

    public void SetPointList(List<Transform> list, float keepTime)
    {
        mTransfromList = list;
        mKeepTime = keepTime;
    }

    void Update()
    {
        if (mTransfromList == null)
        {
            Destroy(gameObject);
            return;
        }

        mKeepTime -= Time.deltaTime;
        if (mKeepTime < 0)
        {
            Destroy(gameObject);
            return;
        }

        lineRenderer.positionCount = mTransfromList.Count;
        var j = 0;
        foreach (var trans in mTransfromList)
        {
            var x = 0.2f;
            if (trans == null || !trans.gameObject.activeSelf)
            {
                continue;
            }
            var pos = trans.position;
            lineRenderer.SetPosition(j, new Vector3(pos.x + Random.Range(-x, x),
                                                    pos.y + Random.Range(-x, x),
                                                    pos.z + Random.Range(-x, x)));
            j++;
        }

    }



}

