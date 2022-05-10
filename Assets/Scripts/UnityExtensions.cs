using UnityEngine;

public static class UnityExtensions
{
	static public bool IsSame_INT(this Vector2 me, Vector2 other)
	{
		var meX = Mathf.RoundToInt(me.x);
		var meY = Mathf.RoundToInt(me.y);
		var otherX = Mathf.RoundToInt(other.x);
		var otherY = Mathf.RoundToInt(other.y);

		return meX == otherX && meY == otherY;
	}
}