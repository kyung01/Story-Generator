using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Threading.Tasks;

public class ZoneOrganizer
{
	public delegate void DEL_ZONE_REMOVED(Zone zone);
	public delegate void DEL_SINGLE_ZONE_SELECTED(Zone zone);
	public delegate void DEL_NO_ZONE_SELECTED();

	public List<DEL_ZONE_REMOVED> OnZoneRemoved = new List<DEL_ZONE_REMOVED>();
	public List<DEL_SINGLE_ZONE_SELECTED> OnSingleZoneSelected = new List<DEL_SINGLE_ZONE_SELECTED>();
	public List<DEL_NO_ZONE_SELECTED> OnNO_ZONE_SELECTED = new List<DEL_NO_ZONE_SELECTED>();

	void raiseSingleZoneSelected(Zone zone)
	{
		for (int i = 0; i < OnSingleZoneSelected.Count; i++)
		{
			OnSingleZoneSelected[i](zone);
		}
	}
	void raiseNoZoneSelected()
	{
		for (int i = 0; i < OnNO_ZONE_SELECTED.Count; i++)
		{
			OnNO_ZONE_SELECTED[i]();
		}
	}
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
		Zone zoneSelected = new StockpileZone();
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
		if (zonesWithin.Count == 1)
		{
			raiseSingleZoneSelected(zonesWithin[0]);
		}
		else if (zonesWithin.Count == 0)
		{
			raiseNoZoneSelected();
		}
		zonesSelected = zonesWithin;
	}
}
