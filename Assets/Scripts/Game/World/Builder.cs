using StoryGenerator.World;
using UnityEngine;

public class Builder
{
	static Frame GetFrame(Thing.CATEGORY type)
	{
		switch (type)
		{
			case Thing.CATEGORY.WALL:
				return ThingSheet.GetWall();
			case Thing.CATEGORY.DOOR:
				return new Door();
			case Thing.CATEGORY.ROOF:
				return ThingSheet.GetRoof();
		}
		Debug.LogError("Builder : cannot find " + type + " of frame to build");
		return new Frame(Thing.CATEGORY.UNDEFINED);
	}
	static Frame hprGetStructure(Thing.CATEGORY type)
	{
		switch (type)
		{
			case Thing.CATEGORY.WALL:
				return ThingSheet.GetWall();
			case Thing.CATEGORY.DOOR:
				return new Door();
			case Thing.CATEGORY.ROOF:
				return ThingSheet.GetRoof();
		}
		return new Frame(Thing.CATEGORY.UNDEFINED);
	}

	static public void Build(World world, Thing.CATEGORY thingToBuild, int x, int y)
	{

		Frame structure = hprGetStructure(thingToBuild);
		if (thingToBuild != Thing.CATEGORY.ROOF)
		{
			world.EmptySpot(x, y);
		}

		if ((thingToBuild == Thing.CATEGORY.ROOF) ? world.IsRoofAt(x, y) : world.IsStructureAt(x, y) )
		{
			//Not buildable
			return;
		}
		structure.SetPosition(x, y);
		world.AddThingAndInit(structure);
		structure.Install();

	}
}