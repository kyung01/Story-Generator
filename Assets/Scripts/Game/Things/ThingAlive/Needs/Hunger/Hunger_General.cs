using StoryGenerator.World;
using System.Collections.Generic;

public class Hunger_General : Need
{
	float demandThreshold = 100;
	float desiredKeywordTransfer_To_CalmDownDemandCall = 30;
	public Hunger_General()
	{
		this.requiredKeyword = Game.Keyword.FOOD;
		this.stressKeyword = Game.Keyword.HUNGER;
		this.name = "Hunger";
		this.explanation = "A general need for food. It can be satisfied by any consumable that can be calssified as a food.";
	}

	public override bool ResolveNeed(World world, ThingAlive thing, float timeElapsed)
	{
		if (demand < demandThreshold) return false;

		var thingsIsee = world.GetSightableThings(thing, thing.body.GetSight());
		Thing bestTargetThing = getBestTargetThing(thingsIsee, requiredKeyword);
		if (bestTargetThing == null) return false;
		thing.TAM.MoveTo(bestTargetThing);
		thing.TAM.RequestKeywordTransfer(bestTargetThing, requiredKeyword,
			(demand - demandThreshold) + desiredKeywordTransfer_To_CalmDownDemandCall);
		return true;
	}

	private Thing getBestTargetThing(List<Thing> thingsIsee, Game.Keyword requiredKeyword)
	{
		Thing thingSelected = null;
		float amountOfKeyword_of_thingCurrentlySelected = 0;
		for(int i = 0; i< thingsIsee.Count; i++)
		{
			var thing = thingsIsee[i];
			var thingsKeywords = thing.GetKeywords();
			if (!thingsKeywords.ContainsKey(requiredKeyword)) continue;
			if(thingsKeywords[requiredKeyword] > amountOfKeyword_of_thingCurrentlySelected)
			{
				thingSelected = thing;
				amountOfKeyword_of_thingCurrentlySelected = thingsKeywords[requiredKeyword];
			}

		}
		return thingSelected;
	}
}
