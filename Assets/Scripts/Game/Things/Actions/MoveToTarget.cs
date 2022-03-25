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
	Vector2 nextDestinationXY { get { return pathRegistered[0]; } }
	Vector2 destinationXY { get { return targetThing.XY; } }

	bool isFinished = false;
	float distanceMinToDestination;
	bool updateNewPath = true;
	List<Vector2> pathRegistered = new List<Vector2>();

	public MoveToTarget(Thing targetThing, float distanceMinToDestination = 1)
	{
		this.name = "MoveToTarget";
		this.targetThing = targetThing;
		this.distanceMinToDestination = distanceMinToDestination;
	}
	

	public override void Update(World world, Thing thingAlive, float timeElapsed)
	{
		base.Update(world, thingAlive, timeElapsed);
		if (updateNewPath)
		{
			pathRegistered = new List<Vector2>();
			var path = world.pathFinder.getPath(thingAlive.XY_Int, destinationXY);
			while (path != null)
			{
				pathRegistered.Add(path);
				path = path.after;
			}
			updateNewPath = false;

			if(pathRegistered.Count == 0)
			{
				//Failed to find a new path
				finish();
				return;
			}
		}

		Debug.Log("AvailablePaths " + pathRegistered.Count);
		

		move(world, thingAlive, timeElapsed);
		var diff = thingAlive.XY - pathRegistered[0];
		if (diff.magnitude < ZEROf)
		{
			pathRegistered.RemoveAt(0);
		}
		if ((thingAlive.XY - destinationXY).magnitude < distanceMinToDestination || pathRegistered.Count == 0)
		{
			finish();
			return;
		}
	}

	private void move(World world, Thing thing, float timeElapsed)
	{

		//I am going to move thing to the target location
		float speed = world.GetThingsSpeed(thing);
		float maxDistanceToMove = (thing.XY - nextDestinationXY).magnitude;
		float distanceApplied = Mathf.Min(speed * timeElapsed, maxDistanceToMove);
		var thingsNewPosition = thing.XY + (nextDestinationXY - thing.XY).normalized * distanceApplied;
		thing.XY = thingsNewPosition;
	}
}
