using UnityEngine;
using StoryGenerator.World;
using System.Collections.Generic;
using System;

public class Howl :Work
{
	public Thing thingToHowl;
	public Vector2 positionToHowlTo;
	public int positionToHowlToXInt { get { return Mathf.RoundToInt(positionToHowlTo.x); } }
	public int positionToHowlToYInt { get { return Mathf.RoundToInt(positionToHowlTo.y); } }

	public Howl(Thing assignedWorker, Thing thingToHowl, Vector2 positionToHowlTo) : base(assignedWorker)
	{
		this.thingToHowl = thingToHowl;
		this.positionToHowlTo = positionToHowlTo;

	}
	bool isItemAtCorrectPosition()
	{
		return (thingToHowl.XY - positionToHowlTo).magnitude < ZEROf;
	}

	public override void Do(World world, float timeElapsed)
	{
		base.Do(world, timeElapsed);
		//check fail conditions
		bool isItemSpotAlreadyOccupied = false;
		if(this.thingToHowl is Item)
		{
			if(!getConditionForItem(world))
			{
				//There was something wrong
				//I cannot proceed
				return;
			}
		}
		var isThingAtRightPosition = (thingToHowl.XY - positionToHowlTo).magnitude < ZEROf;
		var isThingDropped = thingToHowl.IsBeingCarried;

		bool conditionToCompleteTheJob = isThingAtRightPosition && isThingDropped;
		if (conditionToCompleteTheJob)
		{
			finish();
			return;
		}
		if (thingToHowl.Carrier != this.assignedWorker)
		{
			//worker must get that thing first
			this.assignedWorker.TAM.Carry(thingToHowl);
			return;
		}
		//ThingToHowl's carrier is assignedWorker, then...
		if (!isThingAtRightPosition)
		{
			this.assignedWorker.TAM.MoveTo(positionToHowlTo);
			return;
		}
		//Thing's carrier is assigned worker and thing is at the right position
		this.assignedWorker.TAM.Drop();
		

	}


	private bool getConditionForItem(World world)
	{
		List<Thing> thingsAt = world.GetThingsAt(positionToHowlToXInt, positionToHowlToYInt);
		var items = getItemsOnTheGround(thingsAt);
		if (items.Count != 0)
		{
			//There are items on the ground. Find out ther reason why
			for (int i = 0; i < items.Count; i++)
			{
				if (items[i] == thingToHowl)
				{
					//Item is already on the ground I can finish the job
					finish();
					return false;
				}
			}
			//I muust move the items[i] to somewhere else
			return false;
		}
		//There is no item on the ground I need to care about
		return true;
	}

	private List<Thing> getItemsOnTheGround(List<Thing> thingsAt)
	{
		List<Thing> items = new List<Thing>();
		for (int i = 0; i< thingsAt.Count; i++)
		{
			if (!(thingsAt[i] is Item)) continue;
			if (thingsAt[i].IsBeingCarried) continue;
			items.Add(thingsAt[i]);
		}
		return items;
	}
}
