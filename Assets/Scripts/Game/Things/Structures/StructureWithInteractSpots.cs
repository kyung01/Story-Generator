using GameEnums;
using StoryGenerator.World;
using StoryGenerator.World.Things.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class StructureWithInteractSpots : Structure
{
	internal List<InteractSpot> spotsToEnter = new List<InteractSpot>();
	internal List<OccupyingSpot> spotsToOccupy = new List<OccupyingSpot>();

	public StructureWithInteractSpots(ThingCategory category, CAIModel model) : base(category, model)
	{
	}

	public List<Vector2> GetAvailableInteractionSpot(World world, params InteractSpot[] spots)
	{
		List<Vector2> availableSpots = new List<Vector2>();
		for (int i = 0; i < spotsToEnter.Count; i++)
		{
			if (spotsToEnter[i].IsAvailableForConsideration(world, this))
			{
				int x, y;
				spotsToEnter[i].GetInteractionXY(this, out x, out y);
				availableSpots.Add(new Vector2(x, y));
			}
		}
		return availableSpots;
	}

	public override bool RequestUnInteract(Thing_Interactable thing)
	{
		Debug.Log("Bed RequestUnInteract");
		for (int i = 0; i < spotsToEnter.Count; i++)
		{
			if (spotsToEnter[i].CanUnInteract(thing))
			{
				Debug.Log("Bed succesfully freed the actor");
				spotsToEnter[i].UnInteract(this);
				return true;
			}
		}
		Debug.LogError("Bed unexpected end reached");
		return false;
	}


}