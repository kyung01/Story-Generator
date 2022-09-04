using StoryGenerator.World;
using UnityEngine;

public class Builder
{
	static Frame GetFrsame(Game.CATEGORY type)
	{
		switch (type)
		{
			case Game.CATEGORY.WALL:
				return ThingSheet.GetWall();
			case Game.CATEGORY.DOOR:
				return new Door();
			case Game.CATEGORY.ROOF:
				return ThingSheet.GetRoof();
		}
		Debug.LogError("Builder : cannot find " + type + " of frame to build");
		return new Frame(Game.CATEGORY.UNDEFINED);
	}

	static Thing categoryToActualThing(Game.CATEGORY type)
	{
		switch (type)
		{
			case Game.CATEGORY.WALL:
				return ThingSheet.GetWall();
			case Game.CATEGORY.DOOR:
				return new Door();
			case Game.CATEGORY.ROOF:
				return ThingSheet.GetRoof();
			case Game.CATEGORY.BED:
				return ThingSheet.GetBed();
		}
		return new Thing(Game.CATEGORY.UNDEFINED);
	}

	static public void Build(World world, Game.CATEGORY thingToBuild, int x, int y)
	{
		Thing thing = categoryToActualThing(thingToBuild);
		if (thingToBuild != Game.CATEGORY.ROOF)
		{
			world.EmptySpot(x, y);
		}

		if ((thingToBuild == Game.CATEGORY.ROOF) ? world.IsRoofAt(x, y) : world.IsStructureAt(x, y) )
		{
			//Not buildable
			return;
		}
		thing.SetPosition(x, y);
		
		world.AddThingAndInit(thing);
		if (thing is Frame)
		{

			((Frame)thing).Install();

		}

	}
}