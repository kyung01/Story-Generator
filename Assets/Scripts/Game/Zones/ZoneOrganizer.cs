using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Threading.Tasks;

public class ZoneOrganizer
{
	public delegate void DEL_ZONE_ADDED(Zone zone);
	public delegate void DEL_ZONE_REMOVED(Zone zone);
	public delegate void DEL_SINGLE_ZONE_SELECTED(Zone zone);
	public delegate void DEL_NO_ZONE_SELECTED();

	public List<DEL_ZONE_ADDED> OnZoneAdded = new List<DEL_ZONE_ADDED>();
	public List<DEL_ZONE_REMOVED> OnZoneRemoved = new List<DEL_ZONE_REMOVED>();
	public List<DEL_SINGLE_ZONE_SELECTED> OnSingleZoneSelected = new List<DEL_SINGLE_ZONE_SELECTED>();
	public List<DEL_NO_ZONE_SELECTED> OnNO_ZONE_SELECTED = new List<DEL_NO_ZONE_SELECTED>();

	public List<StockpileZone> GetStockpiles()
	{
		List<StockpileZone> stockpileZones = new List<StockpileZone>();
		foreach(Zone zone in zones)
		{
			if(zone is StockpileZone)
			{
				stockpileZones.Add((StockpileZone) zone);
			}
		}
		return stockpileZones;
	}

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
	void raiseZoneAdded(Zone zone)
	{
		for (int i = 0; i < OnZoneAdded.Count; i++)
		{
			OnZoneAdded[i](zone);
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

	void addZone(Zone zone, int xBegin, int yBegin, int xEnd, int yEnd)
	{
		for (int i = xBegin; i <= xEnd; i++)
		{
			for (int j = yBegin; j <= yEnd; j++)
			{
				if (isInAnotherZone(i, j)) continue;
				zone.AddPosition(new Vector2(i, j));

			}
		}
		zone.RefreshPositions();
		zones.Add(zone);
		raiseZoneAdded(zone);
	}

	public void BuildHouseZone(int xBegin, int yBegin, int xEnd, int yEnd)
	{
		Zone houseZone = new HouseZone();
		addZone(houseZone, xBegin, yBegin, xEnd, yEnd);
	}	

	public void BuildStockpileZone (int xBegin, int yBegin, int xEnd, int yEnd){
		Zone spZone = new StockpileZone();
		addZone(spZone, xBegin,yBegin,xEnd,yEnd);
	}

	public void BuildBedroom(int xBegin, int yBegin, int xEnd, int yEnd)
	{
		Zone bedroomZone = new RoomZone();
		bedroomZone.type = Zone.TYPE.BEDROOM;
		BuildHouseRoom(bedroomZone, xBegin, yBegin, xEnd, yEnd);
	}
	public void BuildBathroom(int xBegin, int yBegin, int xEnd, int yEnd)
	{
		Zone bedroomZone = new RoomZone();
		bedroomZone.type = Zone.TYPE.BATHROOM;
		BuildHouseRoom(bedroomZone, xBegin, yBegin, xEnd, yEnd);
	}
	public void BuildLivingroom(int xBegin, int yBegin, int xEnd, int yEnd)
	{
		Zone bedroomZone = new RoomZone();
		bedroomZone.type = Zone.TYPE.LIVINGROOM;
		BuildHouseRoom(bedroomZone, xBegin, yBegin, xEnd, yEnd);
	}


	void BuildHouseRoom(Zone bedroomZone, int xBegin, int yBegin, int xEnd, int yEnd)
	{
		for (int i = xBegin; i <= xEnd; i++)
		{
			for (int j = yBegin; j <= yEnd; j++)
			{
				bedroomZone.AddPosition(new Vector2(i, j));

			}

		}
		//a bedroom zone must be in one of the house zones
		HouseZone houseBedroomBelongsTo = getHouseThisZoneIsIn(bedroomZone);
		if (houseBedroomBelongsTo == null)
		{
			//couldn't find any house that this zone belogns to
			return;
		}

		bedroomZone.positions = new List<Vector2>();

		for (int i = xBegin; i <= xEnd; i++)
		{
			for (int j = yBegin; j <= yEnd; j++)
			{
				if (isInAnotherZone(i, j, houseBedroomBelongsTo.Rooms)) continue;
				bedroomZone.AddPosition(new Vector2(i, j));

			}

		}
		bedroomZone.RefreshPositions();

		//a bedroom(inside a house) cannot overlap onto another room in the house
		var rooms = houseBedroomBelongsTo.Rooms;
		bool isOverlapping = false;
		foreach(var houseRoom in rooms)
		{
			if(bedroomZone.IsOverlapping( houseRoom))
			{
				//room is overlapping
				isOverlapping = true;
				break;
			}
		}
		if (isOverlapping)
		{
			//cannot add the bedroom to the house
		}
		else
		{
			houseBedroomBelongsTo.Add(bedroomZone);
			raiseZoneAdded(bedroomZone);
		}

	}

	private bool isOverlappingAny(Zone bedroomZone, Zone houseRoom)
	{
		throw new NotImplementedException();
	}

	private HouseZone getHouseThisZoneIsIn(Zone bedroomZone)
	{
		foreach(var zone in this.zones)
		{
			if(zone is HouseZone)
			{
				//we found a house zone
				if (zone.IsInZone(bedroomZone))
				{
					return (HouseZone)zone;
				}
			}
		}
		return null;
	}

	bool isInAnotherZone(List<Zone> zones, int x, int y)
	{
		for (int i = 0; i < zones.Count; i++)
		{
			if (zones[i].IsInZone(x, y)) return true;

		}
		return false;
	}

	bool isInAnotherZone(int x, int y)
	{
		return isInAnotherZone(x, y, this.zones);
	}
	bool isInAnotherZone(int x, int y,List<Zone> zones)
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
			if (zones[k].IsNotAlive)
			{
				raiseZoneRemoved(zones[k]);
				zones.RemoveAt(k);
			}

		}
	}

