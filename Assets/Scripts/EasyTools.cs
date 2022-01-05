using System.Collections;
using UnityEngine;

namespace EasyTools
{

	public class EzT
	{

		public static bool ChkWithinBoundaries(int value, int min, int max)
		{
			return (value >= min) && (value <= max);
		}

		public static bool ChkWithinBoundaries(float value, float min, float max)
		{
			return (value >= min) && (value <= max);
		}

		public static bool ChkWithinBoundaries(Vector2 value, Vector2 min, Vector2 max)
		{
			return ChkWithinBoundaries(value.x, min.x, max.x) && ChkWithinBoundaries(value.y, min.y, max.y);
		}

	}

}