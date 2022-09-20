using StoryGenerator.World;
using StoryGenerator.World.Things.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Bed : StructureWithInteractSpots, ISleepableStructure
{
	
	public Bed(CAIModel model) : base(GameEnums.ThingCategory.BED, model)
	{
	}

	public virtual List<Vector2> GetSleepAccessiblePositions(World world, ActorBase actor)
	{
		return this.GetAvailableInteractionSpot(world, this.spotsToEnter.ToArray());
	}

	public virtual bool IsSleepable(World world, ActorBase thingTryingToSleepIn)
	{
		foreach(var spot in spotsToEnter)
		{
			if (spot.IsAvailableForConsideration(world, this)) return true;

		}
		return false;
	}

	public virtual bool SleepBy(World world, ActorBase sleepingAgent)
	{
		for(int i  = 0; i < spotsToEnter.Count; i++)
		{
			int x, y;
			spotsToEnter[i].GetInteractionXY(this, out x, out y);

			Debug.Log("attempting to sleep at " + x + y);
			if (spotsToEnter[i].CanInteractWithIt(world, this, sleepingAgent))
			{
				spotsToEnter[i].Interact(this, sleepingAgent);
				sleepingAgent.SetInteractor(this);

				return true;
			}
		}
		Debug.LogError("Bed unexpected end of function reached " + spotsToEnter.Count);
		return false;
	}




}