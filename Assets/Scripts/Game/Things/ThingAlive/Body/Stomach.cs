using StoryGenerator.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Stomach : Body
{
	static float THRESHOLD = 100;
	float hungerFrustrationRate = 1;
	float hungerIncreaseSpeed = 5;
	float hunger = 0;

	void hdrThingConsumedKeyword(Game.Keyword keyword, float amount)
	{
		if (!Game.IsKeywordCompatible(Game.Keyword.FOOD, keyword)) return;
		hunger -= amount;
	}

	public override void Init(ThingAlive thing)
	{
		base.Init(thing);
		thing.OnConsumeKeyword.Add(hdrThingConsumedKeyword);
	}

	public override void Update(World world, ThingAlive thing, float timeElapsed)
	{
		base.Update(world, thing, timeElapsed);
		float hungerIncreased = hungerIncreaseSpeed * timeElapsed;
		//UnityEngine.Debug.Log(this + " hunger increased " + hungerIncreased);
		hunger += hungerIncreased;
		thing.ConsumeKeyword(Game.Keyword.HUNGER, hungerIncreased);

	}
}
