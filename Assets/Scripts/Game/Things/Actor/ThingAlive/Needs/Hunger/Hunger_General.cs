using StoryGenerator.World;
using StoryGenerator.World.Things.Actors;
using GameEnums;
using System.Collections.Generic;

public partial class Hunger_General : NeedBase { 

	public static Hunger_General InitSimpleHunger(Keyword keywordRequired, params HungerResolutionMethodType[] arr)
	{
		Hunger_General hungerNeed = new Hunger_General();
		hungerNeed.requiredKeywords.Add(keywordRequired);
		hungerNeed.stressKeywords.Add(Keyword.HUNGER);
		hungerNeed.methodsAvailable.AddRange(arr);
		return hungerNeed;
	}
}

public partial class Hunger_General : NeedBase
{
	public enum HungerResolutionMethodType { PASSIVE,HUNT,STEAL,COOK}

	const float LOOKING_FOR_FOOD_DISTNACE = 10;
	float fullfillmentMinimumSatisfaction = 80;
	float desiredKeywordTransfer_To_CalmDownDemandCall = 30;
	//bool isHuntersHunger = false;
	List<HungerResolutionMethodType> methodsAvailable = new List<HungerResolutionMethodType>();

	public Hunger_General(params HungerResolutionMethodType[] arr)
	{
		methodsAvailable.AddRange(arr);
		//this.isHuntersHunger = isHuntersHunger;
		//this.requiredKeywords.Add(Keyword.FOOD);
		//this.stressKeywords.Add( Keyword.HUNGER);

		this.name = "Hunger";
		this.explanation = "A general need for food. It can be satisfied by any consumable that can be calssified as a food.";
	}

	public override void Init(Thing thing)
	{
		base.Init(thing);
		this.fullfillment = fullfillmentMinimumSatisfaction+1;
		thing.OnReceiveKeyword.Add( hdrKeywordConsumed);
	}

	void getTheBestFoodSourceTarget(World world, Thing thing, bool isHunter, 
		ref Thing targetThing, ref Keyword keywordSelected)
	{
		List<Thing> thingsToEat = thing.ThinkGetEdible(world);
		var thingsIsee = world.GetSightableThings(thing, ((ActorBase)thing).moduleBody.MainBody.GetSight());
		if (thing.moduleHouse != null)
		{
			thingsIsee.AddRange(thing.moduleHouse.GetThings(requiredKeywords));
		}

		if(thing.moduleHouse!= null)
		{
			List<Thing> thingsInHouse = thing.moduleHouse.GetThings(requiredKeywords);

		}

		getBestTargetThing(world, thingsIsee, requiredKeywords,ref targetThing,ref keywordSelected, isHunter);
	}

	//Passive resolution is to eat something that's available freely
	public bool resolution_passive(World world, ActorBase thing, ref Thing bestTargetThing, ref Keyword keywordSelected)
	{
		getTheBestFoodSourceTarget(world, thing, false, ref bestTargetThing, ref keywordSelected);


		//if (isHunter) UnityEngine.Debug.Log(this + " thingsIsee " + thingsIsee.Count);
		//if (isHunter) UnityEngine.Debug.Log(this + " Resolving hunger " + (bestTargetThing != null));
		if (bestTargetThing == null)
		{
			//UnityEngine.Debug.LogError(this + ""+thing.type +" : " );
			UnityEngine.Debug.LogWarning(this + " " + thing.Category + " : CANNOT BE COMPLETED::BestTargetThing was null");

			//thing.TAM.MoveToRandomLocationOfDistance(world, thing, LOOKING_FOR_FOOD_DISTNACE);
			return false;
		}

		thing.TAM.MoveToTarget(bestTargetThing, thing.GetEatingDistance());
		//UnityEngine.Debug.Log(this + " MoveToTarget");
		/*
		thing.TAM.Eat(
				   bestTargetThing,
				   keywordSelected,
				   desiredKeywordAmount
				   );
		*/

		return true;
	}

	//Cooker's resolution is to create a possibly something I need from the available resources around me 
	public bool resolution_cook(World world, Thing thing, ref Thing targetThing)
	{

		return false;
	}

