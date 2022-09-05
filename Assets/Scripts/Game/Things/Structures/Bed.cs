﻿using StoryGenerator.World;
using StoryGenerator.World.Things.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Bed : Structure, ISleepableStructure
{
	internal List<InteractSpot> spotsToEnterBed = new List<InteractSpot>();
	internal List<OccupyingSpot> spotsToSleepIn = new List<OccupyingSpot>();

	public Bed(CAIModel model) : base(GameEnums.CATEGORY.BED, model)
	{
	}

	public virtual List<Vector2> GetSleepablePositions(World world, ActorBase actor)
	{
		List<Vector2> sleepingPositions = new List<Vector2>();
		for (int i = 0; i < spotsToEnterBed.Count; i++)
		{
			if (spotsToEnterBed[i].IsAvailableForConsideration(world, this))
			{
				int x, y;
				spotsToEnterBed[i].GetInteractionXY(this, out x,out y);
				sleepingPositions.Add(new Vector2(x, y));
			}
		}
		return sleepingPositions;
	}

	public virtual bool IsSleepable(World world, ActorBase thingTryingToSleepIn)
	{
		foreach(var spot in spotsToEnterBed)
		{
			if (spot.IsAvailableForConsideration(world, this)) return true;

		}
		return false;
	}

	public virtual void SleepBy(World world, ActorBase sleepingAgent)
	{
		for(int i  = 0; i < spotsToEnterBed.Count; i++)
		{
			if (spotsToEnterBed[i].CanInteractWithIt(world, this, sleepingAgent))
			{
				spotsToEnterBed[i].Interact(this, sleepingAgent);
				return;
			}
		}
		Debug.LogError("Bed unexpected end of function reached");
		throw new NotImplementedException();
	}
}