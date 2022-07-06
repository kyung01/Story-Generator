using StoryGenerator.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Hunt : Action
{
	Thing targetThing;
	Game.Keyword keywordToRequest;
	float keywordAmountToRequest;

	public Hunt(Thing thing, Game.Keyword keyword, float amount)
	{
		this.name = "Hunt";
		this.targetThing = thing;
		this.keywordToRequest = keyword;
		this.keywordAmountToRequest = amount;

	}
	public override void Do(World world, Thing thing, float timeElapsed)
	{
		base.Do(world, thing, timeElapsed);
		float attackDistance = 1;

		//Debug.Log(this + "BEFORE  " + keywordAmountToRequest);
		float distance = (thing.XY - targetThing.XY).magnitude;
		if (distance > attackDistance)
		{
			thing.TAM.MoveToTarget(targetThing, attackDistance, ThingActionManager.PriorityLevel.FIRST);
			return;
		}
		//I am close enough to bite now
		
		var taskableBodies = thing.GetBodiesForTask();
		bool isNoMethodOfAttacking = !taskableBodies.ContainsKey(Game.TaskType.BITE)|| taskableBodies[Game.TaskType.BITE].Count == 0;
		if (isNoMethodOfAttacking)
		{
			Debug.Log("Hunt finished : no way to attack");
			finish();
			return;
		}
		var bitingParts = taskableBodies[Game.TaskType.BITE];
		bool targetIsInSurrenderingState = false;
		for(int i = 0; i< bitingParts.Count; i++)
		{
			if (bitingParts[i].IsReady)
			{
				((Mouth)bitingParts[i]).Bite(thing,targetThing);
				if (targetThing.moduleBody.IsBodyAvailableForKeywordExchanges())
				{
					targetIsInSurrenderingState = true;
					break;
				}
			}
		}
		if (targetIsInSurrenderingState)
		{
			targetThing.TakenKeyword(keywordToRequest, keywordAmountToRequest);

		}



	}

	BodyTaskable getBestBodyPartToAttack(List<BodyTaskable> bitingParts)
	{
		BodyTaskable selected= null;
		float score = 0;
		for(int i = 0; i < bitingParts.Count; i++)
		{
			var mouth = (Mouth)bitingParts[i];
			if(mouth.JawPower > score){
				selected = mouth;
				score = mouth.JawPower;
			}

		}
		return selected;

	}
}