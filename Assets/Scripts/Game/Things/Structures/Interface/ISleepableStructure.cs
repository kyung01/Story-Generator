using StoryGenerator.World;
using StoryGenerator.World.Things.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

interface ISleepableStructure
{
	public bool IsSleepable(World world, ActorBase thingTryingToSleepIn);
	public List<Vector2> GetSleepablePositions(World world, ActorBase thingTryingToSleepIn);
	public void SleepBy(World world, ActorBase sleepingAgent);
}
