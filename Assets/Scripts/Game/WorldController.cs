using UnityEngine;
using System.Collections.Generic;
using StoryGenerator.World;
using System;

public partial class WorldController
{

	public enum Command
	{
		NONE,
		BUILD,
		HAUL,
		STOCKPILE,


		END,
	}
	

	public static WorldController INSTANCE;

	private static World World { get { return INSTANCE.world; } }
	private static WorldThingSelector Selector { get { return INSTANCE.worldThingSelector; } }

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
		if(INSTANCE.command == Command.HAUL)
		{
			Debug.Log("WorldController::Issuing a command Haul" + from + " " + to);
			Selector.SelectFromTo(World, from, to);
			//Selector.Select(World, Mathf.RoundToInt(from.x), Mathf.RoundToInt(from.y), Mathf.RoundToInt(to.x), Mathf.RoundToInt(to.y));
			INSTANCE.apply();
			INSTANCE.command = Command.NONE;
		}
		else if(INSTANCE.command == Command.BUILD)
		{
			for(int i = (int)from.x; i <= to.x; i++)
			{
				for(int j = (int)from.y; j <= to.y; j++)
				{
					World.Build(INSTANCE.thingToBuild, i, j);
				}
			}
			INSTANCE.command = Command.NONE;
			INSTANCE.thingToBuild = Thing.TYPE.UNDEFINED;
			
		}
		else if(INSTANCE.command == Command.STOCKPILE)
		{
			Debug.Log("WorldController::Building a stockpile zone " + from + " " + to );
			World.zoneOrganizer.BuildStockpileZone((int)from.x, (int)from.y, (int)to.x, (int)to.y);
			INSTANCE.command = Command.NONE;

		}
		else
		{
			Selector.SelectFromTo(World, from, to);


		}

	}

	public static void SetCommand(Command command, Thing.TYPE thingToBuild = Thing.TYPE.UNDEFINED)
	{
		INSTANCE.command = command;

		if(command == Command.HAUL && Selector.ThingsCurrentlySelected.Count != 0)
		{
			INSTANCE.apply();
			INSTANCE.command = Command.NONE;
		}
		if(command == Command.BUILD)
		{
			INSTANCE.thingToBuild = thingToBuild;
		}
	}


	public static List<Thing> GetCurrentlySelectedThings()
	{
		var selector = INSTANCE.worldThingSelector;

		return selector.ThingsCurrentlySelected;
	}



}

public partial class WorldController
{

	World world;
	WorldThingSelector worldThingSelector = new WorldThingSelector();
	Command command;
	Thing.TYPE thingToBuild = Thing.TYPE.UNDEFINED;


	private WorldController(World world)
	{

		this.world = world;
		WorldController.INSTANCE = this;

	}
	public void apply()
	{
		var selected = worldThingSelector.ThingsCurrentlySelected;
		switch (command)
		{
			case Command.HAUL:
				foreach(var s in selected)
				{
					world.PlayerTeam.WorkManager.Howl(s);
				}
				break;
			default:
				break;
		}
	}

}