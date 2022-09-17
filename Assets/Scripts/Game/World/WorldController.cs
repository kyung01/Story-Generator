using UnityEngine;
using System.Collections.Generic;
using StoryGenerator.World;
using GameEnums;

public partial class WorldController
{
	public enum Command
	{
		NONE,
		BUILD,
		HAUL,
		//STOCKPILE,
		ZONE,


		END,
	}	

	public static WorldController INSTANCE;

	private Direction directionToBuild = Direction.UP;
	private ZoneCategory zoneToBuild;

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

	internal static void CancellCurrentAction()
	{
		INSTANCE.command = Command.NONE;
	}

	public static void Select(Vector2 from, Vector2 to)
	{
		switch (INSTANCE.command)
		{
			case Command.NONE:
				Selector.SelectFromTo(World, from, to);
				break;
			case Command.BUILD:
				for (int i = (int)from.x; i <= to.x; i++)
				{
					for (int j = (int)from.y; j <= to.y; j++)
					{
						Builder.Build(World, INSTANCE.thingToBuild, i, j, INSTANCE.directionToBuild);
					}
				}
				break;
			case Command.HAUL:
				Debug.Log("WorldController::Issuing a command Haul" + from + " " + to);
				Selector.SelectFromTo(World, from, to);
				INSTANCE.apply();
				break;
			case Command.ZONE:
				switch (INSTANCE.zoneToBuild)
				{
					case ZoneCategory.STOCKPILE:
						World.zoneOrganizer.BuildStockpileZone((int)from.x, (int)from.y, (int)to.x, (int)to.y);
						break;
					case ZoneCategory.HOUSING:
						World.zoneOrganizer.BuildHouseZone((int)from.x, (int)from.y, (int)to.x, (int)to.y);
						break;
					case ZoneCategory.HOUSING_BEDROOM:
						World.zoneOrganizer.BuildBedroom((int)from.x, (int)from.y, (int)to.x, (int)to.y);
						break;
					case ZoneCategory.HOUSING_BATHROOM:
						World.zoneOrganizer.BuildBathroom((int)from.x, (int)from.y, (int)to.x, (int)to.y);
						break;
					case ZoneCategory.HOUSING_LIVINGROOM:
						World.zoneOrganizer.BuildLivingroom((int)from.x, (int)from.y, (int)to.x, (int)to.y);
						break;
					case ZoneCategory.NONE:
						break;
					default:
						break;
				}
				INSTANCE.command = Command.NONE;
				break;
			case Command.END:
				break;
			default:
				break;
		}

	}

	public static void UnSelect()
	{
		Selector.Select(World, 0, 0, -1, -1);
	}
	public static void SetCommand(Command command, ThingCategory thingToBuild = ThingCategory.UNDEFINED, ZoneCategory zoneToBuild =  ZoneCategory.NONE)
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
		if(command == Command.ZONE)
		{
			INSTANCE.zoneToBuild = zoneToBuild;

		}
	}

	internal static void SetBuildingDirection(Direction directionToBuild)
	{
		INSTANCE.directionToBuild = directionToBuild;
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
	ThingCategory thingToBuild = ThingCategory.UNDEFINED;


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
					world.PlayerTeam.WorkManager.Howl((Thing_Interactable) s);
				}
				break;
			default:
				break;
		}
	}

}