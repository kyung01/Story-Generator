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
	float hungerIncreaseSpeed = 1;
	float hunger = 0;

	void hdrThingReceivedKeyword(Game.Keyword keyword, float amount)
	{
		if (!Game.IsKeywordCompatible(Game.Keyword.FOOD, keyword)) return;
		hunger -= amount;
	}

	public override void Init(ThingAlive thing)
	{
		base.Init(thing);
		thing.OnReceiveKeyword.Add(hdrThingReceivedKeyword);
	}

	public override void Update(World world, ThingAlive thing, float timeElapsed)
	{
		base.Update(world, thing, timeElapsed);
		hunger += hungerIncreaseSpeed * timeElapsed;
		if (hunger < THRESHOLD) return;
		thing.ReceiveKeyword(Game.Keyword.HUNGER, timeElapsed * hungerFrustrationRate);

	}
}
