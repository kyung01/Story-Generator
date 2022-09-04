using StoryGenerator.World;
using StoryGenerator.World.Things.Actors;
using GameEnums;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class NeedBase
{

	internal string name = "Need Title";
	internal string explanation = "This is a need";

	internal List<Keyword> requiredKeywords = new List<Keyword>();
	internal List<Keyword> stressKeywords = new List<Keyword>();
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
