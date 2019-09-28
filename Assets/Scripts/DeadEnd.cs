using UnityEngine;

public class DeadEnd
{
	public int mX;
	public int mY;

	public static DeadEnd zero = new DeadEnd(0,0,0,0);

	// 所面向的空白方向.
	public Vector2Int mFace;

	public DeadEnd(int x,int y,int i,int j) {
		mX = x;
		mY = y;
		mFace= new Vector2Int(i,j);
	}
}