	//Hunter's resolutiuon is to hunt an animal/target and gain the resource it requires
	public bool resolution_hunter(World world, ActorBase thing, ref Thing bestTargetThing, ref Keyword keywordSelected)
	{

		getTheBestFoodSourceTarget(world, thing, true, ref bestTargetThing, ref keywordSelected);


		//if (isHunter) UnityEngine.Debug.Log(this + " thingsIsee " + thingsIsee.Count);
		//if (isHunter) UnityEngine.Debug.Log(this + " Resolving hunger " + (bestTargetThing != null));
		if (bestTargetThing == null)
		{
			//UnityEngine.Debug.LogError(this + ""+thing.type +" : " );
			UnityEngine.Debug.LogWarning(this + " " + thing.Category + " : CANNOT BE COMPLETED::BestTargetThing was null");

			//thing.TAM.MoveToRandomLocationOfDistance(world, thing, LOOKING_FOR_FOOD_DISTNACE);
			return false;
		}

		thing.TAM.MoveToTarget(bestTargetThing, thing.GetEatingDistance());
		UnityEngine.Debug.Log(this + " MoveToTarget");
		/*
		thing.TAM.Hunt(
				bestTargetThing,
				keywordSelected,
				desiredKeywordAmount
				);
		*/

		return true;
	}

	//Thief's resolution is to steal the resource from the target
	public bool resolution_thief()
	{
		return false;

	}
	public bool eatResolution(World world, ActorBase thing, float timeElapsed, bool isHunter = false)
	{
		//Body thingsBody = thing.moduleBody.MainBody;
		//var thingsIsee = world.GetSightableThings(thing, (isHunter)? 50: thingsBody.GetSight());
		//Thing bestTargetThing = getBestTargetThing(world, thingsIsee, requiredKeyword, isHunter);
		Thing bestTargetThing = null;
		Keyword keywordSelected = Keyword.UNDEFINED;
		
		getTheBestFoodSourceTarget(world, thing, isHunter,ref  bestTargetThing,ref keywordSelected);

		
		//if (isHunter) UnityEngine.Debug.Log(this + " thingsIsee " + thingsIsee.Count);
		//if (isHunter) UnityEngine.Debug.Log(this + " Resolving hunger " + (bestTargetThing != null));
		if (bestTargetThing == null)
		{
			//UnityEngine.Debug.LogError(this + ""+thing.type +" : " );
			UnityEngine.Debug.LogWarning(this + " "+thing.Category + " : CANNOT BE COMPLETED::BestTargetThing was null");

			//thing.TAM.MoveToRandomLocationOfDistance(world, thing, LOOKING_FOR_FOOD_DISTNACE);
			return false;
		}

		thing.TAM.MoveToTarget(bestTargetThing, thing.GetEatingDistance());
		UnityEngine.Debug.Log(this + " MoveToTarget");
		float desiredKeywordAmount = ( fullfillmentMinimumSatisfaction- fullfillment ) + desiredKeywordTransfer_To_CalmDownDemandCall;
		if (isHunter)
		{
			thing.TAM.Hunt(
				bestTargetThing,
				keywordSelected,
				desiredKeywordAmount
				);
		}
		else
		{
			thing.TAM.Eat(
			   bestTargetThing,
			   keywordSelected,
			   desiredKeywordAmount
			   );

		}
		
		return true;
	}

	public class Resolution {
		public HungerResolutionMethodType resolutionType;
		public Thing targetThing;
		public Keyword keyword;
		public Resolution(HungerResolutionMethodType t, Thing th, Keyword k)
		{
			this.resolutionType = t;
			this.targetThing = th;
			this.keyword = k;
		}

	}

