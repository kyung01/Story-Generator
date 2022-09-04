using StoryGenerator.World;
using GameEnums;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Stomach : BodyBase
{
	//static float THRESHOLD = 100;
	//float hungerFrustrationRate = 1;
	float hungerIncreaseSpeed = 5;
	float hunger = 0;
	List<BodyBase> nutritionReceivingBodies = new List<BodyBase>();
	public Stomach()
	{

		this.type = Type.STOMACH;
	}
	public void addNutrtionBody(BodyBase b)
	{
		nutritionReceivingBodies.Add(b);
	}

	void hdrThingConsumedKeyword(Thing me, Thing giver, Keyword keyword, float amount)
	{
		if (!Game.IsKeywordCompatible(Keyword.FOOD, keyword)) return;
		for(int i = 0; i < nutritionReceivingBodies.Count; i++)
		{
			//UnityEngine.Debug.Log("hdrThingConsumedKeyword " + Game.IsKeywordCompatible(Keyword.FOOD, keyword) + " " + keyword);
			//UnityEngine.Debug.Log("hdrThingConsumedKeyword "+(amount / nutritionReceivingBodies.Count));
			nutritionReceivingBodies[i].ConsumeKeyword(Keyword.NUTRITION, amount / nutritionReceivingBodies.Count);
		}
		this.hunger -= amount;
	}

	public override void Init(Thing thing)
	{
		base.Init(thing);
		thing.OnReceiveKeyword.Add(hdrThingConsumedKeyword);
	}

	public override void Update(World world, Thing thing, float timeElapsed)
	{
		base.Update(world, thing, timeElapsed);
		float hungerIncreased = hungerIncreaseSpeed * timeElapsed;
		//UnityEngine.Debug.Log(this + " hunger increased " + hungerIncreased);
		hunger += hungerIncreased;
		thing.Keyword_Receive(thing, Keyword.HUNGER, hungerIncreased);

	}
}
