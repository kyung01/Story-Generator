using StoryGenerator.World;
using GameEnums;
using UnityEngine;

public class Builder
{


	static Thing categoryToActualThing(ThingCategory type)
	{
		switch (type)
		{
			case ThingCategory.WALL:
				return ThingSheet.GetWall();
			case ThingCategory.DOOR:
				return new Door();
			case ThingCategory.ROOF:
				return ThingSheet.GetRoof();
			case ThingCategory.BED:
				return ThingSheet.GetBed();
		}
		return new Thing(ThingCategory.UNDEFINED);
	}

	static bool prepareForConstruction(World world, Structure structure, int x, int y, Direction dir)
	{
		structure.XY = new Vector2(x, y);
		structure.SetFacingDirection(dir);
		var collisionSpots = structure.CAIModel.GetCollisionMap(structure);
		foreach (var c in collisionSpots)
		{
			if (!world.IsWalkableAt((int)c.x, (int)c.y)) return false;
		}
		foreach (var c in collisionSpots)
		{
			world.EmptySpotAndOccupy((int)c.x, (int)c.y);
		}
		return true;
	}
	static public void Build(World world, ThingCategory categoryOfThingToBuild, int x, int y, Direction dirToBuild)
	{
		Debug.Log("Building direction " + dirToBuild);
		Thing thing = categoryToActualThing(categoryOfThingToBuild);
		if(categoryOfThingToBuild == ThingCategory.ROOF)
		{
			if (world.IsRoofAt(x, y)) return;

		}
		else if(thing is Structure)
		{
			if (!prepareForConstruction(world, (Structure)thing, x, y, dirToBuild)) return;			

		}
		else if(thing is Frame)
		{
			if(world.IsFrameAt(thing.X_INT,thing.Y_INT)) return;
			else
			{
				world.EmptySpot(x, y);
			}
		}

		thing.SetPosition(x, y);
		world.AddThingAndInit(thing);

		

	}

}