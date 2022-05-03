using StoryGenerator.World;
using System.Collections.Generic;
using UnityEngine;

public class BodyTaskable : Body
{
	/// <summary>
	/// This is the list of task this body part can perform
	/// </summary>
	public List<Game.TaskType> tasks = new List<Game.TaskType>();
	
	bool isReady = true;
	float timePassed = 0;
	float timeToGetReady = 1.0f;

	public bool IsReady { get { return this.isReady; } }

	public void Use()
	{
		this.isReady = false;
		timePassed = 0;
	}



	public override void Update(World world, Thing thing, float timeElapsed)
	{
		base.Update(world, thing, timeElapsed);
		if (!isReady)
		{
			timePassed += timeElapsed;
			if(timePassed >= timeToGetReady)
			{
				this.isReady = true;
				timePassed = 0;
			}
		}

	}

}