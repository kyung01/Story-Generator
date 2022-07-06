using StoryGenerator.World;
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
	public override void Init(Thing thing)
	{
		base.Init(thing);
		this.demand = demandThreshold+1;
		thing.OnConsumeKeyword.Add( hdrKeywordConsumed);
	}
	Thing getTheBestFoodSourceTarget(World world, Thing thing, bool isHunter)
	{
		Body thingsBody = thing.moduleBody.MainBody;
		var thingsIsee = world.GetSightableThings(thing, (isHunter) ? 50 : thingsBody.GetSight());
		return getBestTargetThing(world, thingsIsee, requiredKeyword, isHunter);
	}

	//Passive resolution is to eat something that's available freely
	public bool resolution_passive(World world, Thing thing, ref Thing targetThing)
	{
		return false;
	}

	//Cooker's resolution is to create a possibly something I need from the available resources around me 
	public bool resolution_cook(World world, Thing thing, ref Thing targetThing)
	{

		return false;
	}

	//Hunter's resolutiuon is to hunt an animal/target and gain the resource it requires
	public bool resolution_hunter()
	{

		return false;
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
		var bestTargetThing = getTheBestFoodSourceTarget(world, thing, isHunter);

		
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

	public override bool ResolveNeed(World world, Thing thing, float timeElapsed)
	{
		if (demand < demandThreshold) return false;
		if (eatResolution(world, thing, timeElapsed)){
			return true;
		}
		else if (isHuntersHunger)
			return eatResolution(world, thing, timeElapsed, true);
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
