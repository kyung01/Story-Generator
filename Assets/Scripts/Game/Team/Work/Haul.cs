using UnityEngine;
using StoryGenerator.World;
using System.Collections.Generic;

public class Haul : Work
{
	public Thing thingToHowl;
	bool isNewLocationNeeded = true;
	Vector2 destination;
	StockpileZone destinationZone;

	public Haul(Thing assignedWorker, Thing thingToHowl) : base(assignedWorker)
	{
		this.thingToHowl = thingToHowl;
		

	}
	bool updateNewLocation(World world)
	{
		int x = 0, y = 0;
		//Dictionary<StockpileZone, Vector2> availableStockpileZones = new Dictionary<StockpileZone, Vector2>();
		List<StockpileZone> availableZones = new List<StockpileZone>();
		var stockpiles = world.zoneOrganizer.GetStockpiles();
		foreach (var sp in stockpiles)
		{
			if (sp.GetAcceptableEmptyPosition(world, ref x, ref y))
			{
				//there was an acceptable position to put this item
				availableZones.Add(sp);
			}
		}
		if (availableZones.Count == 0)
		{
			//Failed to find an available spot to put
			return false;
		}
		var zoneSelected = availableZones[Random.Range(0, availableZones.Count)];
		zoneSelected.GetAcceptableEmptyPosition(world, ref x, ref y);
		this.destination = new Vector2(x, y);
		this.destinationZone = zoneSelected;
		return true;
	}
	bool isCompleted()
	{
		if( (thingToHowl.XY - destination).magnitude < ZEROf && !thingToHowl.IsBeingCarried)
		{
			return true;
		}
		return false;
	}
	public override void Do(World world, float timeElapsed)
	{
		base.Do(world, timeElapsed);
		if (isCompleted())
		{
			finish();
			return;

		}
		if (isNewLocationNeeded)
		{
			if (updateNewLocation(world))
			{
				isNewLocationNeeded = false;
			}
			else
			{
				return;
			}
		}
		if(this.assignedWorker.TAM.IsIdl)
			this.assignedWorker.TAM.Haul(thingToHowl, destination.x, destination.y);
	

	}
}
