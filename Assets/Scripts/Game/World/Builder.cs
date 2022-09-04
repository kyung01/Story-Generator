using StoryGenerator.World;
using GameEnums;
using UnityEngine;

public class Builder
{


	static Thing categoryToActualThing(CATEGORY type)
	{
		switch (type)
		{
			case CATEGORY.WALL:
				return ThingSheet.GetWall();
			case CATEGORY.DOOR:
				return new Door();
			case CATEGORY.ROOF:
				return ThingSheet.GetRoof();
			case CATEGORY.BED:
				return ThingSheet.GetBed();
		}
		return new Thing(CATEGORY.UNDEFINED);
	}

	static public void Build(World world, CATEGORY thingToBuild, int x, int y, Direction dirToBuild)
	{
		Thing thing = categoryToActualThing(thingToBuild);
		if (thingToBuild != CATEGORY.ROOF)
		{
			world.EmptySpot(x, y);
		}

		if ((thingToBuild == CATEGORY.ROOF) ? world.IsRoofAt(x, y) : world.IsFrameAt(x, y) )
		{
			//Not buildable
			return;
		}
		thing.SetPosition(x, y);
		if (thing is ThingWithPhysicalPresence)
		{
			((ThingWithPhysicalPresence)thing).Face(world, dirToBuild);
			Debug.Log(dirToBuild);
		}


		world.AddThingAndInit(thing);
		if (thing is Frame)
		{

			((Frame)thing).Install();

		}
		

	}

}