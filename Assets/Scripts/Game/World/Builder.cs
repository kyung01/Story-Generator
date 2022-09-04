using StoryGenerator.World;
using UnityEngine;

public class Builder
{
	static Frame GetFrame(Game.CATEGORY type)
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

	static Frame hprGetStructure(Game.CATEGORY type)
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
		return new Frame(Game.CATEGORY.UNDEFINED);
	}

	static public void Build(World world, Game.CATEGORY thingToBuild, int x, int y)
	{
		Frame structure = hprGetStructure(thingToBuild);
		if (thingToBuild != Game.CATEGORY.ROOF)
		{
			world.EmptySpot(x, y);
		}

		if ((thingToBuild == Game.CATEGORY.ROOF) ? world.IsRoofAt(x, y) : world.IsStructureAt(x, y) )
		{
			//Not buildable
			return;
		}
		structure.SetPosition(x, y);
		world.AddThingAndInit(structure);
		structure.Install();

	}
}