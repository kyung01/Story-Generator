using StoryGenerator.World;
using StoryGenerator.World.Things.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

interface ISitableStructure
{
	public bool				 IsSitableBy(World world, ActorBase actor);
	public List<Vector2> GetSitablePositions(World world, ActorBase actor);
	public void SitBy(ActorBase actor);
}
