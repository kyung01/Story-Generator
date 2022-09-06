using UnityEngine;
using System.Collections;
using StoryGenerator.World;
using GameEnums;

public class Eye : BodyBase
{
	float sight;
	float vitaminALevel = 10;
	float vitaminDeficiantRatio = .1f;

	public float Sight { get { return this.sight; } }

	public Eye(float sight = 10.0f)
	{
		this.sight = sight;
		this.type = Type.EYE;
	}
	public override void ConsumeKeyword(Keyword keyword, float amount)
	{
		base.ConsumeKeyword(keyword, amount);
		if(keyword == Keyword.VITAMIN_A)
		{
			//
			vitaminALevel += amount;
		}
	}
	public override void Update(World world, Thing thing, float timeElapsed)
	{
		base.Update(world, thing, timeElapsed);
		vitaminALevel += -1*vitaminDeficiantRatio * timeElapsed ;
		if(vitaminALevel < 0)
		{
			vitaminALevel = 0;
			eyeProblem(world,thing,timeElapsed);
		}
	}

	private void eyeProblem(World world, Thing thing, float timeElapsed)
	{
		//sight = Mathf.Max(sight - timeElapsed, 2);
	}
}
