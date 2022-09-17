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
	ISleepableStructure bedAssigned;
	public SleepAction(ISleepableStructure bedish) : base(Type.SLEEP)
	{
		this.name = "Sleep (Action)";
		this.bedAssigned = bedish;

	}
	public override void Do(World world, ActorBase thing, float timeElapsed)
	{
		base.Do(world, thing, timeElapsed);
		Debug.Log("Action Sleep DO");
		var positions = 	bedAssigned.GetSleepablePositions(world, thing);

		if(positions.Count == 0)
		{
			Debug.Log("Bed cannot provide sleepable position");
			finish();

			return;
		}

		bool IAmAtWhereICanSleepIn = isZero((thing.XY - positions[0]).magnitude);

		if (!IAmAtWhereICanSleepIn)
		{
			//Actor must move to position near the bed
			//Debug.Log("Action SLeep IAM NOT WHERE to sleep");
			thing.TAM.MoveTo(positions[0], ThingActionManager.PriorityLevel.FIRST);
			//return;
		}
		else if (!bedAssigned.IsSleepable(world, thing))
		{
			//Actor is at the sleepable bed-accessing position but the bed is no longer available
			this.finish();
			Debug.Log("Sleep Action failing because available bed is no longer availalbe");
			//return;
		}
		else if(bedAssigned.SleepBy(world, thing))
		{
			//Successfully slept 
			Debug.Log("Action SleepBy");
			thing.DoDream(world);
			finish();
		}
		else
		{
			//well I failed to sleep need to get angry
			Debug.LogError("Unexpected Result");
			finish();
		}


	}
}
