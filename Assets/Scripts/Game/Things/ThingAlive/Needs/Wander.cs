using StoryGenerator.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Describes a thing's desire to move around
public class Wander : NeedBase
{
	static float DISTANCE_TO_WANDER = 5;
	static float LIMIT_FULLFILLMENT_TO_SEEK_MOVING = 30;
	public Wander()
	{
		this.name = "Wander";
		this.explanation = "Need to move around";
		this.fullfillment = 100;
		this.requiredKeywords.Add( Game.Keyword.MOVED);
		this.stressKeywords.Add(Game.Keyword.STILL);
	}

	public override void Init(Thing thing)
	{
		base.Init(thing);
		thing.OnReceiveKeyword.Add(hdrConsumeKeyword);
	}

	private void hdrConsumeKeyword(Thing me, Thing giver, Game.Keyword keyword, float amount)
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

	public override bool UpdateResolveNeed(World world, Thing thing, float timeElapsed)
	{
		//UnityEngine.Debug.Log("Wander Resolve Need " + demand);
		if (this.fullfillment < LIMIT_FULLFILLMENT_TO_SEEK_MOVING)
		{
			thing.TAM.MoveToRandomLocationOfDistance(world, thing, DISTANCE_TO_WANDER);
		}
		return false;
	}
}