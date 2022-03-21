using StoryGenerator.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Describes a thing's desire to move around
public class Wander : Need
{
	static float DISTANCE_TO_WANDER = 5;
	public Wander()
	{
		this.demand = 100;
		this.requiredKeyword = Game.Keyword.MOVED;
		this.stressKeyword = Game.Keyword.STILL;
	}
	public override bool ResolveNeed(World world, ThingAlive thing, float timeElapsed)
	{
		//UnityEngine.Debug.Log("Wander Resolve Need " + demand);
		if (this.demand > 100)
		{
			thing.TAM.MoveToRandomLocationOfDistance(world, thing, DISTANCE_TO_WANDER);
		}
		return false;
	}
}