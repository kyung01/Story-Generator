using StoryGenerator.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Assigns a target to perform the action "eat"
/// If the target is unable to be eatten, it attempts to assign appropriate actions that are required to perform eat
/// </summary>
public class Eat :Action
{
	Thing targetThing;
	Game.Keyword keywordToRequest;
	float keywordAmount ;
	public Eat(Thing thing, Game.Keyword keyword, float amount):base(Type.EAT)
	{
		this.name = "Eat";
		this.targetThing = thing;
		this.keywordToRequest = keyword;
		this.keywordAmount = amount;

	}
	public override void Do(World world, Thing thing, float timeElapsed)
	{
		base.Do(world, thing, timeElapsed);
		//Debug.Log(this + "BEFORE  " + keywordAmount);
		float distance = (thing.XY - targetThing.XY).magnitude;
		if (distance > thing.GetEatingDistance()) finish();
		if (!world.TestLOS(thing, targetThing)) finish();
		if (IsFinished) return;
		float amountIAtePerTick = Mathf.Min(keywordAmount, thing.GetEatingSpeed() * timeElapsed);
		float amountOfKeywordICouldTake = targetThing.Keyword_Taken(keywordToRequest, amountIAtePerTick);
		if(amountOfKeywordICouldTake == 0)
		{
			//There was nothing to eat, I must finish the process
			finish();
			return;
		}
		this.keywordAmount -= amountOfKeywordICouldTake;
		thing.Keyword_Receive(thing, keywordToRequest, amountOfKeywordICouldTake);
		//Debug.Log(this + "AFTER  " + keywordAmount);
		if (this.keywordAmount <= ZEROf)
		{
			finish();
		}



	}
}