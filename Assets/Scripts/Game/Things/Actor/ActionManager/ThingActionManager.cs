using StoryGenerator.World;
using StoryGenerator.World.Things.Actors;
using System.Collections.Generic;
using UnityEngine;
using GameEnums;

//T(thing)A(action)M(Manager) TAM
//Manages action of a thing
public class ThingActionManager
{
	public enum State { 
		DEFAULT,//default state
		USER_CONTROLLING,// user is controlling TAM

		END
	}

	public enum PriorityLevel
	{
		DEFAULT, // orders are queued in order
		FIRST, //order is queued to the front
		FOCUSE, // order queue is emptied then the new order is added 
		USER,// this is an order from a user, this flag is required when TAM is in "user controlling mode"
		END
	}
	void addAction(Action action, PriorityLevel priorityLevel)
	{
		switch (priorityLevel)
		{
			case PriorityLevel.DEFAULT:
				actions.Add(action);
				break;
			case PriorityLevel.FIRST:
				actions.Insert(0,action);
				break;
			case PriorityLevel.FOCUSE:
				actions.Clear();
				actions.Add(action);
				break;
			case PriorityLevel.USER:
				break;
			case PriorityLevel.END:
				break;
			default:
				break;
		}
	}
	public List<Action> actions = new List<Action>();

	

	public ThingActionManager()
	{

	}


	public bool IsIdl { get { return actions.Count == 0; } }

	

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
			//var piece = world.IsWalkableAt((int)p.x, (int)p.y);
			if (world.IsWalkableAt((int)p.x, (int)p.y))
			{
				this.MoveTo(p.x, p.y);
				break;
			}
		}
	}

	internal void Dream(World world)
	{
		var action = new Dream();
		addAction(action, PriorityLevel.DEFAULT);
	}

	internal void Haul(Thing_Interactable thingToHowl, float x, float y)
	{
		var action = new ActionManagerAction.Haul(thingToHowl,x,y);
		addAction(action, PriorityLevel.DEFAULT);
	}

	internal void Drop(PriorityLevel priority = PriorityLevel.DEFAULT)
	{
		Drop action = new Drop();
		addAction(action, priority);
	}

	public void Carry(Thing_Interactable thingToCarry, PriorityLevel priority = PriorityLevel.DEFAULT)
	{
		Carry action = new Carry(thingToCarry);
		addAction(action, priority);
	}

	public void Hunt(
		Thing bestTargetThing, 
		Keyword requiredKeyword, float desiredKeywordAmount)
	{
		this.actions.Add(new Hunt(bestTargetThing, requiredKeyword, desiredKeywordAmount));

	}

	public void Eat(
		Thing bestTargetThing,
		Keyword requiredKeyword, float desiredKeywordAmount)
	{
		this.actions.Add(new Eat(bestTargetThing, requiredKeyword, desiredKeywordAmount));
	}


	public void Flee(Thing fleeFromThisThing, float minFleeDistance, PriorityLevel priorityLevel = PriorityLevel.DEFAULT)
	{
		var action = new Flee(fleeFromThisThing, minFleeDistance);
		addAction(action, priorityLevel);
	}

	public void Sleep(World world, ISleepableStructure bedish)
	{
		var action = new SleepAction(bedish);
		addAction(action, PriorityLevel.DEFAULT);
	}

	internal void RequestKeywordTransfer(Thing bestTargetThing, Keyword requiredKeyword, float v)
	{

	}

	internal void MoveToTarget(Thing bestTargetThing, float distanceThatsCloseEnough, PriorityLevel priorityLevel = PriorityLevel.DEFAULT)
	{
		var action = new MoveToTarget(bestTargetThing, distanceThatsCloseEnough);
		addAction(action, priorityLevel);
	}

	internal bool GetNextDestination(ref Vector2 dest)
	{
		if (this.actions.Count == 0) return false;
		if (!(this.actions[0] is  MoveTo)) return false;
		var move = (MoveTo)actions[0];
		if (!move.IsNextDestAvail) return false;
		dest = move.NextDestinationXY;

		return true;

	}

	public void MoveTo(Vector2 pos, PriorityLevel priorityLevel = PriorityLevel.DEFAULT)
	{
		this.MoveTo(pos.x, pos.y, priorityLevel);
	}
	public void MoveTo(float x, float y, PriorityLevel priorityLevel = PriorityLevel.DEFAULT)
	{
		var action = new MoveToPosition(x, y);
		addAction(action, priorityLevel);
	}

	public void Update(World world, ActorBase thing, float timeElapsed)
	{
		if (actions.Count == 0) return;
		var action = actions[0];
		action.Update(world, thing, timeElapsed);
		if (action.IsFinished)
		{
			actions.RemoveAt(0);
		}
	}
}