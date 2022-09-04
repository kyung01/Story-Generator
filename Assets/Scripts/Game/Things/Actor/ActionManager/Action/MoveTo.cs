using StoryGenerator.World;
using StoryGenerator.World.Things.Actors;
using GameEnums;
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
	List<Direction> pathFacingDirection = new List<Direction>();

	bool isOpenDoor = false;

	Door doorToOpen = null;

	public MoveTo() : base(Type.MOVE_TO)
	{
		this.name = "MoveTo";
	}

	void flipToNextPathPointRegistered(World world)
	{
		pathRegistered.RemoveAt(0);
		pathFacingDirection.RemoveAt(0);
		
		if(pathRegistered.Count != 0)
		{

			var things = world.GetThingsAt(Mathf.RoundToInt(pathRegistered[0].x), Mathf.RoundToInt(pathRegistered[0].y));
			doorToOpen =  hprSortDoor(things);
		}

	}

	void hprAddNextDirectionFacing(Vector2 from, Vector2 to)
	{
		var diff = to - from;
		if (Mathf.Abs(diff.x) == Mathf.Abs(diff.y))
		{
			if (diff.x > 0)
			{
				if (diff.y > 0)
				{
					pathFacingDirection.Add(Direction.UP_RIGHT);
				}
				else if (diff.y < 0)
				{
					pathFacingDirection.Add(Direction.RIGHT_DOWN);

				}
			}
			else if (diff.x < 0)
			{
				if (diff.y > 0)
				{
					pathFacingDirection.Add(Direction.LEFT_UP);
				}
				else if (diff.y < 0)
				{
					pathFacingDirection.Add(Direction.DOWN_LEFT);

				}

			}
		}
		else if (diff.x != 0 && diff.y == 0
			)
		{
			//base it around x Axis
			if (diff.x > 0)
			{
				pathFacingDirection.Add(Direction.RIGHT);
			}
			else if (diff.x < 0)
			{
				pathFacingDirection.Add(Direction.LEFT);

			}
		}
		else if (diff.y != 0 && diff.x == 0)
		{
			if (diff.y > 0)
			{
				pathFacingDirection.Add(Direction.UP);
			}
			else if (diff.y < 0)
			{
				pathFacingDirection.Add(Direction.DOWN);

			}
		}
		else
		{
			Debug.LogError("Failed to detect which direction MoveTo entity Thing needs to face " + from + " " + to);
			pathFacingDirection.Add(0);
		}
		//Debug.Log("Move " + from + " " + to + " " + pathFacingDirection[pathFacingDirection.Count - 1]);
	}
	void addNextPath(Thing thing, Vector2 point)
	{
		if (pathRegistered.Count == 0)
		{
			hprAddNextDirectionFacing(thing.XY_Int, point);
		}
		else
		{
			hprAddNextDirectionFacing(pathRegistered[pathRegistered.Count - 1], point);
		}
		pathRegistered.Add(point);



	}
	void clearAllPath()
	{

		pathRegistered.Clear();
		pathFacingDirection.Clear();
	}

	bool UpdateNewPath(World world, Thing thing, float timeElapsed)
	{
		var thingWithDirection = (ThingWithPhysicalPresence)thing;
		timeElapsedForNewPathSearching += timeElapsed;
		if (timeElapsedForNewPathSearching > NEWPATH_UPDATING_INTERVAL)
		{
			shouldUpdateNewPath = true;
			timeElapsedForNewPathSearching = 0;
		}
		if (!shouldUpdateNewPath) return true;

		clearAllPath();

		var path = world.pathFinder.getPath(thingWithDirection.XY_Int, destinationXY);
		while (path != null)
		{
			addNextPath(thingWithDirection, path);
			path = path.after;
		}

		//If no path is registered, it means I am already at where I am, add "fixing" point(where I am) to the stack 
		if (pathRegistered.Count == 0)
		{
			if (Mathf.RoundToInt(destinationXY.x) == thingWithDirection.X_INT && Mathf.RoundToInt(destinationXY.y) == thingWithDirection.Y_INT)
			{
				pathRegistered.Add(destinationXY);
				pathFacingDirection.Add(thingWithDirection.DirectionFacing);

			}
			else
			{
				//wait I am not at where I need to be and I cannot reach where I need to by pathing
				//I CANNOT GO WHERE I WANT TO GO!
				Debug.LogError("MoveTo failed " + thing.XY + " ->" + destinationXY);
				finish();
				return false;

			}
		}
		shouldUpdateNewPath = false;
		return true;

	}
	void MoveToAndOpenDoorIfNeedTo(World world, ActorBase thing, float timeElapsed)
	{

		var thingWithDirection = (ThingWithPhysicalPresence)thing;
		try
		{
			if (!thingWithDirection.Face(world, pathFacingDirection[0]))
			{
				return;
			}
		}
		catch
		{
			Debug.LogError("Error 1 " + pathFacingDirection.Count + " " +pathRegistered.Count);

		}
	


		//Debug.Log("AvailablePaths " + pathRegistered.Count);

		if (doorToOpen != null  && !doorToOpen.IsOpen)
		{
			doorToOpen.Open();
			if (doorToOpen.IsOpen)
			{
				doorToOpen = null;
			}
		}
		bool isMovable = doorToOpen == null || doorToOpen.IsOpen;// && (door == null || door.IsOpen);

		if (isMovable)
		{
			move(world, thing, timeElapsed);
		}

		try
		{
			var diffToWhereINeedToGo = thing.XY - pathRegistered[0];
			bool reachedCurrentRelativeDestinationOnPath = diffToWhereINeedToGo.magnitude < ZEROf;
			if (reachedCurrentRelativeDestinationOnPath)
			{
				flipToNextPathPointRegistered(world);
			}
		}
		catch
		{
			Debug.LogError("Error 2");
		}
		
	}
	public override void Do(World world, ActorBase thing, float timeElapsed)
	{
		base.Do(world, thing, timeElapsed);
		if(!UpdateNewPath(world, thing, timeElapsed))
		{
			//Update failed;
			return;
		}
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
		foreach (var t in things)
		{
			if (t.Category == CATEGORY.DOOR)
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
			if (otherThingBlocking is ActorBase)
			{
				if (!((ActorBase)otherThingBlocking).TAM.GetNextDestination(ref otherDestination))
				{
					//other thing had no next destination, thus it is not moving
					return false;
				}
			}
			else
			{
				return false;
			}

			bool isTryingToGoTOTheSamePosition = false;
			Vector2 otherDir = hprGetDir(otherThingBlocking.XY, otherDestination);
			var myDir = hprGetDir(thing.XY, this.NextDestinationXY);
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
		foreach (var exampleD in dirs)
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

	private void move(World world, ActorBase thing, float timeElapsed)
	{
		//I am going to move thing to the target location
		if (isBlocked(world, thing)) return;

		float speed = world.GetThingSpeed(thing);
		float maxDistanceToMove = (thing.XY - nextDestinationXY).magnitude;
		float distanceApplied = Mathf.Min(speed * timeElapsed, maxDistanceToMove);
		var thingsNewPosition = thing.XY + (nextDestinationXY - thing.XY).normalized * distanceApplied;
		thing.MoveTo(thingsNewPosition);
	}
}
