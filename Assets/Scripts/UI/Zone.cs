using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Text;
using System.Threading.Tasks;

public class Zone
{
	public List<Vector2> positions = new List<Vector2>();
	public bool CanYouAddTo(int x, int y)
	{
		return !IsAlreadyInZone(x, y);
	}
	public bool IsAlreadyInZone(int x, int y)
	{
		foreach(var p in positions)
		{
			if ((int)p.x == x && (int)p.y == y) return true;
		}
		return false;
	}
	public bool addZone(int x, int y)
	{
		if (IsAlreadyInZone(x, y)) return false;
		positions.Add(new Vector2(x, y));
		return true;
	}
}