using StoryGenerator.World;
using StoryGenerator.World.Things.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Storage : StructureWithInteractSpots, IStorage
{
	List<Item> items = new List<Item>();
	int maxItemCount = 10;


	public Storage(CAIModel model) : base(GameEnums.ThingCategory.STORAGE, model)
	{
	}

	public List<Item> GetListOfAvailableItems()
	{
		return new List<Item>(this.items);
	}

	public List<Vector2> GetStorageAccessPositions(World world, ActorBase actor)
	{
		return this.GetAvailableInteractionSpot(world, this.spotsToEnter.ToArray());
	}

	public bool IsStorageAvailable(World world, ActorBase actor, Item itemToPutIn)
	{
		foreach(var s in spotsToEnter)
		{
			if (s.IsAvailableForConsideration(world, this)) return true;
		}
		return false;
	}

	public bool PutItInTheStorage(Item item)
	{
		if (items.Count >= maxItemCount) return false;

		items.Add(item);
		item.SetInteractor(this, GameEnums.InteractorType.STORAGE);
		item.SetPosition(this.XY);
		return true;
	}

	public bool TakeItOutOfTheStorage(ActorBase actorRequesting, Item item)
	{
		if (!items.Contains(item)) throw new NotImplementedException();
		items.Remove(item);
		actorRequesting.Carry(item);

		return true;
	}

	public bool UseStorage(World world, ActorBase actor)
	{
		foreach(var spot in this.spotsToEnter)
		{
			int x, y;
			if(spot.CanInteractWithIt(world, this, actor))
			{
				spot.Interact(this, actor);
				//actor.SetInteractor(this, GameEnums.InteractorType.STORAGE);
				return true;
			}
		}
		return false;
	}
}
