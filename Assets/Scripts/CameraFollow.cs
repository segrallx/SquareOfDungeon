using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	public Transform mPlayer;  //把运动物体拖放到此处
	private Vector3 mOffset;
	public Camera mUICamera;


	// Use this for initialization
	void Start () {
		Debug.Log("CameraFollow start");
		mOffset = mPlayer.position - transform.position;
	}

	void Awake() {
		Debug.Log("CameraFollow awake");
	}

	void OnDisable() {
		Debug.Log("OnDisable");
	}

	void OnEnable() {
		Debug.Log("OnEnable");
		mOffset = mPlayer.position - transform.position;
	}


	// Update is called once per frame
	void Update () {
		// transform.position = Vector3.Lerp (transform.position, mPlayer.position - mOffset, Time.deltaTime * 5f);
		// Quaternion rotation = Quaternion.LookRotation (mPlayer.position - transform.position);
		// transform.LookAt(mPlayer);

		//transform.position = Vector3.Lerp (transform.position, mPlayer.position - mOffset, Time.deltaTime * 10f);

		transform.position = mPlayer.position - mOffset;
		Quaternion rotation = Quaternion.LookRotation (mPlayer.position - transform.position);
		transform.rotation = rotation;

		//var s = Screen.orientation;
		//Debug.Log(s);
	}
}
