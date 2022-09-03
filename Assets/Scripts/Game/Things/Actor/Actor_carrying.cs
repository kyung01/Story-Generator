using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace StoryGenerator.World.Things.Actors
{
	public partial class ActorBase 
	{
		//Things that this thing is carrying
		List<Thing_Interactable> thingsIAmCarrying = new List<Thing_Interactable>();

		private void hdrUpdateCarryingThingsPositions(Thing thing, float xBefore, float yBefore, float xNew, float yNew)
		{
			for (int i = 0; i < thingsIAmCarrying.Count; i++)
			{
				thingsIAmCarrying[i].XY = this.XY;
			}
		}

		public virtual float GetGrapRange()
		{
			return 0.5f;
		}

		public void InitCarryingFunctionality()
		{
			this.OnPositionChanged.Add(hdrUpdateCarryingThingsPositions);
		}

		private void dropCarryingThing(Thing_Interactable box)
		{
			thingsIAmCarrying.Remove(box);
			box.freeFromCarrier();
		}

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
				if (ActorBase.IsAnActor(t))
				{
					var actor = (ActorBase)t;
					num += actor.CountAllCarryingThings();
				}
			}
			return num;
		}

		private static bool IsAnActor(Thing t)
		{
			if (t.T == TYPE.HUMAN) return true;
			if (t.T == TYPE.RABBIT) return true;
			if (t.T == TYPE.BEAR) return true;
			return false;
		}

		public bool AreYouCarrying(Thing thing)
		{
			foreach (Thing t in thingsIAmCarrying)
			{
				if (t == thing) return true;
			}
			return false;
		}



		public bool IsCarrying
		{
			get
			{
				return thingsIAmCarrying.Count != 0;

			}
		}

		public virtual void Carry(Thing_Interactable thingToCarry)
		{
			thingsIAmCarrying.Add(thingToCarry);
			thingToCarry.SetCarrier( this);
			thingToCarry.XY = this.XY;


		}

	}

}
