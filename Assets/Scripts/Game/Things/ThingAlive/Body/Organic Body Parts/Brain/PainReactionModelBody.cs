using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class PainReactionModelBody : Body
{
	float minimumPainToTrigger = 1;
	float minDistanceToFlee = 5;
	public override void Init(ThingWithBody thing)
	{
		base.Init(thing);
		thing.OnReceiveKeyword.Add(hdrReceiveKeyword);
	}

	
	void hdrReceiveKeyword(Thing me, Thing giver, Game.Keyword keyword, float amount)
	{
		if (keyword != Game.Keyword.PAIN) return;
		//I received pain
		//How should I react
		if(amount>= minimumPainToTrigger)
		{
			TriggerPainReactioin(me,giver);
		}
	}
	public virtual void TriggerPainReactioin(Thing me, Thing giver)
	{
		me.TAM.Flee(giver, minDistanceToFlee);

	}
}