﻿using StoryGenerator.World;
using StoryGenerator.World.Things.Actors;
using GameEnums;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Carry : Action
{
	Thing_Interactable thingToCarry;
	public Carry(Thing_Interactable thingToGrap):base(Type.CARRY)
	{
		this.name = "Carry";
		this.thingToCarry = thingToGrap;

	}
	public override void Do(World world, ActorBase thing, float timeElapsed)
	{
		base.Do(world, thing, timeElapsed);
		if (thingToCarry.IsBeingCarried && thingToCarry.IsInteractor(thing))
		{
			//I have grapped the item already 
			finish();
			return;
		}

		if (thing.IsCarrying && !thing.AreYouCarrying(thing))
		{
			//thing is carrying something that he/she is not supposed to carry
			thing.TAM.Drop(ThingActionManager.PriorityLevel.FIRST);
			return;
		}

		if (thingToCarry.IsBeingCarried)
		{
			//Thing is being carried and it is not me
			//I cannot proceed
			UnityEngine.Debug.LogError(this + "Cannot be completed");
			finish();
			return;
		}
		float rangeToGrap = thing.GetGrapRange();
		bool isItemWithinGrappableRange = (thingToCarry.XY - thing.XY).magnitude < rangeToGrap;
		if (isItemWithinGrappableRange)
		{
			bool SuccessfullyCarried = false;
			if (!thingToCarry.IsBeingCarried)
			{
				thing.Carry(thingToCarry);
				SuccessfullyCarried = true;
			}
			//Maybe I failed or succeeded in carrying the item
			if (!SuccessfullyCarried)
			{
				UnityEngine.Debug.LogError("Unexpected Carry Result(failure)");
			}
			finish();
			return;
		}
		UnityEngine.Debug.Log(this + "Must move closer " + thing + thing.XY + "/"+ (thingToCarry.XY - thing.XY).magnitude + " " + thingToCarry.Category + " " + thingToCarry.XY);
		thing.TAM.MoveToTarget(this.thingToCarry, rangeToGrap*0.98f, ThingActionManager.PriorityLevel.FIRST);
	}
}
