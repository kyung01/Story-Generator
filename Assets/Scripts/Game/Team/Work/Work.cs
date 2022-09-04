using GameEnums;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoryGenerator.World;
using StoryGenerator.World.Things.Actors;

public class Work
{
	public ActorBase assignedWorker;

	public static float ZEROf = 0.01f;
	bool isFinished = false;
	public bool IsFinished
	{
		get {
			return isFinished;
		}
	}
	public bool IsWorkerAssigned
	{
		get { return this.assignedWorker != null; }
	}

	internal void finish()
	{
		isFinished = true;
	}
	public Work(ActorBase assignedThing)
	{
		this.assignedWorker = assignedThing;
	}

	public void Update( World world,   float timeElapsed)
	{
		if (!IsFinished)
		{
			Do(world, timeElapsed);
		}
	}
	public virtual void Do( World world, float timeElapsed){

	}

}
