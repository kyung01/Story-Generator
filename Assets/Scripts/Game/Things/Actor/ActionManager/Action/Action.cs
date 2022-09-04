using StoryGenerator.World;
using StoryGenerator.World.Things.Actors;
using GameEnums;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Action
{
	const float MAX_PROCESSING_TIME = 10;
	internal static float ZEROf = 0.01f;
	internal static float ZEROf_SQUARE = ZEROf * ZEROf;

	public enum Type {
		CARRY,
		DROP,
		EAT,
		HUNT,
		HAUL,
		FLEE,
		MOVE_TO
	}

	Type t;

	public Type T { get { return this.t; } }


	bool isFinished = false;
	public string name = "Action";
	public bool IsFinished { get { return isFinished; } }


	float timeProcessed = 0;

	public Action(Type type)
	{
		this.t = type;
	}
	
	internal void finish()
	{
		isFinished = true;
	}

	public void Update(World world, ActorBase thing, float timeElapsed)
	{
		this.timeProcessed += timeElapsed;
		if(this.timeProcessed > MAX_PROCESSING_TIME)
		{
			UnityEngine.Debug.LogError(this + " TAKING TOO LONG!");
			finish();
		}
		if (!IsFinished) Do(world,thing,timeElapsed);
	}
	public virtual void Do(World world, ActorBase thing, float timeElapsed)
	{
	}
}