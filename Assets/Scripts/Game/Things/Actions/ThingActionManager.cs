using StoryGenerator.World;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

//T(thing)A(action)M(Manager) TAM
//Manages action of a thing
public class ThingActionManager
{
	public List<Action> actions = new List<Action>();
	public ThingActionManager()
	{

	}

	public bool IsIdl { get { return actions.Count == 0; } }

	public void Update(World world, Thing thing, float timeElapsed)
	{
		if (actions.Count == 0) return;
		var action = actions[0];
		action.Update(world, thing, timeElapsed);
		if (action.IsFinished)
		{
			actions.RemoveAt(0);
		}
	}

	public void MoveToRandomLocationOfDistance(World world, Thing thing, float disToWander)
	{
		float xBegin = thing.X - disToWander;
		float yBegin = thing.Y - disToWander;
		float xEnd = thing.X + disToWander;
		float yEnd = thing.Y + disToWander;
		List<Vector2> positions = new List<Vector2>();
		List<Vector2> positionsDummy = new List<Vector2>();
		for (int i = Mathf.RoundToInt(xBegin); i < xEnd; i++)
		{
			for (int j = Mathf.RoundToInt(yBegin); j < yEnd; j++)
			{
				positions.Add(new Vector2(i, j));
			}
		}
		int numMix = positions.Count;
		while (positions.Count > 0)
		{
			int n = Random.Range(0, positions.Count );
			var v2 = positions[n];
			positions.RemoveAt(n);
			positionsDummy.Add(v2);
		}
		positions = positionsDummy;
		for (int i = 0; i < positions.Count; i++)
		{
			var p = positions[i];
			if (p.x < 0 || p.x >= world.width || p.y < 0 || p.y >= world.height) continue;
			var piece = world.terrain.GetPieceAt((int)p.x, (int)p.y);
			if (piece.IsWalkable)
			{
				this.MoveTo(p.x, p.y);
				break;
			}
		}
	}

	internal void Eat(Thing bestTargetThing,
		Game.Keyword requiredKeyword, float amount)
	{
		this.actions.Add(new Eat(bestTargetThing, requiredKeyword, amount));
	}

	internal void RequestKeywordTransfer(Thing bestTargetThing, Game.Keyword requiredKeyword, float v)
	{

	}

	internal void MoveToTarget(Thing bestTargetThing, float distanceThatsCloseEnough)
	{
		actions.Add(new MoveToTarget(bestTargetThing, distanceThatsCloseEnough));
	}

	public void MoveTo(float x, float y)
	{
		actions.Add(new MoveTo(x, y));

	}
}