using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class ThingTypeStatic
{
	public static bool IsPlant(this Thing.TYPE type)
	{
		if (type == Thing.TYPE.GRASS || type == Thing.TYPE.BUSH || type == Thing.TYPE.REED)
			return true;
		return false;
	}
	
}
public partial class Thing
{
	public enum TYPE
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
		ITEM
	}
}