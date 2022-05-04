using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class HouseZone : ResidentialZone
{
	List<Zone> innerRoomes = new List<Zone>();

	public List<Zone> Rooms
	{
		get
		{
			List<Zone> rooms = new List<Zone>();
			foreach (var r in innerRoomes)
			{
				rooms.Add(r);
			}
			return rooms;
		}
	}
	
	public bool Add(Zone zone)
	{
		bool isWithinInMe = IsInZone(zone);
		bool isWithinAnyExitingRoom = false;
		foreach(var room in this.innerRoomes)
		{
			if (room.IsInZone(zone))
			{
				isWithinAnyExitingRoom = true;
				break;
			}
		}
		if (isWithinInMe && !isWithinAnyExitingRoom)
		{
			innerRoomes.Add(zone);
			return true;
		}
		return false;

	}
}