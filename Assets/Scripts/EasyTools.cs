﻿using System.Collections;
using UnityEngine;

namespace EasyTools
{

	public class EzT
	{
		static public bool IsZero(float number)
		{
			return Mathf.Abs(number) < 0.001f;
		}
		public static Color GetRandomColorRaw()
		{
			int n = Random.Range(0, 3);
			float f1 = Random.Range(0, 1.0f);
			float f2 = 1 - f1;
			switch (n)
			{
				default:
				case 0:
					return new Color(1, f1, f2);
				case 1:
					return new Color(f1, 1, f2);
				case 2:
					return new Color(f1, f2, 1);
			}
		}

		public static Color GetRandomColor()
		{
			var c = GetRandomColorRaw();
			return new Color(c.r, c.g, c.b, 1);
		}

		public static void Separte(Vector2 vec1, Vector2 vec2, out Vector2 bottomLeft, out Vector2 topRight)
		{

			bottomLeft = new Vector2(Mathf.Min(vec1.x, vec2.x), Mathf.Min(vec1.y, vec2.y));
			topRight = new Vector2(Mathf.Max(vec1.x, vec2.x), Mathf.Max(vec1.y, vec2.y));
		}
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