using StoryGenerator.World;
using StoryGenerator.World.Things.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public interface ISleepableStructure
{
	/// <summary>
	/// Checks if this structure has available spot to sleep and availalbe positions to enter the bedlike 
	/// If a bed was sleepable, actor will attempt to sleep in it
	/// </summary>
	public bool IsSleepable(World world, ActorBase actorTryingToSleepIn);
	
	public List<Vector2> GetSleepAccessiblePositions(World world, ActorBase actorTryingToSleepIn);
	
	public bool SleepBy(World world, ActorBase sleepingAgent);
}
