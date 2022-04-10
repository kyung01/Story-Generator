using System;
using System.Collections.Generic;
using UnityEngine;

public class Zone
{
	public enum TYPE { DEFAULT,STOCKPILE}
	internal TYPE type = TYPE.DEFAULT;
	public List<Vector2> positions = new List<Vector2>();

	public bool IsNotAlive
	{
		get { return positions.Count == 0; }
	}
	public bool IsInZone(int x, int y)
	{
		foreach (var p in positions)
		{
			if ((int)p.x == x && (int)p.y == y) return true;
		}
		return false;
	}
	public bool ExpandZone(int x, int y)
	{
		if (IsInZone(x, y)) return false;
		positions.Add(new Vector2(x, y));
		return true;
	}
}