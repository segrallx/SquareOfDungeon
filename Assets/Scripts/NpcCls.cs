using UnityEngine;
using System;

[System.Serializable]
public class NpcCls
{

	public int mId;
	public Direction mFace;

	public NpcCls(int id) {
		mId = id;
	}

	public override string ToString() {
		return String.Format("id {0}", mId);
	}

}
