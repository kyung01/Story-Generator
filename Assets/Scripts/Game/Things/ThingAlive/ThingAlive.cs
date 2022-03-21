using StoryGenerator.World;
using System;
using System.Collections.Generic;
using UnityEngine;



public class ThingAlive : ThingDestructable
{
	internal Body body = new Body();
	List<Need> needs = new List<Need>();


	public void addNeed(Need n)
	{
		//n.Init(this);
		this.OnReceiveKeyword.Add(n.hdrKeywordReceived);
		needs.Add(n);

	}
	public override void Init(World world)
	{
		base.Init(world);
		for(int i = 0; i < needs.Count; i++)
		{
			needs[i].Init(this);
		}
		this.body.Init(this);
	}

	public override void Update(World world, float timeElapsed)
	{
		base.Update(world, timeElapsed);
		body.Update(world, this, timeElapsed);
		for (int i = 0; i < needs.Count; i++)
		{
			needs[i].updateStatic(world, this, timeElapsed);
		}

		if (TAM.IsIdl)
		{
			for (int i = 0; i < needs.Count; i++)
			{
				if (needs[i].ResolveNeed(world, this, timeElapsed))
				{
					var resolvingNeed = needs[i];
					needs.RemoveAt(i);
					needs.Add(resolvingNeed);
				}
			}
		}
		
	}

}