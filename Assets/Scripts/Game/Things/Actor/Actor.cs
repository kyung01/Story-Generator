using StoryGenerator.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;




namespace StoryGenerator.World.Things.Actors
{

	public class Actor : Thing
	{
		public enum ActionType { EAT,SLEEP,WOOHOO}

		List<Satisfaction.SatisfactionBase> satisfactions = new List<Satisfaction.SatisfactionBase>();
		internal List<Game.Keyword> foodList = new List<Game.Keyword>();

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
		public virtual float GetScore(Thing thing, ActionType actionType)
		{
			return 0;
		}
	}
}