using StoryGenerator.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class MoveToTarget : Action
{
	static float DISTANCE_CLOSE = 1*1;
	//position I am trying to go to
	Thing targetThing;
	Vector2 Target { get { return new Vector2(targetThing.X, targetThing.Y); } }

	bool isFinished = false;

	public MoveToTarget(Thing thing)
	{
		this.targetThing = thing;
	}

	public override bool IsFinished
	{

		get { return isFinished; }
	}
	public override void Update(World world, Thing thingAlive, float timeElapsed)
	{
		base.Update(world, thingAlive, timeElapsed);
		float xDiff = thingAlive.X - targetThing.X;
		float yDiff = thingAlive.Y - targetThing.Y;
		if (xDiff * xDiff + yDiff * yDiff < DISTANCE_CLOSE)
		{
			isFinished = true;
			return;
		}
		move(world, thingAlive, timeElapsed);
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
