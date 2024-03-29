﻿using StoryGenerator.World.Things.Actors;
using GameEnums;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class PainReactionModelBody : BodyBase
{
	float minimumPainToTrigger = 1;
	float minDistanceToFlee = 10;
	public override void Init(Thing thing)
	{
		base.Init(thing);
		thing.OnReceiveKeyword.Add(hdrReceiveKeyword);
	}

	
	void hdrReceiveKeyword(Thing me, Thing giver, Keyword keyword, float amount)
	{
		//UnityEngine.Debug.Log(this + " flee pain reaction model " + amount);
		if (keyword != Keyword.PAIN) return;
		//UnityEngine.Debug.Log(this + " flee pain reaction model Not returned");
		//I received pain
		//How should I react
		if (amount>= minimumPainToTrigger)
		{
			//UnityEngine.Debug.Log(this + " flee pain reaction :: TriggerPainReactioin");
			TriggerPainReactioin((ActorBase)me, giver);
		}
	}
	public virtual void TriggerPainReactioin(ActorBase me, Thing giver)
	{
		me.TAM.Flee(giver, minDistanceToFlee, ThingActionManager.PriorityLevel.FOCUSE);

	}
}