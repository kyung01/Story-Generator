using UnityEngine;
using System.Collections.Generic;
using GameEnums;

namespace StoryGenerator.World.Things.Actors
{

	public partial class ActorBase : ThingWithPhysicalPresence
	{


		ThingActionManager thingActManager;

		public ThingActionManager TAM
		{
			get
			{
				return thingActManager;
			}
		}

		List<Satisfaction.SatisfactionBase> satisfactions = new List<Satisfaction.SatisfactionBase>();
		internal List<Keyword> foodList = new List<Keyword>();
		public ActorBase(CATEGORY type) : base(type)
		{

			this.thingActManager = new ThingActionManager();
			InitCarryingFunctionality();
		}


		public bool MoveTo(Vector2 position)
		{
			return this.MoveTo(position.x, position.y);
		}

		public bool MoveTo(float x, float y)
		{
			if (IsBeingCarried)
			{
				return false;
			}
			this.XY = new Vector2(x, y);
			return true;
		}



















		public void addSatisfaction(Satisfaction.SatisfactionBase s)
		{
			s.Init(this);
			this.satisfactions.Add(s);
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

		public override void Update(World world, float timeElapsed)
		{
			base.Update(world, timeElapsed);



			this.TAM.Update(world, this, timeElapsed);




			for (int i = 0; i < satisfactions.Count; i++)
			{
				satisfactions[i].Update(world, this, timeElapsed);
			}
		}

	}
}