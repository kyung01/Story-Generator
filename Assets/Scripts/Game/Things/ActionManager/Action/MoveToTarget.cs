using StoryGenerator.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class MoveToTarget : MoveTo
{
	//position I am trying to go to
	Thing targetThing;
	float distanceMinToDestination;
	public override Vector2 destinationXY
	{
		get
		{
			return targetThing.XY;
		}
	}

	public override bool IsDestinationReached(World world, Thing thing)
	{
		return (thing.XY - targetThing.XY).magnitude <= distanceMinToDestination;
	}


	public MoveToTarget(Thing targetThing, float distanceMinToDestination = 1)
	{
		this.name = "MoveToTarget";
		this.targetThing = targetThing;
		this.distanceMinToDestination = distanceMinToDestination;
	}
	

}
