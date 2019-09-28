using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class RoadPointTest
{
	[Test]
	public void RoadPointTestPushAndLeft()
	{
		var p1 = new RoadPoint(3, 3);
		for(var i=0;i<15;i++)  {
			var dir = p1.GeDirectionUnUsedRandom();
			p1.SetDirectionUsed(dir);
			Debug.Log("dir:"+ dir + " Idx" + p1.Idx + " use:"+p1.DoesHaveLeftDirection());
		}
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

