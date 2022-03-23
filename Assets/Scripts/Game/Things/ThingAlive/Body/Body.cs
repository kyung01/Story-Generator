using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Body
{
	public List<Body> otherBodyParts = new List<Body>();
	public virtual void Init(ThingAlive thing)
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
	public virtual void Update(StoryGenerator.World.World world, ThingAlive thing, float timeElapsed)
	{
		for(int i = 0; i < otherBodyParts.Count; i++)
		{
			otherBodyParts[i].Update(world, thing, timeElapsed);
		}

	}

	internal float GetSight()
	{
		//calculate sight by computing all the things that are "eye" in the body
		return 5;
	}
}