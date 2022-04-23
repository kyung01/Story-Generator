using StoryGenerator.World;
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

	public virtual void Init(ThingWithNeeds thing)
	{

	}
	public virtual void updateStatic(World world, ThingWithNeeds thingAlive, float timeElapsed)
	{
		//UnityEngine.Debug.Log("UpdateStatic " + this.name + " " + this.explanation);
	}

	//Return true if it did resolve
	public virtual bool ResolveNeed(World world, ThingWithNeeds thingAlive, float timeElapsed)
	{
		return false;
	}

}
