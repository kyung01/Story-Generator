using StoryGenerator.World;
using StoryGenerator.World.Things.Actors;
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

	float getScoreOfPosition(World world,Thing thingToConsiderWhenAddingScore, int x, int y)
	{
		float score = 0;
		var things = world.GetThingsAt(x,y);
		bool isOccupied = false;
		foreach (var thing in things)
		{
			if (thing is Item)
			{
				if (!((Thing_Interactable) thing).IsBeingCarried)
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
		score += things.Count;
		if (thingToConsiderWhenAddingScore != null)
		{
			//Debug.Log("ThingToConsider is not null");
			if ((new Vector2(x,y) - thingToConsiderWhenAddingScore.XY_Int).magnitude < 0.01f)
			{
				//Debug.Log("SubtractingSDcore -" + (1 + thingToConsiderWhenAddingScore.CountAllCarryingThings()));
				score -= 1;
				if(thingToConsiderWhenAddingScore is ActorBase)
				{
					score -= ((ActorBase)thingToConsiderWhenAddingScore).CountAllCarryingThings();

				}
			}
		}
		return score;
	}

	public bool IsPositionEfficient(World world, Thing thingToConsiderWhenAddingScore, int x, int y)
	{
		List<Vector3> availablePositionsnew = new List<Vector3>();
		foreach (var p in positions)
		{
			if (!world.terrain.IsEmptyAt((int)p.x, (int)p.y)) continue;
			float score = getScoreOfPosition(world, thingToConsiderWhenAddingScore, (int)p.x, (int)p.y);
			var things = world.GetThingsAt((int)p.x, (int)p.y);
			bool isOccupied = false;
			foreach (var thing in things)
			{
				if (thing is Item)
				{
					if (! ((Item)thing).IsBeingCarried)
					{
						//this spot is occupied
						isOccupied = true;
						break;
					}
				}
			}
			if (!isOccupied)
			{
				//availablePositionsOld.Add(p);
				availablePositionsnew.Add(new Vector3(p.x, p.y, score));
				//Debug.Log(new Vector3(p.x, p.y, score));
			}
		}
		if (availablePositionsnew.Count == 0) return false;
		availablePositionsnew = availablePositionsnew.OrderBy(p => p.z).ToList();

		float lowestScore = availablePositionsnew[0].z;
		float askedPositionsScore = getScoreOfPosition(world, thingToConsiderWhenAddingScore, x, y);
		return askedPositionsScore <= lowestScore;
	}
	
	bool hprGetBestAcceptableEmptyPosition(World world, ref int x, ref int y, 
		Thing thingToConsiderWhenAddingScore = null)
	{
		//List<Vector2> availablePositionsOld = new List<Vector2>();
		//x,y are positions
		//Z value stores negative-score, the lower it is, better the position is
		List<Vector3> availablePositionsnew = new List<Vector3>();
		foreach (var p in positions)
		{
			if (!world.terrain.IsEmptyAt((int)p.x, (int)p.y)) continue;
			float score = getScoreOfPosition(world,thingToConsiderWhenAddingScore,(int)p.x,(int)p.y);
			var things = world.GetThingsAt((int)p.x, (int)p.y);
			bool isOccupied = false;
			foreach (var thing in things)
			{
				if (thing is Item)
				{
					if (!((Item)thing).IsBeingCarried)
					{
						//this spot is occupied
						isOccupied = true;
						break;
					}
				}
			}
			//if(things)
			if (thingToConsiderWhenAddingScore != null)
			{
			}
			else
			{

				Debug.Log("ThingToConsider is null");
			}

			if (!isOccupied)
			{
				//availablePositionsOld.Add(p);
				availablePositionsnew.Add(new Vector3(p.x, p.y, score));
				//Debug.Log(new Vector3(p.x, p.y, score));
			}
		}
		if (availablePositionsnew.Count == 0) return false;
		availablePositionsnew = availablePositionsnew.OrderBy(p => p.z).ToList();
		float lowestScore = availablePositionsnew[0].z;
		for(int i = availablePositionsnew.Count-1;i >= 0; i--)
		{
			if(availablePositionsnew[i].z > lowestScore)
			{
				availablePositionsnew.RemoveAt(i);
			}
		}
		var pos = availablePositionsnew[Random.Range(0, availablePositionsnew.Count)];
		//Debug.Log(this + "Returning a position " + pos );
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