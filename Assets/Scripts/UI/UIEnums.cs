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

	public static bool isZONES(this FEEDBACK value)
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
	public static bool isBUILDS(this FEEDBACK value)
	{
		switch (value)
		{
			case FEEDBACK.BUILDS:
			case FEEDBACK.BUILDS_ROOF:
			case FEEDBACK.BUILDS_DOOR:
			case FEEDBACK.BUILDS_WALL:
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
	public enum FEEDBACK
	{
		NONE=0,

		BUILDS,
		TASKS,
		TASKS_HAUL,

		BUILDS_WALL,
		BUILDS_DOOR,
		BUILDS_ROOF,


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
}