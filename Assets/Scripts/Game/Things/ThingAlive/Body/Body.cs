using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Generic class for all body parts
/// </summary>
public class BodyBase
{
	string name = "BodyBase";
	public List<BodyBase> otherBodyParts = new List<BodyBase>();

	public BodyBase SetName(String s)
	{
		this.name = s;
		return this;
	}

	public virtual void Init(Thing thing)
	{
		for(int i = 0; i< otherBodyParts.Count; i++)
		{
			otherBodyParts[i].Init(thing);
		}
	}

	internal void addBody(BodyBase body)
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
	public void GetCertainBodyParts<T>(ref List<BodyBase> bodies)
	{

		foreach(var otherBody in otherBodyParts)
		{
			if (otherBody.GetType().GetGenericTypeDefinition() == typeof(List<T>) )
				bodies.Add(otherBody);
			otherBody.GetCertainBodyParts<T>(ref bodies);
		}
	}
	internal float GetSight()
	{
		List<BodyBase> eyes = new List<BodyBase>();
		GetCertainBodyParts<Eye>(ref eyes);
		float sight = 0;
		foreach(var eye in eyes)
		{
			sight += eye.GetSight();
		}
		//calculate sight by computing all the things that are "eye" in the body
		return sight;
	}


	public virtual void Update(StoryGenerator.World.World world, Thing thing, float timeElapsed)
	{
		for (int i = 0; i < otherBodyParts.Count; i++)
		{
			otherBodyParts[i].Update(world, thing, timeElapsed);
		}

	}

}