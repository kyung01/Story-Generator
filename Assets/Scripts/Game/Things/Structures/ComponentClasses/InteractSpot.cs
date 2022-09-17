using StoryGenerator.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class InteractSpot
{
	//ThingWithPhysicalPresence structureWithThisSpot;
	int interactSpot_X, interactSpot_Y; //Thing will interact at here
	//int usingSpot_X, usingSpot_Y;//Once it interacts, thing will be moved to here
	OccupyingSpot occupyingSpot;

	public InteractSpot( int x, int y, OccupyingSpot spot)
	{
		//this.structureWithThisSpot = owner;
		this.interactSpot_X = x;
		this.interactSpot_Y = y;
		occupyingSpot = spot;

	}
	public bool IsAvailableForConsideration(World world, ThingWithPhysicalPresence structureWithThisSpot) {
		if (!occupyingSpot.IsFree()) return false;
		int x, y;
		structureWithThisSpot.GetRelativePosition(interactSpot_X, interactSpot_Y, out x, out y);
		if(world.IsWalkableAt(x, y))
		{
			return true;
		}
		return false;
	}

	internal void GetInteractionXY(ThingWithPhysicalPresence structureWithThisSpot, out int x, out int y)
	{
		structureWithThisSpot.GetRelativePosition(this.interactSpot_X, this.interactSpot_Y, out x, out y);
	}

	public bool CanInteractWithIt(World world, ThingWithPhysicalPresence structureWithThisSpot, Thing thing)
	{
		if (!IsAvailableForConsideration(world, structureWithThisSpot))
		{
			Debug.Log("Bed is no longer interactable");
			return false;
		}
		int x, y;
		structureWithThisSpot.GetRelativePosition(interactSpot_X, interactSpot_Y, out x, out y);
		float diff = (thing.XY_Int - new UnityEngine.Vector2(x, y)).magnitude;
		Debug.Log("checking position " + thing.XY + " but mine is at " + x + " " + y);
		return EasyTools.EzT.IsZero(diff);
	}
	public void Interact(ThingWithPhysicalPresence structureWithThisSpot, ThingWithPhysicalPresence user)
	{
		int x, y;
		occupyingSpot.GetXY(structureWithThisSpot, out x,out y);
		occupyingSpot.GetOuccpiedBy(user);
		user.SetPosition(x, y);
		user.SetFacingDirection(occupyingSpot.GetDirection(structureWithThisSpot));
		//user.OnPositionIndexChanged.Add(hdrCheckIfUserLeftBecauseUserMoved);
	}
	public bool CanUnInteract(Thing thing)
	{
		if(this.occupyingSpot.Occupier == thing) {
			return true;
		}
		return false;
	}
	public void UnInteract(ThingWithPhysicalPresence structureWithThisSpot)
	{
		if (this.occupyingSpot.IsFree())
		{
			Debug.Log("Occupying spot is already free??? What do I uninteract");
			return;
		}

		var user = occupyingSpot.Occupier;
		occupyingSpot.Free();

		int x, y;
		structureWithThisSpot.GetRelativePosition(interactSpot_X, interactSpot_Y, out x, out y);
		Debug.Log("setting user's poisiton from " + user.XY);
		Debug.Log("setting user's poisiton to " + x + " " + y);
		user.SetPosition(x, y);//hdrCheckIfUserLeftBecauseUserMoved will be removed automatically
		

	}

}
