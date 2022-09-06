using StoryGenerator.World;
using StoryGenerator.World.Things.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class SleepAction : Action
{
	bool isZero(float n)
	{
		return Mathf.Abs(n) < 0.001f;
	}
	ISleepableStructure bedish;
	public SleepAction(ISleepableStructure bedish) : base(Type.SLEEP)
	{
		this.name = "Sleep (Action)";
		this.bedish = bedish;

	}
	public override void Do(World world, ActorBase thing, float timeElapsed)
	{
		base.Do(world, thing, timeElapsed);
		Debug.Log("Action SLeep");
		var positions = 	bedish.GetSleepablePositions(world, thing);
		bool IAMAtWhereICanSleepIn = isZero((thing.XY - positions[0]).magnitude);
		if (!IAMAtWhereICanSleepIn)
		{
			Debug.Log("Action SLeep IAM NOT WHERE to sleep");
			thing.TAM.MoveTo(positions[0], ThingActionManager.PriorityLevel.FIRST);
			return;
		}
		//I am at position to sleep now!

		if (!bedish.IsSleepable(world, thing))
		{
			//WHAT?! BED IS NOT SLEEPABLE ANTMORE!!!!!!!!
			this.finish();
			Debug.Log("Sleep Action failing because available bed is no longer availalbe");
			return;
		}
		//I can sleep!

		if(bedish.SleepBy(world, thing))
		{
			Debug.Log("Action SleepBy");

			thing.DoDream(world);
			finish();

			

		}
		else
		{
			//well I failed to sleep need to get angry
			Debug.Log("Failed to sleep");
			finish();
		}


	}
}
