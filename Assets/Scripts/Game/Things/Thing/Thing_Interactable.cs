
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Thing_Interactable : Thing
{
	public Thing_Interactable(Game.CATEGORY type) : base(type)
	{

	}

	//Thing carrying me
	StoryGenerator.World.Things.Actors.ActorBase thingCarryingThis = null;
	public Thing Carrier { get { return thingCarryingThis; } }
	
	public virtual float GetGrapRange()
	{
		return 0.5f;
	}
	public void freeFromCarrier()
	{
		this.thingCarryingThis = null;
	}
	public void SetCarrier(StoryGenerator.World.Things.Actors.ActorBase actorCarrier)
	{
		thingCarryingThis = actorCarrier;
	}


	public bool IsBeingCarried
	{
		get { return thingCarryingThis != null; }
	}

	internal static bool Is(Thing thing)
	{
		throw new NotImplementedException();
	}

	public virtual bool CheckGetCarriedBy(Thing thing)
	{
		return !IsBeingCarried;
	}


	/*
	private void hdrUpdateCarryingThingsPositions(Thing thing, float xBefore, float yBefore, float xNew, float yNew)
	{
		for (int i = 0; i < thingsIAmCarrying.Count; i++)
		{
			thingsIAmCarrying[i].XY = this.XY;
		}
	}
	 * */



	/*
	public void InitCarryingFunctionality()
	{
		this.OnPositionChanged.Add(hdrUpdateCarryingThingsPositions);
	}

	private void dropCarryingThing(Thing box)
	{
		thingsIAmCarrying.Remove(box);
		box.freeFromCarrier();
	}
	*/


	/*
	public void Drop()
	{
		for (int i = thingsIAmCarrying.Count - 1; i >= 0; i--)
		{
			dropCarryingThing(thingsIAmCarrying[i]);
		}
	}

	public int CountAllCarryingThings()
	{
		int num = 0;
		num += thingsIAmCarrying.Count;
		foreach (Thing t in thingsIAmCarrying)
		{
			num += t.CountAllCarryingThings();
		}
		return num;
	}

	public bool AreYouCarrying(Thing thing)
	{
		foreach (Thing t in thingsIAmCarrying)
		{
			if (t == thing) return true;
		}
		return false;
	}
	 * */
	/*
	public bool IsCarrying
	{
		get
		{
			return thingsIAmCarrying.Count != 0;

		}
	}
	 * */

	/*
	public virtual void Carry(Thing thingToCarry)
	{
		thingsIAmCarrying.Add(thingToCarry);
		thingToCarry.thingCarryingThis = this;
		thingToCarry.XY = this.XY;


	}
	*/


}