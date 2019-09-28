using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class RoomTest
{
	[Test]
	public void RoomGenShow()
	{
		var r1 = new Room(new RectInt(3, 5, 3, 3));
		for(var i=0;i<10;i++) {
			var edge = r1.RandomEdge();
			Debug.Log("edget -> x:" + edge.x + " y:"+ edge.y);
		}
	}

	[Test]
	public void RoomOverLap()
	{
		var r1 = new Room(new RectInt(0, 0, 3, 3));
		var r2 = new Room(new RectInt(4, 4, 1, 1));
		Debug.Log("overlaps:"+r1.IsOverlap(r2));
	}


	// A RoomyTest behaves like a coroutine in PlayMode
	// and allows you to yield null to skip a frame in EditMode
	[UnityTest]
	public IEnumerator RoomTestWithEnumeratorPasses()
	{
		// Use the Assert class to test conditions.
		// yield to skip a frame
		yield return null;
	}
}

