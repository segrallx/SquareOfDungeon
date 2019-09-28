using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility {

	// 3d坐标转换为UI坐标
	public static Vector2 PosWorldToUI(Vector3 wpos, RectTransform uiParent)
	{
		var mainCamera = Camera.main;
		var pos = mainCamera.WorldToScreenPoint(wpos);
		Debug.LogFormat("screen point {0}", pos);
		var tmp2d = new Vector2(pos.x, pos.y);
		var ret2d = new Vector2(0,0);
		var uiCamera = mainCamera.GetComponent<CameraFollow>().mUICamera;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(uiParent, tmp2d, uiCamera, out ret2d);
		return ret2d;
	}
}
