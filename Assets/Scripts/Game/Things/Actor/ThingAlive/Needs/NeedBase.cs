using StoryGenerator.World;
using StoryGenerator.World.Things.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class NeedBase
{

	internal string name = "Need Title";
	internal string explanation = "This is a need";

	internal List<Game.Keyword> requiredKeywords = new List<Game.Keyword>();
	internal List<Game.Keyword> stressKeywords = new List<Game.Keyword>();
	internal float fullfillment;


	public virtual void Init(Thing thing)
	{
	}
	public virtual void updateStatic(World world, ActorBase thingAlive, float timeElapsed)
	{
		//UnityEngine.Debug.Log("UpdateStatic " + this.name + " " + this.explanation);
	}

	//Return true if it did resolve
	public virtual bool UpdateResolveNeed(World world, ActorBase thingAlive, float timeElapsed)
	{
		return false;
	}

}
