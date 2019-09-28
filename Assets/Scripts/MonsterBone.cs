using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MonsterBone : MonoBehaviour {

	public MonsterRender mRender;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter(Collider coll)
    {
		mRender.OnTriggerEnter(coll);
	}

}
