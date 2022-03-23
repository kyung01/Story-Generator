using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Body
{

	public List<Body> otherBodyParts = new List<Body>();
	public virtual void Init(ThingWithBody thing)
	{
		for(int i = 0; i< otherBodyParts.Count; i++)
		{
			otherBodyParts[i].Init(thing);
		}
	}

	internal void addBody(Body body)
	{
		otherBodyParts.Add(body);

	}
	public virtual Dictionary<Game.Keyword,float> GetKeywords()
	{
		var d = new Dictionary<Game.Keyword, float>(); 

		for (int i = 0; i < otherBodyParts.Count; i++)
		{
			var otherD = otherBodyParts[i].GetKeywords();
			foreach (var pair in otherD)
			{
				if (!d.ContainsKey(pair.Key))
				{
					d.Add(pair.Key, 0);

				}
				d[pair.Key] += pair.Value;

			}
		}
		return d;
	}

	public virtual float TakenKeyword(Game.Keyword keywordToRequest, float remainingDebt)
	{
		float needToPay = remainingDebt;
		for(int i = 0; i< otherBodyParts.Count; i++)
		{
			var other = otherBodyParts[i];
			needToPay -= other.TakenKeyword(keywordToRequest, needToPay);
		}
		return 0;
	}

	public virtual void ConsumeKeyword(Game.Keyword keyword, float amount)
	{

	}

	internal float GetSight()
	{
		//calculate sight by computing all the things that are "eye" in the body
		return 5;
	}


	public virtual void Update(StoryGenerator.World.World world, ThingWithBody thing, float timeElapsed)
	{
		for (int i = 0; i < otherBodyParts.Count; i++)
		{
			otherBodyParts[i].Update(world, thing, timeElapsed);
		}

	}

}