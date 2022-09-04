using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class UIEnums
{
	public enum FEEDBACK
	{
		NONE=0,

		BUILDS,
		TASKS,
		TASKS_HAUL,

		BUILD_WALL,
		BUILD_DOOR,
		BUILD_ROOF,
		BUILD_BED,
		BUILD_CHAIR,
		BUILD_TABLE,



		ZONES,
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
	public delegate void DEL_FEEDBACK(FEEDBACK value);
	public static void Raise(this List<DEL_FEEDBACK> me, FEEDBACK value)
	{
		foreach(var d in me)
		{
			d(value);
		}
	}

	public static bool IsZONES(this FEEDBACK value)
	{
		switch (value)
		{
			case FEEDBACK.ZONES:
			case FEEDBACK.ZONES_HOUSING:
			case FEEDBACK.ZONES_HOUSING_BATHROOM:
			case FEEDBACK.ZONES_HOUSING_BEDROOM:
			case FEEDBACK.ZONES_HOUSING_HOUSE:
			case FEEDBACK.ZONES_HOUSING_LIVINGROOM:
			case FEEDBACK.ZONES_STOCKPILE:
				return true;
		}
		return false;
	}

	internal static FEEDBACK ToEnum(string feedback)
	{
		Dictionary<string, FEEDBACK> dic = new Dictionary<string, FEEDBACK>() {
			{"BUILD_WALL",FEEDBACK.BUILD_WALL  },
			{"BUILD_DOOR",FEEDBACK.BUILD_DOOR  },
			{"BUILD_ROOF",FEEDBACK.BUILD_ROOF  },
			{"BUILD_BED",FEEDBACK.BUILD_BED  }
		};
		if(dic.ContainsKey(feedback))
		{
			return dic[feedback];
		}
		return FEEDBACK.NONE;
	}

	public static bool isBUILDS(this FEEDBACK value)
	{
		switch (value)
		{
			case FEEDBACK.BUILDS:
			case FEEDBACK.BUILD_ROOF:
			case FEEDBACK.BUILD_DOOR:
			case FEEDBACK.BUILD_WALL:
			case FEEDBACK.BUILD_BED:
			case FEEDBACK.BUILD_CHAIR:
			case FEEDBACK.BUILD_TABLE:
				return true;
		}
		return false;
	}
	
	public static bool isTASK(this FEEDBACK value)
	{
		switch (value)
		{
			case FEEDBACK.TASKS:
			case FEEDBACK.TASKS_HAUL:
				return true;
		}
		return false;
	}
}