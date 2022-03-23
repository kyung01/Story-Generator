using StoryGenerator.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class MoveToTarget : Action
{
	//position I am trying to go to
	Thing targetThing;
	Vector2 Target { get { return new Vector2(targetThing.X, targetThing.Y); } }

	bool isFinished = false;
	float distanceMinToDestination;

	public MoveToTarget(Thing targetThing, float distanceMinToDestination = 1)
	{
		this.name = "MoveToTarget";
		this.targetThing = targetThing;
		this.distanceMinToDestination = distanceMinToDestination;
	}

	public override void Update(World world, Thing thingThis, float timeElapsed)
	{
		Debug.Log(this + " " + targetThing.XY + " " +  thingThis.XY);
		base.Update(world, thingThis, timeElapsed);
		float xDiff = thingThis.X - targetThing.X;
		float yDiff = thingThis.Y - targetThing.Y;
		if ((thingThis.XY - Target).magnitude <= distanceMinToDestination)
		{
			finish();
			return;
		}
		Debug.Log(this + " Is Finished  " + (xDiff * xDiff + yDiff * yDiff < ZEROf_SQUARE));
		move(world, thingThis, timeElapsed);
	}

	private void move(World world, Thing thing, float timeElapsed)
	{
		//I am going to move thing to the target location
		float speed = world.GetThingsSpeed(thing);
		float maxDistanceToMove = (thing.XY - Target).magnitude;
		float distanceApplied = Mathf.Min(speed * timeElapsed, maxDistanceToMove);
		var thingsNewPosition = thing.XY + (Target - thing.XY).normalized * distanceApplied;
		thing.XY = thingsNewPosition;
	}
}
