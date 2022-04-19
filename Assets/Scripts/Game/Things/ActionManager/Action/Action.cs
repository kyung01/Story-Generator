using StoryGenerator.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Action
{
	const float MAX_PROCESSING_TIME = 5;
	internal static float ZEROf = 0.01f;
	internal static float ZEROf_SQUARE = ZEROf * ZEROf;
	bool isFinished = false;
	public string name = "Action";
	public bool IsFinished { get { return isFinished; } }


	float timeProcessed = 0;
	
	internal void finish()
	{
		isFinished = true;
	}

	public void Update(World world, Thing thing, float timeElapsed)
	{
		this.timeProcessed += timeElapsed;
		if(this.timeProcessed > MAX_PROCESSING_TIME)
		{
			UnityEngine.Debug.LogError(this + " TAKING TOO LONG!");
		}
		if (!IsFinished) Do(world,thing,timeElapsed);
	}
	public virtual void Do(World world, Thing thing, float timeElapsed)
	{
	}
}