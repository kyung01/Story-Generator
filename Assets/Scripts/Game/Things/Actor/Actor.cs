using StoryGenerator.World;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryGenerator.World.Things.Actors
{

	public class Actor : Thing
	{

		
		List<Satisfaction.SatisfactionBase> satisfactions = new List<Satisfaction.SatisfactionBase>();
		internal List<Game.Keyword> foodList = new List<Game.Keyword>();
		public Actor(TYPE type) : base(type)
		{

		}
		public void addSatisfaction(Satisfaction.SatisfactionBase s)
		{
			s.Init(this);
			this.satisfactions.Add(s);
		}

		public override void Update(World world, float timeElapsed)
		{
			base.Update(world, timeElapsed);
			for(int i =0; i < satisfactions.Count; i++)
			{
				satisfactions[i].Update(world,this, timeElapsed);
			}
		}

		public virtual void DoEat(World world)
		{

		}

		public virtual void DoSleep(World world)
		{
			var zone = world.zoneOrganizer.GetZoneAt(this.X_INT, this.Y_INT);
			if (zone == null)
			{
				Debug.Log("Actor attempted to sleep but failed to be in a zone, therefore this action cannot proceed");
				return;
			}

		}

		public virtual void DoRest(World world)
		{

		}

		public virtual void DoFun(World world)
		{

		}

		public virtual void OnMovedTo(World world, int xOld, int yOld, int xNew, int yNew)
		{

		}

		public virtual float GetScore(Thing thing, Action.Type actionType)
		{
			return 0;
		}
	}
}