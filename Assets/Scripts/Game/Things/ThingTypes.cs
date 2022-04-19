using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class Thing
{
	public enum TYPE
	{
		UNDEFINED, 
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
		CEILING,
		FLOOR,

		END,
		STRUCTURE
	}
}