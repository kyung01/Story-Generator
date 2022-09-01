using StoryGenerator.World;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Flee : Action
{
	Thing thingToRunAwayFrom;
	float minDistanceToPutBetween;
	bool isNewPathNeeded = true;
	public Flee(Thing thingToRunAwayFrom, float minDistanceToPutBetween):base(Type.FLEE)
	{
		this.name = "Flee";
		this.thingToRunAwayFrom = thingToRunAwayFrom;
		this.minDistanceToPutBetween = minDistanceToPutBetween;
	}
	public override void Do(World world, Thing thing, float timeElapsed)
	{
		base.Do(world, thing, timeElapsed);
		if (isNewPathNeeded)
		{
			//get new path
			
			//world.
			//isNewPathNeeded = false;
		}
		if( (thing.XY - thingToRunAwayFrom.XY).magnitude >= minDistanceToPutBetween)
		{
			finish();
			return;
		}
		int xMin = Mathf.Max(0, Mathf.RoundToInt(thing.X - minDistanceToPutBetween));
		int xMax = Mathf.Min(world.width, Mathf.RoundToInt(thing.X + minDistanceToPutBetween));
		int yMin = Mathf.Max(0, Mathf.RoundToInt(thing.Y - minDistanceToPutBetween));
		int yMax = Mathf.Min(world.height, Mathf.RoundToInt(thing.Y + minDistanceToPutBetween));
		List<Vector2> availablePositions = new List<Vector2>();
		for (int i = xMin; i < xMax; i++)
			for (int j = yMin; j < yMax; j++)
			{
				if (!world.terrain.GetPieceAt(i, j).IsWalkable || !world.terrain.GetPieceAt(i, j).IsSightable)
				{
					continue;
				}
				availablePositions.Insert(Random.Range(0, 1 + availablePositions.Count), new Vector2(i, j));
			}
		if (availablePositions.Count == 0)
		{
			Debug.LogError(this + " ERROR::UnavailablePosition");
			finish();
			return;
		}
		Vector2 pos = getTheBestPosition(availablePositions, thing.XY, thingToRunAwayFrom.XY);
		thing.TAM.MoveTo(pos.x, pos.y, ThingActionManager.PriorityLevel.FIRST);
	}

	Vector2 getTheBestPosition(List<Vector2> availablePositions, Vector2 myPosition, Vector2 threatsPosition)
	{
		Vector2 posSelected = new Vector2();
		float score = 0;
		for(int i = 0; i< availablePositions.Count; i++)
		{
			var pos = availablePositions[i];
			var howFarFromMe = (pos - myPosition).magnitude;
			var howFarFromThreat = (pos - threatsPosition).magnitude;
			var itsScore = howFarFromThreat - howFarFromMe;
			if(itsScore > score)
			{
				posSelected = pos;
			}
		}
		return posSelected;
	}
}