	public List<Zone> GetZonesAt(int xBegin, int yBegin, int xEnd, int yEnd)
	{
		List<Zone> zonesWithin = new List<Zone>();
		for (int x = xBegin; x <= xEnd; x++)
		{
			for (int y = yBegin; y <= yEnd; y++)
			{
				for (int i = 0; i < zones.Count; i++)
				{
					if (zones[i].IsInZone(x, y))
					{
						zonesWithin.Add(this.zones[i]);
					}
				}
			}
		}
		return zonesWithin;
	}

	bool hprIsZoneThisType(Zone.TYPE type, Zone.TYPE[] typeArray)
	{
		foreach(var t in typeArray)
		{
			if (type == t) return true;
		}
		return false;
	}
	internal void Select(int xBegin, int yBegin, int xEnd, int yEnd, params Zone.TYPE[] selectedZoneTypes)
	{
		List<Zone> zonesWithin = new List<Zone>();
		for (int i = 0; i < zones.Count; i++)
		{
			if (!hprIsZoneThisType(zones[i].type, selectedZoneTypes))
			{
				continue;
			}
			bool isZoneAdded = false;
			for (int x = xBegin; x <= xEnd && !isZoneAdded; x++)
			{
				for (int y = yBegin; y <= yEnd && !isZoneAdded; y++)
				{
					if (zones[i].IsInZone(x, y))
					{
						zonesWithin.Add(this.zones[i]);
						isZoneAdded = true;
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
	internal void Select(int xBegin, int yBegin, int xEnd, int yEnd)
	{
		var array = Enum.GetValues(typeof(Zone.TYPE)).Cast<Zone.TYPE>().ToArray();
		Select(xBegin, yBegin, xEnd, yEnd, array);
		return;
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
