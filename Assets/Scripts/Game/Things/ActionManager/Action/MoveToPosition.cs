using StoryGenerator.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class MoveToPosition : MoveTo
{
	Vector2 target;
	public override Vector2 destinationXY
	{
		get
		{
			return target;
		}
	}
	public override bool IsDestinationReached(World world, Thing thing)
	{
		return (thing.XY - target).magnitude <= ZEROf;
	}
	public MoveToPosition(float x, float y)
	{
		this.name = "MoveToPosition";
		target = new Vector2(x, y);


	}
}
