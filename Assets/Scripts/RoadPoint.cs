using UnityEngine;
using System;
using System.Collections.Generic;

public struct RoadPoint
{
    public int X;
    public int Y;
    public int[] Dirs;
    public int Idx;

    public RoadPoint(int x, int y)
    {
        X = x;
        Y = y;
        Idx = 0;
        Dirs = new int[5]{0,0,0,0,0};
    }

	public bool DoesHaveLeftDirection()
	{
		return Idx<4;
	}

    // 设置一个方向被使用
    public void SetDirectionUsed(int x)
    {
		if(x<0){
			return;
		}

        if (Dirs[x] == 0)
        {
            Idx++;
            Dirs[x] = 1;
        }
        else
        {
            Debug.LogError("should not be here");
            //throw new Exception("should not be here");
        }
    }

    // 随机获取一个可用的未使用方向
    public int GeDirectionUnUsedRandom()
    {
        if (Idx > 4)
        {
            return -1;
        }

        var left = new int[4];
        var size = 0; // 4个备选.
        for (var i = 1; i <= 4; i++)
        {
			if (Dirs[i] == 0)
            {
				left[size]=i;
				size++;
            }
        }

        var id = UnityEngine.Random.Range(0, size);
        return left[id];
    }
}
