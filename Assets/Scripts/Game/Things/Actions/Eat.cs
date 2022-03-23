using StoryGenerator.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Eat :Action
{
	Thing targetThing;
	Game.Keyword keywordToRequest;
	float keywordAmount ;
	public Eat(Thing thing, Game.Keyword keyword, float amount)
	{
		this.name = "Eat";
		this.targetThing = thing;
		this.keywordToRequest = keyword;
		this.keywordAmount = amount;

	}
	public override void Update(World world, Thing thing, float timeElapsed)
	{
		base.Update(world, thing, timeElapsed);
		Debug.Log(this + "BEFORE  " + keywordAmount);
		float distance = (thing.XY - targetThing.XY).magnitude;
		if (distance > thing.GetEatingDistance()) finish();
		if (!world.TestLOS(thing, targetThing)) finish();
		if (IsFinished) return;
		float amountIAtePerTick = Mathf.Min(keywordAmount, thing.GetEatingSpeed() * timeElapsed);
		float amountOfKeywordICouldTake = targetThing.TakenKeyword(keywordToRequest, amountIAtePerTick);
		if(amountOfKeywordICouldTake == 0)
		{
			//There was nothing to eat, I must finish the process
			finish();
			return;
		}
		this.keywordAmount -= amountOfKeywordICouldTake;
		thing.ConsumeKeyword(keywordToRequest, amountOfKeywordICouldTake);
		Debug.Log(this + "AFTER  " + keywordAmount);
		if (this.keywordAmount <= ZEROf)
		{
			finish();
		}



	}
}