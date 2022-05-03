using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ThingSheet
{
	public static Thing GetGrass()
	{
		Plant plant = new Plant();
		plant.type = Thing.TYPE.GRASS;
		plant.resources.Add(Game.Keyword.FOOD_VEGI, 100);
		return plant;
	}
	public static Thing GetBush()
	{
		Plant plant = new Plant();
		plant.type = Thing.TYPE.BUSH;
		//plant.resources.Add(Game.Keyword.FOOD_VEGI, 100);
		return plant;
	}
	public static Thing GetRock()
	{
		Thing thing = new Thing();
		thing.type = Thing.TYPE.ROCK;
		return thing;
	}
	public static Thing GetReed()
	{
		Thing thing = new Thing();
		thing.type = Thing.TYPE.REED;
		return thing;
	}

	public static Structure GetRoof()
	{
		Structure roof = new Structure();
		roof.type = Thing.TYPE.ROOF;
		return roof;
	}
	public static Structure GetWall()
	{
		Structure roof = new Structure();
		roof.type = Thing.TYPE.WALL;
		return roof;
	}
	public static Thing Bear()
	{
		return null;
	}
}
