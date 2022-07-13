using System;
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

	
	void hdrReceiveKeyword(Thing me, Thing giver, Game.Keyword keyword, float amount)
	{
		//UnityEngine.Debug.Log(this + " flee pain reaction model " + amount);
		if (keyword != Game.Keyword.PAIN) return;
		//UnityEngine.Debug.Log(this + " flee pain reaction model Not returned");
		//I received pain
		//How should I react
		if (amount>= minimumPainToTrigger)
		{
			//UnityEngine.Debug.Log(this + " flee pain reaction :: TriggerPainReactioin");
			TriggerPainReactioin(me,giver);
		}
	}
	public virtual void TriggerPainReactioin(Thing me, Thing giver)
	{
		me.TAM.Flee(giver, minDistanceToFlee, ThingActionManager.PriorityLevel.FOCUSE);

	}
}