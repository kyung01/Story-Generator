using GameEnums;
using System.Collections.Generic;
using System;

/// <summary>
/// Generic class for all body parts
/// </summary>
public class BodyBase
{
	public enum Type { BASE, EYE,HAND,MEATBODY,MOUTH,STOMACH,
		BRAIN_MOTION_DEMANDER,
		BRAIN_PAIN_CREATOR
	}

	string name = "BodyBase";
	internal Type type = BodyBase.Type.BASE;
	public Type T { get { return this.type; } }

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
	public virtual Dictionary<Keyword,float> GetKeywords()
	{
		var d = new Dictionary<Keyword, float>(); 

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

	public virtual float TakenKeyword(Keyword keywordToRequest, float remainingDebt)
	{
		float needToPay = remainingDebt;
		for(int i = 0; i< otherBodyParts.Count; i++)
		{
			var other = otherBodyParts[i];
			needToPay -= other.TakenKeyword(keywordToRequest, needToPay);
		}
		return 0;
	}

	public virtual void ConsumeKeyword(Keyword keyword, float amount)
	{

	}
	public void GetCertainBodyParts(ref List<BodyBase> bodies, BodyBase.Type type)
	{

		foreach(var otherBody in otherBodyParts)
		{
			if (otherBody.T== type )
				bodies.Add(otherBody);
			else
			{
				//UnityEngine.Debug.Log(otherBody.T);
			}
			otherBody.GetCertainBodyParts(ref bodies,type);
		}
	}
	internal float GetSight()
	{
		List<BodyBase> eyes = new List<BodyBase>();
		GetCertainBodyParts(ref eyes, BodyBase.Type.EYE);
		float sight = (this.type == Type.EYE)? ((Eye)this).Sight: 0;
		foreach(var eye in eyes)
		{
			sight += eye.GetSight();
		}
		//UnityEngine.Debug.Log(sight);
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