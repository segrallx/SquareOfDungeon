using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //transform.Rotate(new Vector3(0, 5, 0));
    }

	public void FlipStart()
	{
		// Debug.Log("xxxx");
		// GetComponent<Animation>().Play();
		// GetComponent<AudioSource>().Play();
	}

	public void FlipBegin()
	{
		Debug.Log("FlipBegin");
	}

	public void FlipEnd()
	{
		Debug.Log("FlipEnd");
		//Destroy(transform.parent.gameObject);
	}

	public void CoinGet()
	{
		Destroy(gameObject);
		var rssEffect = GameObject.FindGameObjectWithTag("RssEffect").GetComponent<RssEffect>();
		rssEffect.PlayCoin(transform.position, 1);
	}


}
