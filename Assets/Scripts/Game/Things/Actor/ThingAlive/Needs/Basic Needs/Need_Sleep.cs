using GameEnums;
using StoryGenerator.World;
using StoryGenerator.World.Things.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class NeedSleepHere : NeedBase
{
	public NeedSleepHere(Person person)
	{
		Init(person);

		this.name = "Sleep";
		this.explanation = "Actor needs to sleep periodically.";
		this.fullfillment = 55;
		this.requiredKeywords.Add(Keyword.SLEEP);
		this.stressKeywords.Add(Keyword.CHEM_SLEEPY);
	}
	public override void Init(Thing thing)
	{
		base.Init(thing);
		thing.OnReceiveKeyword.Add(hdrThingReceivedKeywords);
	}

	private void hdrThingReceivedKeywords(Thing me, Thing giver, Keyword keyword, float amount)
	{
		if (Game.IsKeywordCompatible(this.requiredKeywords, keyword))
		{
			this.fullfillment += amount;
		}
		if (Game.IsKeywordCompatible(this.stressKeywords, keyword))
		{
			this.fullfillment -= amount;

		}
	}


	public override bool UpdateResolveNeed(World world, ActorBase thing, float timeElapsed)
	{
		//UnityEngine.Debug.Log("Wander Resolve Need " + demand);
		if (this.fullfillment > 50) return false;
		if (thing.DoSleep(world)) return true;

		

		return false;
	}
}