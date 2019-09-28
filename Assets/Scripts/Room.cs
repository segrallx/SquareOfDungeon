using UnityEngine;
using System;

public class Room
{
    public RectInt mRect;

    public Room(RectInt rec)
    {
        mRect = rec;
    }

    public string Info()
    {
         return String.Format("Room x:{0} y:{1} w:{2} h:{3}\n", mRect.x, mRect.y, mRect.width, mRect.height);
    }

    // 计算两个正方形是否重叠
    public bool IsOverlap(Room r1)
    {
        var rc1 = this.mRect;
        var rc2 = r1.mRect;

        var minx = Mathf.Max(Mathf.Min(rc1.x-1, rc1.x + rc1.width+1), Mathf.Min(rc2.x-1, rc2.x + rc2.width+1));
        var miny = Mathf.Max(Mathf.Min(rc1.y-1, rc1.y + rc1.height+1), Mathf.Min(rc2.y-1, rc2.y + rc2.height+1));
        var maxx = Mathf.Min(Mathf.Max(rc1.x-1, rc1.x + rc1.width+1), Mathf.Max(rc2.x-1, rc2.x + rc2.width+1));
        var maxy = Mathf.Min(Mathf.Max(rc1.y-1, rc1.y + rc1.height+1), Mathf.Max(rc2.y-1, rc2.y + rc2.height+1));

        if (minx > maxx || miny > maxy)
        {
            return false;
        }

        return true;
    }


    // 计算两个正方形是否重叠
    public bool IsInside(int width , int height)
    {
        var rc1 = this.mRect;

		if(rc1.x <1 || (rc1.x + rc1.width) > width) {
			return false;
		}

		if(rc1.y <1 || (rc1.y+rc1.height) > height) {
			return false;
		}

        return true;
    }


    // 随机边缘点.
    public Vector3Int RandomEdge()
    {
        int edgeX;
        int edgeY;
        int edgeSide;

        if (0 == UnityEngine.Random.Range(0, 100) % 2)
        {
            if (0 == UnityEngine.Random.Range(0, 100) % 2)
            {
                edgeX = mRect.x + mRect.width;
            }
            else
            {
                edgeX = mRect.x;
            }

            edgeY = UnityEngine.Random.Range((int)mRect.y, (int)(mRect.y + mRect.height + 1));
            edgeSide = 1;
        }
        else
        {
            if (0 == UnityEngine.Random.Range(0, 100) % 2)
            {
                edgeY = mRect.y + mRect.height;
            }
            else
            {
                edgeY = mRect.y;
            }
            edgeX = UnityEngine.Random.Range((int)mRect.x, (int)(mRect.x + mRect.width + 1));
            edgeSide = 2;
        }
        return new Vector3Int((edgeX), (edgeY), edgeSide);
    }

    // 获取org边缘以外X个单位的点
    public Vector3Int OutEdgePoint(Vector3Int org, int unit)
    {
        int outX;
        int outY;
        var middleX = (mRect.x * 2 + mRect.width) / 2;
        var middleY = (mRect.y * 2 + mRect.height) / 2;

        if (org.z == 1)
        {
            if (org.x - middleX > 0)
            {
                outX = org.x + unit;
            }
            else
            {
                outX = org.x - unit;
            }

            return new Vector3Int(outX, org.y, 0);
        }
        else
        {
            if (org.y - middleY > 0)
            {
                outY = org.y + unit;
            }
            else
            {
                outY = org.y - unit;
            }

            return new Vector3Int(org.x, outY, 0);
        }
    }
}
