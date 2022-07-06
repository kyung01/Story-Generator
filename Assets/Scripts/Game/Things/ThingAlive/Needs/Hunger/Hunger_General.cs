using StoryGenerator.World;
using System;
using System.Collections.Generic;

public partial class Hunger_General : Need { 

	public static Hunger_General InitSimpleHunger(Game.Keyword keywordRequired, params HungerResolutionMethodType[] arr)
	{
		Hunger_General hungerNeed = new Hunger_General();
		hungerNeed.requiredKeywords.Add(keywordRequired);
		hungerNeed.stressKeywords.Add(Game.Keyword.HUNGER);
		hungerNeed.methodsAvailable.AddRange(arr);
		return hungerNeed;
	}
}

public partial class Hunger_General : Need
{
	public enum HungerResolutionMethodType { PASSIVE,HUNT,STEAL,COOK}

	const float LOOKING_FOR_FOOD_DISTNACE = 10;
	float demandThreshold = 100;
	float desiredKeywordTransfer_To_CalmDownDemandCall = 30;
	//bool isHuntersHunger = false;
	List<HungerResolutionMethodType> methodsAvailable = new List<HungerResolutionMethodType>();

	public Hunger_General(params HungerResolutionMethodType[] arr)
	{
		methodsAvailable.AddRange(arr);
		//this.isHuntersHunger = isHuntersHunger;
		//this.requiredKeywords.Add(Game.Keyword.FOOD);
		//this.stressKeywords.Add( Game.Keyword.HUNGER);

		this.name = "Hunger";
		this.explanation = "A general need for food. It can be satisfied by any consumable that can be calssified as a food.";
	}

	public override void Init(Thing thing)
	{
		base.Init(thing);
		this.demand = demandThreshold+1;
		thing.OnConsumeKeyword.Add( hdrKeywordConsumed);
	}

	void getTheBestFoodSourceTarget(World world, Thing thing, bool isHunter, ref Thing targetThing, ref Game.Keyword keywordSelected)
	{
		Body thingsBody = thing.moduleBody.MainBody;
		var thingsIsee = world.GetSightableThings(thing, (isHunter) ? 50 : thingsBody.GetSight());
		getBestTargetThing(world, thingsIsee, requiredKeywords,ref targetThing,ref keywordSelected, isHunter);
	}

