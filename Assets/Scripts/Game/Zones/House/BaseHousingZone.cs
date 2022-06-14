using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ResidentDescription { }

public class BaseHousingZone : Zone
{
	public Dictionary<Thing, ResidentDescription> residents = new Dictionary<Thing, ResidentDescription>();
	List<Zone> otherZones = new List<Zone>();

	public BaseHousingZone()
	{
		this.type = TYPE.HOUSE;
	}

	public List<Zone> Rooms
	{
		get
		{
			List<Zone> rooms = new List<Zone>();
			foreach (var r in otherZones)
			{
				rooms.Add(r);
			}
			return rooms;
		}
	}

	public override void RemovePosition(Vector2 v)
	{
		base.RemovePosition(v);
		for(int i = this.otherZones.Count-1; i >= 0; i--)
		{
			var r = this.otherZones[i];
			r.RemovePosition(v);
			r.RefreshPositions();
			if(!r.IsAlive )
			{
				//room is no longer available
				this.otherZones.RemoveAt(i);

			}


		}
	}

	public virtual bool AddRoom(Zone zone)
	{
		bool isWithinInMe = IsInZone(zone);
		bool isWithinAnyExitingRoom = false;
		foreach(var room in this.otherZones)
		{
			if (room.IsInZone(zone))
			{
				isWithinAnyExitingRoom = true;
				break;
			}
		}
		if (isWithinInMe && !isWithinAnyExitingRoom)
		{
			otherZones.Add(zone);
			return true;
		}
		else
		{
			Debug.LogError("Cannot add zone " + isWithinInMe + " " + isWithinAnyExitingRoom);
		}
		return false;

	}
}