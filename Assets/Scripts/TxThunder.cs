using UnityEngine;
using System.Collections.Generic;

// 闪电链条效果

[RequireComponent(typeof(LineRenderer))]
//[ExecuteInEditMode]
public class TxThunder : MonoBehaviour
{
    public float detail = 1;        //增加后，线条数量会减少，每个线条会更长.
    public float displacement = 15; //位移量，也就是线条数值方向偏移的最大量.
    public Transform EndPosition;
    public Transform StartPosition;
    public float yOffset = 0;
    private LineRenderer _lineRender;
    private List<Vector3> _linePosList;

    private void Awake()
    {
        _lineRender = GetComponent<LineRenderer>();
        _linePosList = new List<Vector3>();
    }

    private void Update()
    {
        if (Time.timeScale != 0)
        {
            _linePosList.Clear();
            Vector3 startPos = Vector3.zero;
            Vector3 endPos = Vector3.zero;
            if (EndPosition != null)
            {
                endPos = EndPosition.position + Vector3.up * yOffset;
            }

            if (StartPosition != null)
            {
                startPos = StartPosition.position + Vector3.up * yOffset;
            }

            collLinePos(startPos, endPos, displacement);
            _linePosList.Add(endPos);

            _lineRender.positionCount = (_linePosList.Count);
            for (int i = 0; i < _lineRender.positionCount; i++)
            {
                _lineRender.SetPosition(i, _linePosList[i]);
            }
        }
    }

    // 收集顶点，中点分形法插值抖动
    private void collLinePos(Vector3 startPos, Vector3 endPos, float displace)
    {
        if (displace < detail)
        {
            _linePosList.Add(startPos);
        }
        else
        {
            float midX = (startPos.x + endPos.x) / 2;
            float midY = (startPos.y + endPos.y) / 2;
            float midZ = (startPos.z + endPos.z) / 2;

            midX += (float)(UnityEngine.Random.value - 0.5) * displace;
            midY += (float)(UnityEngine.Random.value - 0.5) * displace + 1f;
            midZ += (float)(UnityEngine.Random.value - 0.5) * displace;

            var midPos = new Vector3(midX, midY, midZ);
            collLinePos(startPos, midPos, displace / 2);
            collLinePos(midPos, endPos, displace / 2);
        }

    }


}
