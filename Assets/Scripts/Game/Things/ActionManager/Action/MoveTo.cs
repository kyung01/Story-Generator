using StoryGenerator.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class MoveTo : Action
{
	/// <summary>
	/// Pathing need to be updated periodically to effectively reach the destination
	/// </summary>
	const float NEWPATH_UPDATING_INTERVAL = 2.0f;

	//position I am trying to go to
	public virtual Vector2 destinationXY { get { return new Vector2(); } }
	Vector2 nextDestinationXY { get { return pathRegistered[0]; } }
	public Vector2 NextDestinationXY { get { return pathRegistered[0]; } }
	public bool IsNextDestAvail { get { return pathRegistered.Count != 0; } }

	bool shouldUpdateNewPath = true;
	float timeElapsedForNewPathSearching = 0;
	List<Vector2> pathRegistered = new List<Vector2>();
	List<int> pathFacingDirection = new List<int>();

	bool isOpenDoor = false;

	Door doorToOpen = null;

	public MoveTo():base(Type.MOVE_TO)
	{
		this.name = "MoveTo";
	}

	void flipToNextPathPointRegistered()
	{
		pathRegistered.RemoveAt(0);
	}
	void hprAddNextDirectionFacing(Vector2 from, Vector2 to)
	{
		var diff = to - from;
		if (
			Mathf.Abs(diff.x) > Mathf.Abs(diff.y)||
			Mathf.Abs(diff.x) == Mathf.Abs(diff.y)
			)
		{
			//base it around x Axis
			if (diff.x > 0)
			{
				pathFacingDirection.Add(1);
			}
			else if (diff.x < 0)
			{
				pathFacingDirection.Add(3);

			}
		}
		else if (Mathf.Abs(diff.x) < Mathf.Abs(diff.y))
		{
			if (diff.y > 0)
			{
				pathFacingDirection.Add(0);
			}
			else if (diff.y < 0)
			{
				pathFacingDirection.Add(2);

			}
		}
		else if( Mathf.Abs(diff.x) == Mathf.Abs(diff.y))
		{

		}
		else
		{
			Debug.LogError("Failed to detect which direction MoveTo entity Thing needs to face " + from + " " + to);
			pathFacingDirection.Add(0);
		}
	}
	void addNextPath(Thing thing, Vector2 point)
	{
		if(pathRegistered.Count == 0)
		{
			hprAddNextDirectionFacing(thing.XY, point);
		}
		else
		{
			hprAddNextDirectionFacing(pathRegistered[pathRegistered.Count-1],point);
		}
		pathRegistered.Add(point);

	}
	void clearAllPath()
	{

		pathRegistered = new List<Vector2>();
		pathFacingDirection = new List<int>();
	}

	void UpdateNewPath(World world, Thing thing, float timeElapsed)
	{
		timeElapsedForNewPathSearching += timeElapsed;
		if (timeElapsedForNewPathSearching > NEWPATH_UPDATING_INTERVAL)
		{
			shouldUpdateNewPath = true;
			timeElapsedForNewPathSearching = 0;
		}
		if (!shouldUpdateNewPath) return;
		
		clearAllPath();

		var path = world.pathFinder.getPath(thing.XY_Int, destinationXY);
		while (path != null)
		{
			addNextPath(thing,path);
			path = path.after;
		}

		//If no path is registered, it means I am already at where I am, add "fixing" point(where I am) to the stack 
		if (pathRegistered.Count == 0)
		{
			if (Mathf.RoundToInt(destinationXY.x) == thing.X_INT && Mathf.RoundToInt(destinationXY.y) == thing.Y_INT)
			{
				pathRegistered.Add(destinationXY);
				pathFacingDirection.Add(thing.DirectionFacing);

			}
		}
		shouldUpdateNewPath = false;

	}
	void MoveToAndOpenDoorIfNeedTo(World world, Thing thing, float timeElapsed)
	{


		if (thing.DirectionFacing != pathFacingDirection[0])
		{
			if (!thing.Face(world ,pathFacingDirection[0]))
			{
				return;
			}
		}



		//Debug.Log("AvailablePaths " + pathRegistered.Count);


		var things = world.GetThingsAt(Mathf.RoundToInt(pathRegistered[0].x), Mathf.RoundToInt(pathRegistered[0].y));
		Door door = hprSortDoor(things);
		if (door != null)
		{
			door.Open();
			if (door.IsOpen)
			{
				door = null;
			}
		}
		bool isMovable = doorToOpen == null && (door == null || door.IsOpen);

		if (isMovable)
		{
			move(world, thing, timeElapsed);
		}

		var diffToWhereINeedToGo = thing.XY - pathRegistered[0];
		bool reachedCurrentRelativeDestinationOnPath = diffToWhereINeedToGo.magnitude < ZEROf;
		if (reachedCurrentRelativeDestinationOnPath)
		{
			flipToNextPathPointRegistered();
		}
	}
	public override void Do(World world, Thing thing, float timeElapsed)
	{
		base.Do(world, thing, timeElapsed);
		UpdateNewPath(world, thing, timeElapsed);
		MoveToAndOpenDoorIfNeedTo(world, thing, timeElapsed);

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
			if(t.T == Thing.TYPE.DOOR)
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
			Vector2 otherDir = hprGetDir( otherThingBlocking.XY, otherDestination);
			var myDir = hprGetDir(thing.XY,this.NextDestinationXY ) ;
			otherDir.Normalize();
			myDir.Normalize();
			//Debug.Log(myDir + " / " + otherDir);
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

	private Vector2 hprGetDir(Vector2 xY, Vector2 otherDestination)
	{
		var originalD = otherDestination - xY;
		var d = otherDestination - xY;
		Vector2[] dirs = new Vector2[] {Vector2.up,Vector2.right,Vector2.down,Vector2.left,
			Vector2.up + Vector2.right ,Vector2.up + Vector2.left,
			Vector2.down + Vector2.right,Vector2.down + Vector2.left
		};
		float distance = 999;
		foreach(var exampleD in dirs)
		{
			var sqrM = (exampleD - originalD).sqrMagnitude;
			if (sqrM < distance)
			{
				d = exampleD;
				distance = sqrM;
			}

		}
		return d;
	}

	private void move(World world, Thing thing, float timeElapsed)
	{
		//I am going to move thing to the target location
		if (isBlocked(world, thing)) return;

		float speed = world.GetThingSpeed(thing);
		float maxDistanceToMove = (thing.XY - nextDestinationXY).magnitude;
		float distanceApplied = Mathf.Min(speed * timeElapsed, maxDistanceToMove);
		var thingsNewPosition = thing.XY + (nextDestinationXY - thing.XY).normalized * distanceApplied;
		thing.MoveTo( thingsNewPosition);
	}
}
