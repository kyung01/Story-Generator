using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class ThingTypeStatic
{
	public static bool IsPlant(this Thing.CATEGORY type)
	{
		if (type == Thing.CATEGORY.GRASS || type == Thing.CATEGORY.BUSH || type == Thing.CATEGORY.REED)
			return true;
		return false;
	}
	
}
public partial class Thing
{
	public enum CATEGORY
	{
		UNDEFINED=0, 
		

		//Animals
		RABBIT,
		BEAR,
		HUMAN,
		//Enviornment
		ROCK, 
		GRASS, 
		BUSH,
		REED,
		//Buildings
		DOOR,
		WALL,
		FLOOR,
		ROOF,

		END,
		STRUCTURE,
		FRAME,
		ITEM,
		BED
	}
}