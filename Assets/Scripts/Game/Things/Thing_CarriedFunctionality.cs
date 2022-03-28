using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class Thing{
	//Things that this thing is carrying
	List<Thing> carryingThings = new List<Thing>();
	//Thing carrying me
	Thing thingCarryingThis;
	public Thing Carrier { get { return thingCarryingThis; } }

	public void Drop()
	{
		for(int i = carryingThings.Count-1; i >=0; i--)
		{
			dropCarryingThing(carryingThings[i]);
		}
	}

	private void dropCarryingThing(Thing thing)
	{
		carryingThings.Remove(thing);
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
			return carryingThings.Count != 0;

		}
	}
	void initCarryingFunctionality()
	{
		this.OnPositionChanged.Add(hdrUpdateCarryingThingsPositions);
	}

	private void hdrUpdateCarryingThingsPositions(Thing thing, int xBefore, int yBefore, int xNew, int yNew)
	{
		for(int i = 0; i < carryingThings.Count; i++)
		{
			carryingThings[i].XY = this.XY;
		}
	}

	public virtual float GetGrapRange()
	{
		return 1;
	}


	public void Carry(Thing thingToCarry)
	{
		carryingThings.Add(thingToCarry);
		thingToCarry.thingCarryingThis = this;
		thingToCarry.XY = this.XY;
		
	}

	public virtual bool CheckGetCarriedBy(Thing thing)
	{
		return !IsBeingCarried;
	}
}