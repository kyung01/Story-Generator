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
	bool isOpenDoor = false;

	Door doorToOpen = null;

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


		if(doorToOpen == null)
		{
			move(world, thing, timeElapsed);

		}
		else
		{
			//There was a door I needed to open
			if (!doorToOpen.IsOpen)
			{
				//Door is closed!
				doorToOpen.Open();
			}
			else
			{
				//I opened the door
				doorToOpen = null;
			}
		}
		var diff = thing.XY - pathRegistered[0];
		bool reachedCurrentRelativeDestinationOnPath = diff.magnitude < ZEROf;
		if (reachedCurrentRelativeDestinationOnPath)
		{
			pathRegistered.RemoveAt(0);
			if(pathRegistered.Count != 0)
			{
				//There are more paths to go
				var things = world.GetThingsAt(Mathf.RoundToInt(pathRegistered[0].x), Mathf.RoundToInt(pathRegistered[0].y));
				Door door = hprSortDoor(things);
				if(door!= null)
				{
					this.doorToOpen = door;
				}
			}
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

	private Door hprSortDoor(List<Thing> things)
	{
		foreach(var t in things)
		{
			if(t.type == Thing.TYPE.DOOR)
			{
				var d = (Door)t;
				if (d.IsInstalled) return d;
			}
		}
		return null;
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
