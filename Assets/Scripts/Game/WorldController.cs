using UnityEngine;
using System.Collections.Generic;
using StoryGenerator.World;
public partial class WorldController
{
	public static WorldController INSTANCE;

	private static World World { get { return INSTANCE.world; } }
	private static WorldThingSelector Selector { get { return INSTANCE.wrdThingSelector; } }

	public static void Init(World world)
	{
		if(INSTANCE != null)
		{
			Debug.LogError("WorldController Errror :: " + "has INSTANCE already been initialized::Is trying to call it again ");
			return;
		}
		INSTANCE = new WorldController(world);
	}
	public static void Select(Vector2 from, Vector2 to)
	{
		Selector.Select(World, Mathf.RoundToInt(from.x), Mathf.RoundToInt(from.y), Mathf.RoundToInt(to.x), Mathf.RoundToInt(to.y));
	}

	public static List<Thing> GetCurrentlySelectedThings()
	{
		var selector = INSTANCE.wrdThingSelector;

		return selector.ThingsCurrentlySelected;
	}



}

public partial class WorldController 
{
	World world;
	WorldThingSelector wrdThingSelector = new WorldThingSelector();

	private WorldController(World world)
	{

		WorldController.INSTANCE = this;

	}
	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}
}