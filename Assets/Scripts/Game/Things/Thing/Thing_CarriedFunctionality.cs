using StoryGenerator.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class Thing{
	//Things that this thing is carrying
	List<Thing> thingsIAmCarrying = new List<Thing>();
	//Thing carrying me
	Thing thingCarryingThis = null;
	public Thing Carrier { get { return thingCarryingThis; } }
	
	public void InitCarryingFunctionality()
	{
		this.OnPositionChanged.Add(hdrUpdateCarryingThingsPositions);
	}

	public void Drop()
	{
		for(int i = thingsIAmCarrying.Count-1; i >=0; i--)
		{
			dropCarryingThing(thingsIAmCarrying[i]);
		}
	}
	public int CountAllCarryingThings()
	{
		int num = 0;
		num += thingsIAmCarrying.Count;
		foreach(Thing t in thingsIAmCarrying)
		{
			num += t.CountAllCarryingThings();
		}
		return num;
	}

	internal bool AreYouCarrying(Thing thing)
	{
		foreach(Thing t in thingsIAmCarrying)
		{
			if (t == thing) return true;
		}
		return false;
	}

	private void dropCarryingThing(Thing thing)
	{
		thingsIAmCarrying.Remove(thing);
		thing.freeFromCarrier();
	}

	void freeFromCarrier()
	{
		this.thingCarryingThis = null;
	}

	public bool IsBeingCarried
	{
		get { return thingCarryingThis != null; }
	}
	public bool IsCarrying
	{
		get
		{
			return thingsIAmCarrying.Count != 0;

		}
	}


	private void hdrUpdateCarryingThingsPositions(Thing thing, float xBefore, float yBefore, float xNew, float yNew)
	{
		for(int i = 0; i < thingsIAmCarrying.Count; i++)
		{
			thingsIAmCarrying[i].XY = this.XY;
		}
	}

	public virtual float GetGrapRange()
	{
		return 0.5f;
	}


	public void Carry(Thing thingToCarry)
	{
		thingsIAmCarrying.Add(thingToCarry);
		thingToCarry.thingCarryingThis = this;
		thingToCarry.XY = this.XY;


	}

	public virtual bool CheckGetCarriedBy(Thing thing)
	{
		return !IsBeingCarried;
	}
}