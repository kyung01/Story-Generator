using UnityEngine;
using StoryGenerator.World;
using System.Collections.Generic;
using StoryGenerator.World.Things.Actors;

public class Haul : Work
{
	public Thing_Interactable thingToHowl;
	bool isNewLocationNeeded = true;
	Vector2 destination;
	StockpileZone destinationZone;

	public Haul(ActorBase assignedWorker, Thing_Interactable thingToHowl) : base(assignedWorker)
	{
		this.thingToHowl = thingToHowl;
		

	}
	bool updateNewLocation(World world)
	{
		int x = 0, y = 0;
		//Dictionary<StockpileZone, Vector2> availableStockpileZones = new Dictionary<StockpileZone, Vector2>();
		List<StockpileZone> availableZones = new List<StockpileZone>();
		var stockpiles = world.zoneOrganizer.GetStockpiles();
		Debug.Log(this + " worker " +this.assignedWorker);
		foreach (var a_stockpile in stockpiles)
		{
			if (a_stockpile.GetBestAcceptableEmptyPositionForThing(world, ref x, ref y,this.assignedWorker))
			{
				//there was an acceptable position to put this item
				availableZones.Add(a_stockpile);
			}
		}
		if (availableZones.Count == 0)
		{
			//Failed to find an available spot to put
			return false;
		}
		var zoneSelected = availableZones[Random.Range(0, availableZones.Count)];
		zoneSelected.GetBestAcceptableEmptyPositionForThing(world, ref x, ref y,this.assignedWorker);
		this.destination = new Vector2(x, y);
		this.destinationZone = zoneSelected;
		return true;
	}
	bool isCompleted()
	{
		if (destinationZone == null) return false;
		if(destinationZone.IsInZone(Mathf.RoundToInt(thingToHowl.X), Mathf.RoundToInt(thingToHowl.Y)))
		{
			if (!thingToHowl.IsBeingCarried)
			{
				return true;
			}

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
		if (this.assignedWorker.TAM.IsIdl)
		{

			Debug.Log("Work : Haul Commanded");
			this.assignedWorker.TAM.Haul(thingToHowl, destination.x, destination.y);
		}
	

	}
}
