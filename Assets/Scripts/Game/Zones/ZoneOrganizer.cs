using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Threading.Tasks;

public class ZoneOrganizer
{
	public delegate void DEL_ZONE_REMOVED(Zone zone);
	public List<DEL_ZONE_REMOVED> OnZoneRemoved = new List<DEL_ZONE_REMOVED>();
	void raiseZoneRemoved(Zone zone)
	{
		for(int i = 0; i < OnZoneRemoved.Count; i++)
		{
			OnZoneRemoved[i](zone);
		}
	}
	public List<Zone> zones = new List<Zone>();
	public List<Zone> zonesSelected = new List<Zone>();

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

	internal void DeleteZone(int xBegin, int yBegin, int xEnd, int yEnd)
	{
		for (int i = xBegin; i <= xEnd; i++)
		{
			for (int j = yBegin; j <= yEnd; j++)
			{
				for (int k = 0; k < zones.Count;k++)
				{
					if (zones[k].IsInZone(i, j))
					{
						zones[k].positions.Remove(new Vector2(i, j));
					}

				}

			}
		}

		for (int k = 0; k < zones.Count; k++)
		{
			if (zones[k].IsEmpty)
			{
				raiseZoneRemoved(zones[k]);
				zones.RemoveAt(k);
			}

		}
	}

	internal void Select(int xBegin, int yBegin, int xEnd, int yEnd)
	{
		List<Zone> zonesWithin = new List<Zone>();
		for(int x = xBegin; x <= xEnd; x++)
		{
			for(int y = yBegin; y <= yEnd; y++)
			{
				for(int i = 0; i < zones.Count; i++)
				{
					if(zones[i].IsInZone(x, y))
					{
						zonesWithin.Add(this.zones[i]);
					}
				}
			}
		}
		zonesSelected = zonesWithin;
	}
}
