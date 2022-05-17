using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class UIEnums
{
	public delegate void DEL_FEEDBACK(FEEDBACK value);
	public static void Raise(this List<DEL_FEEDBACK> me, FEEDBACK value)
	{
		foreach(var d in me)
		{
			d(value);
		}
	}
	public enum FEEDBACK
	{
		NONE,

		BUILDS,
		TASKS,
		ZONES,

		BUILDS_WALL,
		BUILDS_DOOR,
		BUILDS_CEILING,

		TASK_HAUL,

		ZONES_STOCKPILE,
		ZONES_HOUSING,

		ZONES_HOUSING_HOUSE,
		ZONES_HOUSING_BEDROOM,
		ZONES_HOUSING_BATHROOM,
		ZONES_HOUSING_LIVINGROOM,

		ANY,
		ADD,
		REMOVE,

		END
	}
}