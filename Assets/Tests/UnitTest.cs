using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class UnitTest
{
	[Test]
	public void UnitTestNearestCenter()
	{
		//Use the Assert class to test conditions.
		Vector3 v1 = new Vector3(7.1f, 0f, 2f);
		var v1_x = Unit.NearestCenter(v1);
		Assert.AreEqual(8.0f, v1_x.x);
		Assert.AreEqual(2.0f, v1_x.z);
	}

	[Test]
	public void UnitTestNextTileIdx()
	{
		Vector3 v1 = new Vector3(7.1f, 0f, 2f);
		var next = Unit.NextTiledIdx(v1, new Vector3(0,0,1));
		Assert.AreEqual(4, next.x);
		Assert.AreEqual(2, next.y);
	}

	[Test]
	public void UnitTileIdx()
	{
		//Use the Assert class to test conditions.
		Vector3 v1;
		Vector2Int v1_x ;
		v1 = new Vector3(7.1f, 0f, 2.1f);
		v1_x = Unit.TileIdx(v1);
		Assert.AreEqual(4, v1_x.x);
		Assert.AreEqual(1, v1_x.y);

		v1 = new Vector3(7.9f, 0f, 3.1f);
		v1_x = Unit.TileIdx(v1);
		Assert.AreEqual(4, v1_x.x);
		Assert.AreEqual(2, v1_x.y);

		v1 = new Vector3(6.9f, 0f, 2.9f);
		v1_x = Unit.TileIdx(v1);
		Assert.AreEqual(3, v1_x.x);
		Assert.AreEqual(1, v1_x.y);

		v1 = new Vector3(6.1f, 0f, 3.4f);
		v1_x = Unit.TileIdx(v1);
		Assert.AreEqual(3, v1_x.x);
		Assert.AreEqual(2, v1_x.y);


	}



	// A UnityTest behaves like a coroutine in PlayMode
	// and allows you to yield null to skip a frame in EditMode
	[UnityTest]
	public IEnumerator UnitTestWithEnumeratorPasses()
	{
		// Use the Assert class to test conditions.
		// yield to skip a frame
		yield return null;
	}
}

