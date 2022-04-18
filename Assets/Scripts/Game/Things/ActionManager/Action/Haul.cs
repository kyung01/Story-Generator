using StoryGenerator.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ActionManagerAction {

	public class Haul : Action
	{
		Thing thingToHaul;
		Vector2 destination;
		public Haul(Thing thingToCarry, float x, float y)
		{
			this.thingToHaul = thingToCarry;
			destination = new Vector2(x, y);

		}
		public override void Do(World world, Thing worker, float timeElapsed)
		{
			base.Do(world, worker, timeElapsed);
			bool thingAtDestination = (thingToHaul.XY - destination).magnitude < ZEROf;
			if (thingAtDestination && !thingToHaul.IsBeingCarried)
			{
				Debug.Log("Action Haul Finished");
				//ThingToHaul is at destination and no longer being held
				finish();
				return;
			}
			if(thingAtDestination && thingToHaul.Carrier == worker)
			{
				//Package is at the destination and I need to drop this object 
				Debug.Log("Action Haul Drop");
				//Am I at the best spot? though?
				var zones = world.zoneOrganizer.GetZonesAt(
					Mathf.RoundToInt(destination.x), Mathf.RoundToInt(destination.y), Mathf.RoundToInt(destination.x), Mathf.RoundToInt(destination.y));
				//bool amIDroppingItInTheStockpileZone = false;
				StockpileZone stockpileZone = null;
				foreach (var z in zones)
				{
					if (z is StockpileZone)
					{
						//amIDroppingItInTheStockpileZone = true;
						stockpileZone = (StockpileZone)z;
						break;
					}
				}
				if (stockpileZone != null)
				{
					Debug.Log(this + " stockpile zone is detected");
					int newX=0, newY=0;
					if(!stockpileZone.IsPositionEfficient(world,  worker, Mathf.RoundToInt(destination.x), Mathf.RoundToInt(destination.y)) )
					{
						stockpileZone.GetBestAcceptableEmptyPositionForThing(world, ref newX, ref newY, worker);
						float distanceDiff = (this.destination - new Vector2(newX, newY)).magnitude;
						if(distanceDiff >= ZEROf)
						{
							Debug.Log(this + " stockpile zone offering a new location " + destination + " -> " + new Vector2(newX, newY));
							destination = new Vector2(newX, newY);
							worker.TAM.MoveTo(destination, ThingActionManager.PriorityLevel.FIRST);
							return;
						}
						Debug.Log(this + " current position is the best");

					}
				}
				else
				{

					Debug.Log(this + " stockpile zone is not detected, dropping already");
				}

				worker.TAM.Drop(ThingActionManager.PriorityLevel.FIRST);
				return;
			}
			if(thingToHaul.Carrier != worker)
			{
				//if I am not holding the object, let me grap it first
				Debug.Log("Action Haul Carry");
				worker.TAM.Carry(thingToHaul, ThingActionManager.PriorityLevel.FIRST);
				return;
			}
			if(!thingAtDestination && thingToHaul.Carrier == worker)
			{
				//package is not at the destination but I am carrying it
				Debug.Log("Action Haul MoveTo");
				worker.TAM.MoveTo(destination, ThingActionManager.PriorityLevel.FIRST);
			}
			// I am grapping the object


		}
	}

}
