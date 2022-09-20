using StoryGenerator.World;
using StoryGenerator.World.Things.Actors;
using System.Collections.Generic;
using UnityEngine;

public interface IStorage
{
	public bool IsStorageAvailable(World world, ActorBase actor, Item itemToPutIn);

	public List<Vector2> GetStorageAccessPositions(World world, ActorBase actor);

	public bool UseStorage(World world, ActorBase actor);

	public bool PutItInTheStorage(Item item);
	public bool TakeItOutOfTheStorage(ActorBase actorRequesting, Item item);

	public List<Item> GetListOfAvailableItems();

}