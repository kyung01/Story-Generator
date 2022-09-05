using StoryGenerator.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class InteractSpot
{
	//ThingWithPhysicalPresence structureWithThisSpot;
	int interactSpot_X, interactSpot_Y; //Thing will interact at here
	//int usingSpot_X, usingSpot_Y;//Once it interacts, thing will be moved to here
	OccupyingSpot spotForInteractingThing;

	public InteractSpot( int x, int y, OccupyingSpot spot)
	{
		//this.structureWithThisSpot = owner;
		this.interactSpot_X = x;
		this.interactSpot_Y = y;
		spotForInteractingThing = spot;

	}
	public bool IsAvailableForConsideration(World world, ThingWithPhysicalPresence structureWithThisSpot) {
		if (!spotForInteractingThing.IsFree()) return false;
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
		if (!IsAvailableForConsideration(world, structureWithThisSpot)) return false;
		int x, y;
		structureWithThisSpot.GetRelativePosition(interactSpot_Y, interactSpot_Y, out x, out y);
		float diff = (thing.XY_Int - new UnityEngine.Vector2(x, y)).magnitude;
		return EasyTools.EzT.IsZero(diff);
	}
	public void Interact(ThingWithPhysicalPresence structureWithThisSpot,Thing user)
	{
		int x, y;
		spotForInteractingThing.GetXY(structureWithThisSpot, out x,out y);
		spotForInteractingThing.GetOuccpiedBy(user);

		user.SetPosition(x, y);
		//user.OnPositionIndexChanged.Add(hdrCheckIfUserLeftBecauseUserMoved);
	}
	public void UnInteract(ThingWithPhysicalPresence structureWithThisSpot)
	{
		if (this.spotForInteractingThing.IsFree()) return;

		var user = spotForInteractingThing.Occupier;
		spotForInteractingThing.Free();

		int x, y;
		structureWithThisSpot.GetRelativePosition(interactSpot_Y, interactSpot_Y, out x, out y);

		user.SetPosition(x, y);//hdrCheckIfUserLeftBecauseUserMoved will be removed automatically
		

	}

}
