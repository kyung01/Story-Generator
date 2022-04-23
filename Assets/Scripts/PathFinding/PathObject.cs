using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine; 

public class PathObject
{
	public static bool WillPhysicallyBeInteractable(PathObject objA, PathObject objB)
	{
		return true;
	}

	static public bool WillPhysicallyBeInteractable(PathObject a, Vector2 aPosition, PathObject b, Vector2 bPosition)
	{
		var aOldPosition = a.PositionVec2Float;
		var bOldPosition = b.PositionVec2Float;

		a.SetPosition(aPosition);
		b.SetPosition(bPosition);

		var result = a.IsPhysicallyInteractable(b) && b.IsPhysicallyInteractable(a);

		a.SetPosition(aOldPosition);
		b.SetPosition(bOldPosition);

		return result;
	}

	public float x, y;
	public float exchangeRangePhysical = 0.5f;

	public int X { get { return Mathf.RoundToInt(X); } }
	public int Y { get { return Mathf.RoundToInt(y); } }

	public Vector2 PositionInt
	{
		get
		{
			return new Vector2(Mathf.Round(x), Mathf.Round(y));
		}
	}
	public Vector2 PositionVec2Float
	{
		get
		{
			return new Vector2(x,y);
		}
	}

	public void SetPosition(Vector2 aPosition)
	{
		this.x = aPosition.x;
		this.y = aPosition.y;
	}

	public bool IsPhysicallyInteractable(PathObject other)
	{
		if ((other.PositionInt - this.PositionInt).magnitude < this.exchangeRangePhysical)
		{
			return true;
		}
		return false;
	}

}
