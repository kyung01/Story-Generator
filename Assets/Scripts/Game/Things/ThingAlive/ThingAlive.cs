using StoryGenerator.World;
using System;
using System.Collections.Generic;
using UnityEngine;



public class ThingAlive : ThingWithBody
{
	public List<Need> needs = new List<Need>();

	public override bool IsReadyToProvideWithBody
	{
		get
		{
			return health < 30;
		}

	}

	public void addNeed(Need n)
	{
		//n.Init(this);
		needs.Add(n);

	}
	public override void Init(World world)
	{
		base.Init(world);
		for(int i = 0; i < needs.Count; i++)
		{
			needs[i].Init(this);
		}
	}

	public override void Update(World world, float timeElapsed)
	{
		base.Update(world, timeElapsed);
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