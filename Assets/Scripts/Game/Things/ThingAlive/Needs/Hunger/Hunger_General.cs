using StoryGenerator.World;
using System.Collections.Generic;

public class Hunger_General : Need
{
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
	public override void Init(ThingAlive thing)
	{
		base.Init(thing);
		this.demand = demandThreshold+1;
		thing.OnConsumeKeyword.Add( hdrKeywordConsumed);
	}
	public bool passiveResolution(World world, ThingAlive thing, float timeElapsed, bool isHunter = false)
	{
		var thingsIsee = world.GetSightableThings(thing, (isHunter)?20: thing.body.GetSight());
		Thing bestTargetThing = getBestTargetThing(thingsIsee, requiredKeyword, isHunter);
		if (isHunter)
		{
			UnityEngine.Debug.Log("Engaging a hunter mode");
		}
		if (isHunter) UnityEngine.Debug.Log(this + " thingsIsee " + thingsIsee.Count);
		if (isHunter) UnityEngine.Debug.Log(this + " Resolving hunger " + (bestTargetThing != null));
		if (bestTargetThing == null) return false;

		thing.TAM.MoveToTarget(bestTargetThing, thing.GetEatingDistance());
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
			thing.TAM.Eat(
			   bestTargetThing,
			   requiredKeyword,
			   desiredKeywordAmount
			   );

		}
		
		return true;
	}

	public override bool ResolveNeed(World world, ThingAlive thing, float timeElapsed)
	{
		if (demand < demandThreshold) return false;
		if (passiveResolution(world, thing, timeElapsed)){
			return true;
		}
		else if (isHuntersHunger)
			return passiveResolution(world, thing, timeElapsed, isHuntersHunger);
		return false;
	}

	private Thing getBestTargetThing(List<Thing> thingsIsee, Game.Keyword requiredKeyword, bool hunterMode = false)
	{
		Thing thingSelected = null;
		float amountOfKeyword_of_thingCurrentlySelected = 0;
		for(int i = 0; i< thingsIsee.Count; i++)
		{
			var thing = thingsIsee[i];
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
