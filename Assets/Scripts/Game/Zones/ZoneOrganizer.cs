using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Threading.Tasks;

public class ZoneOrganizer
{
	public List<Zone> zones = new List<Zone>();

	public void BuildZone (int xBeing, int yBeing, int xEnd, int yEnd){
		Zone zoneSelected = new Zone();
		for (int i = xBeing; i <= xEnd; i++)
		{
			for (int j = yBeing; j <= yEnd; j++)
			{
				if (isInAnotherZone(i, j)) continue;
				zoneSelected.positions.Add(new Vector2(i, j));

			}
		}
		zones.Add(zoneSelected);
	}
	bool isInAnotherZone(int x, int y)
	{
		for (int i = 0; i < zones.Count; i++)
		{
			if (zones[i].IsInZone(x, y)) return true;

		}
		return false;
	}

}