	bool isFullfillmentSatisfied()
	{
		return (fullfillment > fullfillmentMinimumSatisfaction) ;

	}
	public override bool UpdateResolveNeed(World world, ActorBase thing, float timeElapsed)
	{
		if (isFullfillmentSatisfied()) return false;
		List<Resolution> resolutions = new List<Resolution>();
		
		float desiredKeywordAmount = (fullfillmentMinimumSatisfaction-fullfillment) + desiredKeywordTransfer_To_CalmDownDemandCall;

		foreach (var method in methodsAvailable)
		{
			Thing bestTargetThing = null;
			Keyword keywordSelected = Keyword.UNDEFINED;

			switch (method)
			{
				case HungerResolutionMethodType.PASSIVE:
					resolution_passive(world, thing, ref bestTargetThing, ref keywordSelected);
					break;
				case HungerResolutionMethodType.HUNT:
					resolution_hunter(world, thing, ref bestTargetThing, ref keywordSelected);
					break;
				default:
				case HungerResolutionMethodType.STEAL:
				case HungerResolutionMethodType.COOK:
					break;
			}
			if(bestTargetThing != null)
				resolutions.Add(new Resolution( method,  bestTargetThing,  keywordSelected ));
		}
		//Find the best target per method_of_feeding_hunger
		Resolution resolutionSelected = null;
		float score = float.MinValue;
		foreach(var res in resolutions)
		{
			var resScore = calScore(thing , res.resolutionType, res.targetThing, res.keyword);
			if (resScore > score)
			{
				resolutionSelected = res;
				score = resScore;
			}
		}
		if (resolutionSelected != null)
			switch (resolutionSelected.resolutionType)
			{
				case HungerResolutionMethodType.PASSIVE:
					thing.TAM.Eat(
					   resolutionSelected.targetThing,
					   resolutionSelected.keyword,
					   desiredKeywordAmount
					   );
					break;
				case HungerResolutionMethodType.HUNT:
					thing.TAM.Hunt(
					   resolutionSelected.targetThing,
					   resolutionSelected.keyword,
					   desiredKeywordAmount
					   );
					break;
				case HungerResolutionMethodType.STEAL:
					break;
				case HungerResolutionMethodType.COOK:
					break;
				default:
					break;
			}
		else
		{
			UnityEngine.Debug.LogError(this + " : resolutionSelected is null Resolution was " + resolutions.Count + " available methods were " + this.methodsAvailable.Count);
		}
		return false;
	}

	private float calScore(Thing thing, HungerResolutionMethodType resolutionType, Thing targetThing, Keyword keyword)
	{
		return (targetThing.XY - thing.XY).magnitude;
	}

	private void getBestTargetThing(
		World world, List<Thing> thingsIsee, List<Keyword> requiredKeyword,
		ref Thing thingSelected, ref Keyword keywordSelected ,bool hunterMode = false)
	{
		//Thing thingSelected = null;
		//Keyword keywordSelected = Keyword.UNDEFINED;

		float amountOfKeyword_of_thingCurrentlySelected = 0;
		for(int i = 0; i< thingsIsee.Count; i++)
		{
			var thing = thingsIsee[i];
			if (!world.IsWalkableAt(thing.X_INT, thing.Y_INT)) continue;
			List<KeywordInformation> keywordsIGot = thing.GetKeywords();
			if (!hunterMode)
			{
				for(int j = keywordsIGot.Count-1; j>=0 ;j--)
				{
					if(keywordsIGot[j].state == KeywordInformation.State.LOCKED)
					{
						keywordsIGot.RemoveAt(j);
					}
				}
			}
			Dictionary<Keyword, float> thingsKeywords = new Dictionary<Keyword, float>();
			foreach(var info in keywordsIGot)
			{
				if (!thingsKeywords.ContainsKey(info.keyword))
				{
					thingsKeywords.Add(info.keyword, info.amount);
				}
				thingsKeywords[info.keyword] += info.amount;
			}
			List<Keyword> keywordsContained = new List<Keyword>();

			if (!thingsKeywords.ContainsAnyKey(requiredKeyword,ref keywordsContained)) continue;

			foreach(var keyword in keywordsContained)
			{
				if (thingsKeywords[keyword] > amountOfKeyword_of_thingCurrentlySelected)
				{
					thingSelected = thing;
					amountOfKeyword_of_thingCurrentlySelected = thingsKeywords[keyword];
					keywordSelected = keyword;
				}
			}
			

		}
		//return thingSelected;
	}


	public virtual void hdrKeywordConsumed(Thing me, Thing giver, Keyword keyword, float amount)
	{
		if (Game.IsKeywordCompatible(this.requiredKeywords, keyword))
		{
			this.fullfillment += amount;
		}
		if (Game.IsKeywordCompatible(this.stressKeywords, keyword))
		{
			this.fullfillment -= amount;

		}
		//UnityEngine.Debug.Log(this + " demand at  " + this.demand);
	}
}
