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
				//Apple is at the destination and I need to drop this object 
				Debug.Log("Action Haul Drop");
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
