using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorColor : MonoBehaviour {

	private Color mOldColor = Color.black;
	//public Tiled mTiled;

    // Use this for initialization
    void Awake()
    {
		mOldColor = GetComponent<MeshRenderer>().material.color;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SetColor(Color color)
    {
        var mat = GetComponent<MeshRenderer>().material;
        mat.color = color;
    }


    public void RecoverColor()
    {
		var mat = GetComponent<MeshRenderer>().material;
		mat.color = mOldColor;
    }
}
