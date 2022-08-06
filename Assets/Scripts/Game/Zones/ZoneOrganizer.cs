using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Threading.Tasks;
using StoryGenerator.World;

public static class StaticZoneOrganizer
{
	public static void Raise(this List<ZoneOrganizer.DEL_ZONE> list, Zone zone)
	{
		foreach (var l in list)
		{
			l(zone);
		}
	}
}

public class ZoneOrganizer
{
	public delegate void DEL_ZONE(Zone zone);
	
	//public delegate void DEL_ZONE_REMOVED(Zone zone);
	//public delegate void DEL_SINGLE_ZONE_SELECTED(Zone zone);
	public delegate void DEL_NO_ZONE_SELECTED();

	public List<DEL_ZONE> OnZoneAdded			= new List<DEL_ZONE>();
	public List<DEL_ZONE> OnZoneRemoved			= new List<DEL_ZONE>();
	public List<DEL_ZONE> OnZoneEdited			= new List<DEL_ZONE>();
	public List<DEL_ZONE> OnSingleZoneSelected	= new List<DEL_ZONE>();
	public List<DEL_NO_ZONE_SELECTED> OnNO_ZONE_SELECTED = new List<DEL_NO_ZONE_SELECTED>();

	public List<Zone> zones = new List<Zone>();

	public List<Zone> zonesSelected = new List<Zone>();

	public Dictionary<Vector2, List<Zone>> zoneMap = new Dictionary<Vector2, List<Zone>>();


	public ZoneOrganizer(World world)
	{
		world.OnThingMoved.Add(hdrWorldThingMoved);

	}

	private void hdrWorldThingMoved(World world, Thing thing, int xBefore, int yBefore, int xNew, int yNew)
	{
		if (!zoneMap.ContainsKey(new Vector2(xBefore, yBefore)))
		{
			zoneMap[new Vector2(xBefore, yBefore)] = new List<Zone>();
		}
		if (!zoneMap.ContainsKey(new Vector2(xNew, yNew)))
		{
			zoneMap[new Vector2(xNew, yNew)] = new List<Zone>();
		}
		var zonesBefore = zoneMap[new Vector2(xBefore, yBefore)];
		var zonesAfter = zoneMap[new Vector2(xNew, yNew)];

		for(int i = 0; i < zonesBefore.Count; i++)
		{
			zonesBefore[i].MovedOut(thing);
		}
		for(int i = 0; i < zonesAfter.Count; i++)
		{
			zonesBefore[i].MovedIn(thing);

		}
	}

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

	internal void raiseNoZoneSelected()
	{
		for (int i = 0; i < OnNO_ZONE_SELECTED.Count; i++)
		{
			OnNO_ZONE_SELECTED[i]();
		}
	}
	

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
		if (!zone.IsHavePositions) return;
		zones.Add(zone);
		foreach(var p in zone.positions)
		{
			if (!zoneMap.ContainsKey(p)){
				zoneMap.Add(p, new List<Zone>());
			}
			zoneMap[p].Add(zone);
		}
		OnZoneAdded.Raise(zone);

	}

	void zonePositionRemoved(Zone zone, int x, int y)
	{
		zone.positions.Remove(new Vector2(x, y));
		zoneMap[new Vector2(x, y)].Remove(zone);
	}

	public void DeleteZone(int xBegin, int yBegin, int xEnd, int yEnd)
	{
		for (int i = xBegin; i <= xEnd; i++)
		{
			for (int j = yBegin; j <= yEnd; j++)
			{
				for (int k = 0; k < zones.Count; k++)
				{
					if (zones[k].IsInZone(i, j))
					{
						zonePositionRemoved(zones[k], i, j);
					}

				}

			}
		}

		for (int k = 0; k < zones.Count; k++)
		{
			if (!zones[k].IsHavePositions)
			{
				zones.RemoveAt(k);
				OnZoneRemoved.Raise(zones[k]);
			}

		}
	}

	public void DeleteZone(int xBegin, int yBegin, int xEnd, int yEnd, Zone.TYPE zoneType)
	{
		List<Zone> zonesEdited = new List<Zone>();
		for (int i = xBegin; i <= xEnd; i++)
		{
			for (int j = yBegin; j <= yEnd; j++)
			{
				for (int k = 0; k < zones.Count; k++)
				{
					if (zones[k].type == zoneType)
					{
						if (zones[k].IsInZone(i, j))
						{
							zonePositionRemoved(zones[k], i, j);
							//zones[k].RemovePosition(new Vector2(i, j));
							zonesEdited.Add(zones[k]);
						}
					}


				}

			}
		}

		for (int k = zones.Count - 1; k >= 0; k--)
		{
			var zone = zones[k];
			if (zone.IsDead)
			{
				zones.RemoveAt(k);
				OnZoneRemoved.Raise(zone);
			}

		}

		for (int i = 0; i < zonesEdited.Count; i++)
		{
			if (zonesEdited[i].IsHavePositions)
			{
				OnZoneEdited.Raise(zonesEdited[i]);
				//raiseZoneEdited(zones[i]);
			}

		}
	}

	#region Add/Build Zones
	
	public void BuildStockpileZone (int xBegin, int yBegin, int xEnd, int yEnd){
		Zone spZone = new StockpileZone();
		addZone(spZone, xBegin,yBegin,xEnd,yEnd);
	}

	public void BuildHouseZone( int xBegin, int yBegin, int xEnd, int yEnd)
	{
		Zone houseZone = new BaseHousingZone();
		addZone(houseZone, xBegin, yBegin, xEnd, yEnd);
	}	

	public void BuildBedroom(int xBegin, int yBegin, int xEnd, int yEnd)
	{
		Zone bedroomZone = new BaseHousingZone();
		bedroomZone.type = Zone.TYPE.BEDROOM;
		BuildHouseRoom(bedroomZone, xBegin, yBegin, xEnd, yEnd);
	}

	public void BuildBathroom(int xBegin, int yBegin, int xEnd, int yEnd)
	{
		Zone bedroomZone = new BaseHousingZone();
		bedroomZone.type = Zone.TYPE.BATHROOM;
		BuildHouseRoom(bedroomZone, xBegin, yBegin, xEnd, yEnd);
	}
	public void BuildLivingroom(int xBegin, int yBegin, int xEnd, int yEnd)
	{
		Zone bedroomZone = new BaseHousingZone();
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
		BaseHousingZone houseBedroomBelongsTo = getHouseThisZoneIsIn(bedroomZone);
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
			houseBedroomBelongsTo.AddRoom(bedroomZone);
			//raiseZoneAdded(bedroomZone);
			OnZoneEdited.Raise(houseBedroomBelongsTo);
		}

	}

	#endregion

	private bool isOverlappingAny(Zone bedroomZone, Zone houseRoom)
	{
		throw new NotImplementedException();
	}

	private BaseHousingZone getHouseThisZoneIsIn(Zone bedroomZone)
	{
		foreach(var zone in this.zones)
		{
			if(zone is BaseHousingZone)
			{
				//we found a house zone
				if (zone.IsInZone(bedroomZone))
				{
					return (BaseHousingZone)zone;
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
			OnSingleZoneSelected.Raise(zonesWithin[0]);
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
			OnSingleZoneSelected.Raise(zonesWithin[0]);
		}
		else if (zonesWithin.Count == 0)
		{
			raiseNoZoneSelected();
		}
		zonesSelected = zonesWithin;
	}

	public void Update(World world, float timeElapsed)
	{
		for(int i = 0; i < zones.Count; i++)
		{
			this.zones[i].Update(world, timeElapsed);
		}
	}
}
