using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITalentTrigger : MonoBehaviour
{
	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	void OnTriggerEnter(Collider coll) {
		Debug.Log("uitalent enter" + coll.ToString() + " " + coll.tag);
	}

	void OnTriggerExit(Collider coll) {
		Debug.Log("uitalent exit" + coll.ToString() + " " + coll.tag);
	}
}
