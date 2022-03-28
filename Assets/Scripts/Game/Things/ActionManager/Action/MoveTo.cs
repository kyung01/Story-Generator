using StoryGenerator.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class MoveTo : Action
{
	//position I am trying to go to
	public virtual Vector2 destinationXY { get { return new Vector2(); } }
	Vector2 nextDestinationXY { get { return pathRegistered[0]; } }

	bool updateNewPath = true;
	List<Vector2> pathRegistered = new List<Vector2>();

	public MoveTo()
	{
		this.name = "MoveTo";
	}

	public override void Do(World world, Thing thing, float timeElapsed)
	{
		base.Do(world, thing, timeElapsed);
		if (updateNewPath)
		{
			pathRegistered = new List<Vector2>();
			var path = world.pathFinder.getPath(thing.XY_Int, destinationXY);
			while (path != null)
			{
				pathRegistered.Add(path);
				path = path.after;
			}
			updateNewPath = false;
		}
		if(pathRegistered.Count == 0)
		{
			finish();
			return;
		}

		//Debug.Log("AvailablePaths " + pathRegistered.Count);


		move(world, thing, timeElapsed);
		var diff = thing.XY - pathRegistered[0];
		if (diff.magnitude < ZEROf)
		{
			pathRegistered.RemoveAt(0);
		}

		if (IsDestinationReached(world, thing) || pathRegistered.Count == 0)
		{
			finish();
			return;
		}
		/*
		if ((thing.XY - destinationXY).magnitude < ZEROf || pathRegistered.Count == 0)
		{
			finish();
			return;
		}
		*/
	}
	public virtual bool IsDestinationReached(World world, Thing thing)
	{
		return true;
	}

	private void move(World world, Thing thing, float timeElapsed)
	{

		//I am going to move thing to the target location
		float speed = world.GetThingsSpeed(thing);
		float maxDistanceToMove = (thing.XY - nextDestinationXY).magnitude;
		float distanceApplied = Mathf.Min(speed * timeElapsed, maxDistanceToMove);
		var thingsNewPosition = thing.XY + (nextDestinationXY - thing.XY).normalized * distanceApplied;
		thing.MoveTo( thingsNewPosition);
	}
}
