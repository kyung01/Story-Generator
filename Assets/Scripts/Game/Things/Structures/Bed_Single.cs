using StoryGenerator.World;
using StoryGenerator.World.Things.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Bed_Single : Bed
{

	public Bed_Single() : base( CAIModelSheet.Bed_Single)
	{
		spotsToSleepIn.Add(new OccupyingSpot(0, 0));
		spotsToEnterBed.Add(new InteractSpot( -1, 0, spotsToSleepIn[0]));
		spotsToEnterBed.Add(new InteractSpot( +1, 0, spotsToSleepIn[0]));
	}

}