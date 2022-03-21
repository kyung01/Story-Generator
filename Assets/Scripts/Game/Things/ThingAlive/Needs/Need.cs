﻿using StoryGenerator.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Need
{

	internal string name = "Need Title";
	internal string explanation = "This is a need";

	internal Game.Keyword requiredKeyword;
	internal Game.Keyword stressKeyword;
	internal float demand;

	public virtual void Init(ThingAlive thing)
	{

	}
	public virtual void updateStatic(World world, ThingAlive thingAlive, float timeElapsed)
	{
		//UnityEngine.Debug.Log("UpdateStatic " + this.name + " " + this.explanation);
	}

	//Return true if it did resolve
	public virtual bool ResolveNeed(World world, ThingAlive thingAlive, float timeElapsed)
	{
		return false;
	}


	public virtual void hdrKeywordReceived(Game.Keyword keyword, float amount)
	{
		if (Game.IsKeywordCompatible(this.requiredKeyword, keyword))
		{
			this.demand -= amount;
		}
		if (Game.IsKeywordCompatible(this.stressKeyword, keyword))
		{
			this.demand += amount;

		}
	}
}
