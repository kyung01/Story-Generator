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
		this.type = TYPE.STOCKPILE;
		var values = System.Enum.GetValues(typeof(Thing.TYPE));
		foreach(Thing.TYPE v in values)
		{
			acceptableTypes.Add(v);
		}
	}
	bool hprGetBestAcceptableEmptyPosition(World world, ref int x, ref int y, Thing thingToConsiderWhenAddingScore = null)
	{
		//List<Vector2> availablePositionsOld = new List<Vector2>();
		//x,y are positions
		//Z value stores negative-score, the lower it is, better the position is
		List<Vector3> availablePositionsnew = new List<Vector3>();
		foreach (var p in positions)
		{
			if (!world.terrain.IsEmptyAt((int)p.x, (int)p.y)) continue;
			float score = 0;
			var things = world.GetThingsAt((int)p.x, (int)p.y);
			bool isOccupied = false;
			foreach (var thing in things)
			{
				if (thing is Item)
				{
					if (!thing.IsBeingCarried)
					{
						//this spot is occupied
						isOccupied = true;
						break;
					}
				}
			}
			if (isOccupied)
			{
				score += 1000;
			}
			var things = world.GetThingsAt((int)p.x, (int)p.y);
			if(things)
			score += world.GetThingsAt((int)p.x, (int)p.y).Count;
			if (thingToConsiderWhenAddingScore != null)
			{
				Debug.Log("ThingToConsider is not null");
				if((p-thingToConsiderWhenAddingScore.XY_Int).magnitude< 0.01f)
				{
					Debug.Log("SubtractingSDcore -"  + (1+ thingToConsiderWhenAddingScore.CountAllCarryingThings()));
					score -= 1;
					score -= thingToConsiderWhenAddingScore.CountAllCarryingThings();
				}
			}
			else
			{

				Debug.Log("ThingToConsider is null");
			}

			if (!isOccupied)
			{
				//availablePositionsOld.Add(p);
				availablePositionsnew.Add(new Vector3(p.x, p.y, score));
				Debug.Log(new Vector3(p.x, p.y, score));
			}
		}
		if (availablePositionsnew.Count == 0) return false;
		availablePositionsnew = availablePositionsnew.OrderBy(p => p.z).ToList();
		var pos = availablePositionsnew[0];
		Debug.Log(this + "Returning a position " + pos );
		x = (int)pos.x;
		y = (int)pos.y;
		return true;
	}
	public bool GetBestAcceptableEmptyPosition(World world, ref int x,ref  int y)
	{
		return hprGetBestAcceptableEmptyPosition(world, ref x, ref y, null);
	}

	internal bool GetBestAcceptableEmptyPositionForThing(World world, ref int x, ref int y, Thing thing)
	{
		return hprGetBestAcceptableEmptyPosition(world, ref x, ref y, thing);
	}
}