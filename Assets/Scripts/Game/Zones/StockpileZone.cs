using StoryGenerator.World;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class StockpileZone : Zone
{
	List<Thing.TYPE> acceptableTypes = new List<Thing.TYPE>();
	public StockpileZone()
	{

		var values = System.Enum.GetValues(typeof(Thing.TYPE));
		foreach(Thing.TYPE v in values)
		{
			acceptableTypes.Add(v);
		}
	}

	public bool GetAcceptableEmptyPosition(World world, ref int x,ref  int y)
	{
		List<Vector2> availablePositions = new List<Vector2>();
		foreach(var p in positions)
		{
			if (!world.terrain.IsEmptyAt((int)p.x, (int)p.y)) continue;
			var things = world.GetThingsAt((int)p.x, (int)p.y);
			bool isOccupied = false;
			foreach(var thing in things)
			{
				if(thing is Item)
				{
					if (!thing.IsBeingCarried)
					{
						//this spot is occupied
						isOccupied = true;
						break;
					}
				}
			}
			if (!isOccupied)
			{
				availablePositions.Add(p);
			}
		}
		if (availablePositions.Count == 0) return false;
		var pos = availablePositions[Random.Range(0, availablePositions.Count)];
		x = (int)pos.x;
		y = (int)pos.y;
		return true;
	}

}