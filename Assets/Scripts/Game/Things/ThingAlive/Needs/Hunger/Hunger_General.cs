﻿using StoryGenerator.World;
using System.Collections.Generic;

public class Hunger_General : Need
{
	const float LOOKING_FOR_FOOD_DISTNACE = 10;
	float demandThreshold = 100;
	float desiredKeywordTransfer_To_CalmDownDemandCall = 30;
	bool isHuntersHunger = false;

	public Hunger_General(bool isHuntersHunger = false)
	{
		this.isHuntersHunger = isHuntersHunger;
		this.requiredKeyword = Game.Keyword.FOOD;
		this.stressKeyword = Game.Keyword.HUNGER;
		this.name = "Hunger";
		this.explanation = "A general need for food. It can be satisfied by any consumable that can be calssified as a food.";
	}
	public override void Init(ThingWithNeeds thing)
	{
		base.Init(thing);
		this.demand = demandThreshold+1;
		thing.OnConsumeKeyword.Add( hdrKeywordConsumed);
	}
	public bool passiveResolution(World world, ThingWithNeeds thing, float timeElapsed, bool isHunter = false)
	{
		var thingsIsee = world.GetSightableThings(thing, (isHunter)?50: thing.body.GetSight());
		Thing bestTargetThing = getBestTargetThing(world, thingsIsee, requiredKeyword, isHunter);
		if (isHunter)
		{
			UnityEngine.Debug.Log("Engaging a hunter mode");
		}
		if (isHunter) UnityEngine.Debug.Log(this + " thingsIsee " + thingsIsee.Count);
		if (isHunter) UnityEngine.Debug.Log(this + " Resolving hunger " + (bestTargetThing != null));
		if (bestTargetThing == null)
		{
			UnityEngine.Debug.LogError(this + " CANNOT BE COMPLETED::BestTargetThing was null");

			//thing.TAM.MoveToRandomLocationOfDistance(world, thing, LOOKING_FOR_FOOD_DISTNACE);
			return false;
		}

		thing.TAM.MoveToTarget(bestTargetThing, thing.GetEatingDistance());
		UnityEngine.Debug.Log(this + " MoveToTarget");
		float desiredKeywordAmount = (demand - demandThreshold) + desiredKeywordTransfer_To_CalmDownDemandCall;
		if (isHunter)
		{
			UnityEngine.Debug.Log("Hunt TAM called");
			thing.TAM.Hunt(
				bestTargetThing,
				requiredKeyword,
				desiredKeywordAmount
				);
		}
		else
		{
			UnityEngine.Debug.Log(this + " Eat " + bestTargetThing + bestTargetThing.XY);
			thing.TAM.Eat(
			   bestTargetThing,
			   requiredKeyword,
			   desiredKeywordAmount
			   );

		}
		
		return true;
	}

	public override bool ResolveNeed(World world, ThingWithNeeds thing, float timeElapsed)
	{
		if (demand < demandThreshold) return false;
		if (passiveResolution(world, thing, timeElapsed)){
			return true;
		}
		else if (isHuntersHunger)
			return passiveResolution(world, thing, timeElapsed, isHuntersHunger);
		return false;
	}

	private Thing getBestTargetThing(World world, List<Thing> thingsIsee, Game.Keyword requiredKeyword, bool hunterMode = false)
	{
		Thing thingSelected = null;
		float amountOfKeyword_of_thingCurrentlySelected = 0;
		for(int i = 0; i< thingsIsee.Count; i++)
		{
			var thing = thingsIsee[i];
			if (!world.IsWalkableAt(thing.X_INT, thing.Y_INT)) continue;
			Dictionary<Game.Keyword, float> thingsKeywords = (hunterMode) ? thing.GetKeywordsForHunter():thing.GetKeywords();
			
			if (!thingsKeywords.ContainsKey(requiredKeyword)) continue;
			if(thingsKeywords[requiredKeyword] > amountOfKeyword_of_thingCurrentlySelected)
			{
				thingSelected = thing;
				amountOfKeyword_of_thingCurrentlySelected = thingsKeywords[requiredKeyword];
			}

		}
		return thingSelected;
	}


	public virtual void hdrKeywordConsumed(Game.Keyword keyword, float amount)
	{
		if (Game.IsKeywordCompatible(this.requiredKeyword, keyword))
		{
			this.demand -= amount;
		}
		if (Game.IsKeywordCompatible(this.stressKeyword, keyword))
		{
			this.demand += amount;

		}
		//UnityEngine.Debug.Log(this + " demand at  " + this.demand);
	}
}
