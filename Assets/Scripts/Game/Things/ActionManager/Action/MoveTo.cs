using StoryGenerator.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class MoveTo : Action
{
	const float NEWPATH_UPDATING_INTERVAL = 2.0f;
	//position I am trying to go to
	public virtual Vector2 destinationXY { get { return new Vector2(); } }
	Vector2 nextDestinationXY { get { return pathRegistered[0]; } }
	public Vector2 NextDestinationXY { get { return pathRegistered[0]; } }
	public bool IsNextDestAvail { get { return pathRegistered.Count != 0; } }

	bool updateNewPath = true;
	float timeElapsedForNewPathSearching = 0;
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
		timeElapsedForNewPathSearching += timeElapsed;
		if(timeElapsedForNewPathSearching > NEWPATH_UPDATING_INTERVAL)
		{
			updateNewPath = true;
			timeElapsedForNewPathSearching = 0;
		}
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
			if(Mathf.RoundToInt(destinationXY.x) == thing.X_INT && Mathf.RoundToInt(destinationXY.y) == thing.Y_INT)
			{
				pathRegistered.Add(destinationXY);

			}
			else
			{
				Debug.LogError(this + "MoveTo(base) found no path. Terminating due to no path found " + thing.XY + "->" + destinationXY);
				finish();
				return;

			}
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
	bool isBlocked(World world, Thing thing)
	{
		var thingsAtMyDestination = world.GetThingsMovingAt((int)nextDestinationXY.x, (int)nextDestinationXY.y);
		bool blockedByOtherThing = false;
		Thing otherThingBlocking = null;
		if (thingsAtMyDestination.Count != 0)
		{
			foreach (var otherThing in thingsAtMyDestination)
			{
				if (otherThing != thing)
				{
					blockedByOtherThing = true;
					otherThingBlocking = otherThing;

					break;
				}
			}
			//There are more than one thing 
		}
		if (blockedByOtherThing)
		{
			Vector2 otherDestination = Vector2.zero;
			if (!otherThingBlocking.TAM.GetNextDestination(ref otherDestination))
			{
				//other thing had no next destination, thus it is not moving
				return false;
			}
			bool isTryingToGoTOTheSamePosition = false;
			var otherDir = otherDestination - otherThingBlocking.XY;
			var myDir = this.NextDestinationXY - thing.XY;
			otherDir.Normalize();
			myDir.Normalize();
			Debug.Log(myDir + " / " + otherDir);
			bool isCrossPassing = false;
			if (
				(otherDir.x != 0 && myDir.x != 0 && otherDir.x == -myDir.x) ||
				(otherDir.y != 0 && myDir.y != 0 && otherDir.y == -myDir.y)
				)
			{
				isCrossPassing = true;
			}
			if (!isCrossPassing) return true;
		}
		return false;

	}
	private void move(World world, Thing thing, float timeElapsed)
	{
		//I am going to move thing to the target location
		if (isBlocked(world, thing)) return;

		float speed = world.GetThingsSpeed(thing);
		float maxDistanceToMove = (thing.XY - nextDestinationXY).magnitude;
		float distanceApplied = Mathf.Min(speed * timeElapsed, maxDistanceToMove);
		var thingsNewPosition = thing.XY + (nextDestinationXY - thing.XY).normalized * distanceApplied;
		thing.MoveTo( thingsNewPosition);
	}
}
