using UnityEngine;

public class ItemCls
{
    public enum Cls
	{
        Coin = 1,
		Chest = 2,
        End,
    }

	public Cls mCls{
		get;
		set;
	}

	public Vector2Int mFace{
		get;
		set;
	}

	public ItemCls(Cls cls) {
		mCls = cls;
	}
}
