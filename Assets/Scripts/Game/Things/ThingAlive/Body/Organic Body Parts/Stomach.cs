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
	List<Body> nutritionReceivingBodies = new List<Body>();

	public void addNutrtionBody(Body b)
	{
		nutritionReceivingBodies.Add(b);
	}

	void hdrThingConsumedKeyword(Game.Keyword keyword, float amount)
	{
		if (!Game.IsKeywordCompatible(Game.Keyword.FOOD, keyword)) return;
		for(int i = 0; i < nutritionReceivingBodies.Count; i++)
		{
			//UnityEngine.Debug.Log("hdrThingConsumedKeyword " + Game.IsKeywordCompatible(Game.Keyword.FOOD, keyword) + " " + keyword);
			//UnityEngine.Debug.Log("hdrThingConsumedKeyword "+(amount / nutritionReceivingBodies.Count));
			nutritionReceivingBodies[i].ConsumeKeyword(Game.Keyword.NUTRITION, amount / nutritionReceivingBodies.Count);
		}
		hunger -= amount;
	}

	public override void Init(ThingWithBody thing)
	{
		base.Init(thing);
		thing.OnConsumeKeyword.Add(hdrThingConsumedKeyword);
	}

	public override void Update(World world, ThingWithBody thing, float timeElapsed)
	{
		base.Update(world, thing, timeElapsed);
		float hungerIncreased = hungerIncreaseSpeed * timeElapsed;
		//UnityEngine.Debug.Log(this + " hunger increased " + hungerIncreased);
		hunger += hungerIncreased;
		thing.ConsumeKeyword(Game.Keyword.HUNGER, hungerIncreased);

	}
}
