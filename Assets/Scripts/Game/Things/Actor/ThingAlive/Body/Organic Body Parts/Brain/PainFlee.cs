using StoryGenerator.World.Things.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class PainFlee : PainReactionModelBody
{
	float minFleeDistance = 5;

	public override void TriggerPainReactioin(ActorBase me, Thing giver)
	{
		base.TriggerPainReactioin(me, giver);
		me.TAM.Flee(giver, minFleeDistance);
	}
}
