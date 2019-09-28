using UnityEngine;
using System;
using System.Collections.Generic;

public class Astar
{

    public class Point
    {
        public int x;
        public int y;
        public int pX;
        public int pY;
        public int G;
        public int H;

        public Point(Vector2Int p)
        {
            x = p.x;
            y = p.y;
            pX = -1;
            pY = -1;
            G = 0;
            H = 0;
        }

        public Point(int xI, int yI)
        {
            x = xI;
            y = yI;
            pX = -1;
            pY = -1;
            G = 0;
            H = 0;
        }


        public Point()
        {
            x = -1;
            y = -1;
            pX = -1;
            pY = -1;
            G = -1;
            H = 1;
        }

        public int F()
        {
            return G + H;
        }


        public bool CompareTo(Point p)
        {
            if (p.x == x && p.y == y)
            {
                return true;
            }
            return false;
        }


        public int ManHanttanDistance(Point p)
        {
            return Mathf.Abs(x - p.x) + Mathf.Abs(y - p.y);
        }

    }

    // 通过算法寻找一条通路，如果返回长度为0则不可达.
    public static List<Vector2Int> FindPath(Maze maze, Vector2Int startPoint, Vector2Int endPoint, Char c)
    {
        List<Vector2Int> ret = new List<Vector2Int>();
        List<Point> openList = new List<Point>();
        List<Point> closeList = new List<Point>();

        // Debug.LogFormat("start point x:{0} y:{1}", startPoint.x, startPoint.y);
        // Debug.LogFormat("end point x:{0} y:{1}", endPoint.x, endPoint.y);

        var starP = new Point(startPoint);
        var endP = new Point(endPoint);
        starP.G = 0;
        starP.H = starP.ManHanttanDistance(endP);
        openList.Add(starP);
        Point curPoint;
        Point pathPoint = new Point(-1, -1);

        Vector2Int[] direction = new Vector2Int[4];
        direction[0] = new Vector2Int(0, 1);
        direction[1] = new Vector2Int(0, -1);
        direction[2] = new Vector2Int(1, 0);
        direction[3] = new Vector2Int(-1, 0);

        var match = false;

        int x = 0;
        while (FindNextPointInOpenList(openList, out curPoint))
        {
            // Debug.LogFormat("cur point x:{0} y:{1}", curPoint.x, curPoint.y);

            for (int i = 0; i < 4; i++)
            {
                x += 1;
                int nextX = curPoint.x + direction[i].x;
                int nextY = curPoint.y + direction[i].y;

                Point nextPoint = new Point(nextX, nextY);
                nextPoint.pX = curPoint.x;
                nextPoint.pY = curPoint.y;

                // Debug.LogFormat("try  point x:{0} y:{1}", nextX, nextX);

                if (!maze.CheckPosValid(nextX, nextY))
                {
                    // Debug.LogFormat("skip not valid x:{0} y:{1}", nextX, nextY);
                    continue;
                }

                if (!maze.CheckMoveAble(nextX, nextY, c.mState) && !nextPoint.CompareTo(endP))
                {
                    // Debug.LogFormat("skip not move able x:{0} y:{1}", nextX, nextY);
                    continue;
                }

                if (FindInList(closeList, nextPoint) != -1)
                {
                    // Debug.LogFormat("skip in closelist x:{0} y:{1}", nextX, nextY);
                    continue;
                }

                var openIdx = (FindInList(openList, nextPoint));
                if (openIdx != -1)
                {
                    var openP = openList[openIdx];
                    if (openP.G > (curPoint.G + 10))
                    {
                        openP.pX = curPoint.x;
                        openP.pY = curPoint.y;
                        openP.G = curPoint.G + 10;
                    }
                    // Debug.LogFormat("skip in openlist x:{0} y:{1}", nextX, nextY);
                    continue;
                }


                nextPoint.G = curPoint.G + 10;
                nextPoint.H = curPoint.ManHanttanDistance(endP);
                if (nextPoint.CompareTo(endP))
                {
                    pathPoint = curPoint;
                    match = true;
                    // Debug.LogFormat("match the end !");
                    break;
                }
                // Debug.LogFormat("add to openlist x:{0} y:{1}", nextX, nextY);
                openList.Add(nextPoint);
            }


            if (match)
            {
                break;
            }
            else
            {
                // Debug.LogFormat("add to close x:{0} y:{1}", curPoint.x, curPoint.y);
                closeList.Add(curPoint);
            }


            if (x > 1000)
            {
                // Debug.LogError("find path too long");
                return ret;
            }

        }

        if (!match)
        {
            // Debug.LogFormat("No point in openlist");
            return ret;
        }


        while (!pathPoint.CompareTo(starP))
        {
            ret.Add(new Vector2Int(pathPoint.x, pathPoint.y));
            // Debug.LogFormat("pathpoint x:{0} y:{1} px:{2} py:{3}", pathPoint.x, pathPoint.y, pathPoint.pX, pathPoint.pY);

            pathPoint.x = pathPoint.pX;
            pathPoint.y = pathPoint.pY;
            //var fOpenIdx  = FindInList(openList, pathPoint);
            var fCloseIdx = FindInList(closeList, pathPoint);
            // if (fOpenIdx!=-1) {
            // 	pathPoint = openList[fOpenIdx];
            // 	openList.RemoveAt(fOpenIdx);
            // }else if  (fCloseIdx!=-1) {
            if (fCloseIdx != -1)
            {
                pathPoint = closeList[fCloseIdx];
                closeList.RemoveAt(fCloseIdx);
            }
            else
            {
                throw new Exception("should noe be here");
            }

        }

        ret.Reverse();
        return ret;
    }

    public static bool FindNextPointInOpenList(List<Point> openList, out Point point)
    {
        if (openList.Count == 0)
        {
            point = new Point();
            return false;
        }

        int minDistance = 1000000;
        int matchIdx = -1;
        for (int i = 0; i < openList.Count; i++)
        {
            var pointTmp = openList[i];
            if (pointTmp.F() < minDistance)
            {
                matchIdx = i;
                minDistance = pointTmp.F();
            }
        }

        point = openList[matchIdx];
        openList.RemoveAt(matchIdx);
        return true;
    }

    // 检查list里面是否有某个点.
    public static int FindInList(List<Point> closeList, Point point)
    {
        for (int i = 0; i < closeList.Count; i++)
        {
            if (closeList[i].CompareTo(point))
            {
                return i;
            }
        }
        return -1;
    }
}