	//Passive resolution is to eat something that's available freely
	public bool resolution_passive(World world, Thing thing, ref Thing bestTargetThing, ref Game.Keyword keywordSelected)
	{
		getTheBestFoodSourceTarget(world, thing, false, ref bestTargetThing, ref keywordSelected);


		//if (isHunter) UnityEngine.Debug.Log(this + " thingsIsee " + thingsIsee.Count);
		//if (isHunter) UnityEngine.Debug.Log(this + " Resolving hunger " + (bestTargetThing != null));
		if (bestTargetThing == null)
		{
			//UnityEngine.Debug.LogError(this + ""+thing.type +" : " );
			UnityEngine.Debug.LogWarning(this + " " + thing.type + " : CANNOT BE COMPLETED::BestTargetThing was null");

			//thing.TAM.MoveToRandomLocationOfDistance(world, thing, LOOKING_FOR_FOOD_DISTNACE);
			return false;
		}

		thing.TAM.MoveToTarget(bestTargetThing, thing.GetEatingDistance());
		//UnityEngine.Debug.Log(this + " MoveToTarget");
		float desiredKeywordAmount = (demand - demandThreshold) + desiredKeywordTransfer_To_CalmDownDemandCall;
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
	public bool resolution_hunter(World world, Thing thing, ref Thing bestTargetThing, ref Game.Keyword keywordSelected)
	{

		getTheBestFoodSourceTarget(world, thing, true, ref bestTargetThing, ref keywordSelected);


		//if (isHunter) UnityEngine.Debug.Log(this + " thingsIsee " + thingsIsee.Count);
		//if (isHunter) UnityEngine.Debug.Log(this + " Resolving hunger " + (bestTargetThing != null));
		if (bestTargetThing == null)
		{
			//UnityEngine.Debug.LogError(this + ""+thing.type +" : " );
			UnityEngine.Debug.LogWarning(this + " " + thing.type + " : CANNOT BE COMPLETED::BestTargetThing was null");

			//thing.TAM.MoveToRandomLocationOfDistance(world, thing, LOOKING_FOR_FOOD_DISTNACE);
			return false;
		}

		thing.TAM.MoveToTarget(bestTargetThing, thing.GetEatingDistance());
		UnityEngine.Debug.Log(this + " MoveToTarget");
		float desiredKeywordAmount = (demand - demandThreshold) + desiredKeywordTransfer_To_CalmDownDemandCall;
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
	public bool eatResolution(World world, Thing thing, float timeElapsed, bool isHunter = false)
	{
		//Body thingsBody = thing.moduleBody.MainBody;
		//var thingsIsee = world.GetSightableThings(thing, (isHunter)? 50: thingsBody.GetSight());
		//Thing bestTargetThing = getBestTargetThing(world, thingsIsee, requiredKeyword, isHunter);
		Thing bestTargetThing = null;
		Game.Keyword keywordSelected = Game.Keyword.UNDEFINED;
		
		getTheBestFoodSourceTarget(world, thing, isHunter,ref  bestTargetThing,ref keywordSelected);

		
		//if (isHunter) UnityEngine.Debug.Log(this + " thingsIsee " + thingsIsee.Count);
		//if (isHunter) UnityEngine.Debug.Log(this + " Resolving hunger " + (bestTargetThing != null));
		if (bestTargetThing == null)
		{
			//UnityEngine.Debug.LogError(this + ""+thing.type +" : " );
			UnityEngine.Debug.LogWarning(this + " "+thing.type + " : CANNOT BE COMPLETED::BestTargetThing was null");

			//thing.TAM.MoveToRandomLocationOfDistance(world, thing, LOOKING_FOR_FOOD_DISTNACE);
			return false;
		}

		thing.TAM.MoveToTarget(bestTargetThing, thing.GetEatingDistance());
		UnityEngine.Debug.Log(this + " MoveToTarget");
		float desiredKeywordAmount = (demand - demandThreshold) + desiredKeywordTransfer_To_CalmDownDemandCall;
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
	public struct Resolution {
		public HungerResolutionMethodType resolutionType;
		public Thing targetThing;
		public Game.Keyword keyword;
		public Resolution(HungerResolutionMethodType t, Thing th, Game.Keyword k)
		{
			this.resolutionType = t;
			this.targetThing = th;
			this.keyword = k;
		}

	}


	public override bool ResolveNeed(World world, Thing thing, float timeElapsed)
	{
		if (demand < demandThreshold) return false;
		List<Resolution> resolutions = new List<Resolution>();

		float desiredKeywordAmount = (demand - demandThreshold) + desiredKeywordTransfer_To_CalmDownDemandCall;

		foreach (var method in methodsAvailable)
		{
			Thing bestTargetThing = null;
			Game.Keyword keywordSelected = Game.Keyword.UNDEFINED;

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
				resolutions.Add(new Resolution() { resolutionType = method, targetThing = bestTargetThing, keyword = keywordSelected });
		}
		//Find the best target per method_of_feeding_hunger
		Resolution? resolutionSelected = null;
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
			switch (resolutionSelected.Value.resolutionType)
			{
				case HungerResolutionMethodType.PASSIVE:
					thing.TAM.Eat(
					   resolutionSelected.Value.targetThing,
					   resolutionSelected.Value.keyword,
					   desiredKeywordAmount
					   );
					break;
				case HungerResolutionMethodType.HUNT:
					thing.TAM.Hunt(
					   resolutionSelected.Value.targetThing,
					   resolutionSelected.Value.keyword,
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

	private float calScore(Thing thing, HungerResolutionMethodType resolutionType, Thing targetThing, Game.Keyword keyword)
	{
		return (targetThing.XY - thing.XY).magnitude;
	}

	private void getBestTargetThing(World world, List<Thing> thingsIsee, List<Game.Keyword> requiredKeyword,ref Thing thingSelected, ref Game.Keyword keywordSelected ,bool hunterMode = false)
	{
		//Thing thingSelected = null;
		//Game.Keyword keywordSelected = Game.Keyword.UNDEFINED;

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
			Dictionary<Game.Keyword, float> thingsKeywords = new Dictionary<Game.Keyword, float>();
			foreach(var info in keywordsIGot)
			{
				if (!thingsKeywords.ContainsKey(info.keyword))
				{
					thingsKeywords.Add(info.keyword, info.amount);
				}
				thingsKeywords[info.keyword] += info.amount;
			}
			List<Game.Keyword> keywordsContained = new List<Game.Keyword>();

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


	public virtual void hdrKeywordConsumed(Game.Keyword keyword, float amount)
	{
		if (Game.IsKeywordCompatible(this.requiredKeywords, keyword))
		{
			this.demand -= amount;
		}
		if (Game.IsKeywordCompatible(this.stressKeywords, keyword))
		{
			this.demand += amount;

		}
		//UnityEngine.Debug.Log(this + " demand at  " + this.demand);
	}
}
