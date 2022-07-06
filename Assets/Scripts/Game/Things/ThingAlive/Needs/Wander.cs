using StoryGenerator.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Describes a thing's desire to move around
public class Wander : Need
{
	static float DISTANCE_TO_WANDER = 5;
	public Wander()
	{
		this.name = "Wander";
		this.explanation = "Need to move around";
		this.demand = 100;
		this.requiredKeywords.Add( Game.Keyword.MOVED);
		this.stressKeywords.Add(Game.Keyword.STILL);
	}

	public override void Init(Thing thing)
	{
		base.Init(thing);
		thing.OnConsumeKeyword.Add(hdrConsumeKeyword);
	}

	private void hdrConsumeKeyword(Game.Keyword keyword, float amount)
	{
		if (Game.IsKeywordCompatible(this.requiredKeywords, keyword))
		{
			this.demand -= amount;
		}
		if (Game.IsKeywordCompatible(this.stressKeywords, keyword))
		{
			this.demand += amount;

		}
	}

	public override bool ResolveNeed(World world, Thing thing, float timeElapsed)
	{
		//UnityEngine.Debug.Log("Wander Resolve Need " + demand);
		if (this.demand > 100)
		{
			thing.TAM.MoveToRandomLocationOfDistance(world, thing, DISTANCE_TO_WANDER);
		}
		return false;
	}
}