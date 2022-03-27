using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class Thing{
	//Things that this thing is carrying
	List<Thing> carryingThings = new List<Thing>();
	//Thing carrying me
	Thing beingCarriedBy;

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

	public bool IsBeingCarried
	{
		get{ return beingCarriedBy != null; }
	}
	public void Carry(Thing thingToCarry)
	{
		carryingThings.Add(thingToCarry);
		thingToCarry.beingCarriedBy = this;
		thingToCarry.XY = this.XY;
		
	}


}