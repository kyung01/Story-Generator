using UnityEngine;
using System.Collections.Generic;
using GameEnums;

namespace StoryGenerator.World.Things.Actors
{

	public class ThingNeedManager : ThingModule
	{
		public List<NeedBase> needs = new List<NeedBase>();

		public void AddNeed( NeedBase n )
		{
			Debug.Log("ThingNeedManager Add need called");
			//n.Init(this);
			needs.Add(n);

		}
		public override void Init(Thing thing)
		{
			base.Init(thing);
			for (int i = 0; i < needs.Count; i++)
			{
				needs[i].Init(thing);
			}

		}

		public override void hdrUpdate(World world, Thing thing, float timeElapsed)
		{
			//Debug.Log("THING NEED MANAGER UPDATE ");
			base.hdrUpdate(world, thing, timeElapsed);
			ActorBase actor = (ActorBase)thing;

			for (int i = 0; i < needs.Count; i++)
			{
				needs[i].updateStatic(world, actor, timeElapsed);
				//Debug.Log("THING NEED MANAGER UPDATE " +  i + " "  + needs[i].name + " " + needs[i].fullfillment);
			}

			if (actor.TAM.IsIdl)
			{
				for (int i = 0; i < needs.Count; i++)
				{
					if (needs[i].UpdateResolveNeed(world, actor, timeElapsed))
					{
						Debug.Log("Resolving a need " + needs[i].name);
						var resolvingNeed = needs[i];
						needs.RemoveAt(i);
						needs.Add(resolvingNeed);
						break;
					}
				}
			}

		}
	}

	public class ThingBodyManager : ThingModule
	{
		BodyBase body = new BodyBase();
		public BodyBase MainBody { get { return this.body; } }

		public void AddBody(BodyBase b)
		{
			body.addBody(b);
		}

		public bool IsBodyAvailableForKeywordExchanges()
		{
			//Check if there is a consciousness in the body,
			//if there is a consciousness, then refuse, if there is no consciousness, then give up
			return false;
		}


		public override void Init(Thing thing)
		{
			base.Init(thing);

			this.body.Init(thing);
		}

		public override List<KeywordInformation> hdrGetKeywords()
		{
			var keywords = body.GetKeywords();
			List<KeywordInformation> keywordsReturn = new List<KeywordInformation>();
			foreach (var k in keywords)
			{
				keywordsReturn.Add(new KeywordInformation(k.Key, KeywordInformation.State.LOCKED, k.Value));
			}
			//return bodyOld.GetKeywords();
			return keywordsReturn;
		}

		public override float hdrTakenKeyword(Keyword keywordToRequest, float requestedAmount)
		{
			float amountIProvidedWithItemsIHave = requestedAmount;
			float remainingDebt = requestedAmount - amountIProvidedWithItemsIHave;
			//float debtPaied = 0;
			if (remainingDebt > 0)
			{
				//here I must provide things with my body lol
				if (!IsBodyAvailableForKeywordExchanges())
				{
					//However some conditons must be mat
					return amountIProvidedWithItemsIHave;
				}
				var availableKeywords = this.body.GetKeywords();
				if (!availableKeywords.ContainsKey(keywordToRequest) || availableKeywords[keywordToRequest] == 0)
				{
					//my body does not have what's requested here
					return amountIProvidedWithItemsIHave;
				}
				float paied = this.body.TakenKeyword(keywordToRequest, remainingDebt);

			}
			return amountIProvidedWithItemsIHave;
		}


		public override void hdrUpdate(World world, Thing thing, float timeElapsed)
		{
			body.Update(world, thing, timeElapsed);

		}


	}

	public partial class ActorBase : ThingWithPhysicalPresence
	{


		ThingNeedManager thingNeedManager;
		public ThingNeedManager moduleNeeds { get { return this.thingNeedManager; } }

		ThingBodyManager thingBodyManager;
		public ThingBodyManager moduleBody { get { return this.thingBodyManager; } }

		ThingActionManager thingActManager;

		public ThingActionManager TAM
		{
			get
			{
				return thingActManager;
			}
		}

		List<Need.NeedBase> satisfactions = new List<Need.NeedBase>();
		internal List<Keyword> foodList = new List<Keyword>();

		BaseHousingZone GetHouseZoneIAmAt(World world) {

			var zone = world.zoneOrganizer.GetZoneAt(this.X_INT, this.Y_INT);
			//Debug.Log("Zone " + zone);
			if (zone is BaseHousingZone) return (BaseHousingZone)zone;
			return null;
		}
		public ActorBase(ThingCategory category) : base(category)
		{

			this.thingActManager = new ThingActionManager();
			InitCarryingFunctionality();

			thingNeedManager = new ThingNeedManager();
			thingNeedManager.Init(this);


			thingBodyManager = new ThingBodyManager();
			thingBodyManager.Init(this);
		}

		public ActorBase AddNeed(NeedBase need)
		{
			Debug.Log("Add need called");
			need.Init(this);
			this.thingNeedManager.AddNeed(need);
			return this;
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

		public void addNeed(Need.NeedBase s)
		{
			s.Init(this);
			this.satisfactions.Add(s);
		}

		public virtual void DoEat(World world)
		{

		}

		public virtual bool DoSleep(World world)
		{
			var hZone = GetHouseZoneIAmAt(world);
			if (hZone == null)
			{
				Debug.LogError(this + " Actor base must be in a housezone to perform acting");
				return false;
			}
			/*
			var zone = world.zoneOrganizer.GetZoneAt(this.X_INT, this.Y_INT);
			if (zone == null)
			{
				Debug.Log("Actor attempted to sleep but failed to be in a zone, therefore this action cannot proceed");
				return;
			}
			*/
			if (this.IsBeingCarried)
			{
				this.ResolveInteractor();
			}
			if (this.IsBeingCarried)
			{
				return false;
			}


			//var thingsIsee = world.GetSightableThings(this, this.moduleBody.MainBody.GetSight());
			//Debug.Log("DoSleep I SEE " + thingsIsee.Count);
			
			foreach (var t in hZone.Things)
			{
				//Debug.Log(t.Category);
				if (t is ISleepableStructure)
				{
					//Debug.Log("BED FOUND");
					//this is bed
					var bed = (ISleepableStructure)t;
					if (bed.IsSleepable(world, this))
					{
						this.TAM.Sleep(world,bed);
						return true;
					}
					else
					{

						Debug.Log("BED NOT SLEEPABLE");
					}

				}
			}
			Debug.LogError("Cannot find bed out of "  + hZone.Things.Count + " things " );
			foreach(var t in hZone.Things)
			{
				Debug.LogError(t);
			}
			return false;


		}

		internal void DoDream(World world)
		{
			TAM.Dream(world);
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
			this.TAM.Update(world, this, timeElapsed);



			base.Update(world, timeElapsed);




			for (int i = 0; i < satisfactions.Count; i++)
			{
				satisfactions[i].Update(world, this, timeElapsed);
			}
		}

	}
}