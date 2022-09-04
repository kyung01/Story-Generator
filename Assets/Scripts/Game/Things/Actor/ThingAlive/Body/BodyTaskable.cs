using StoryGenerator.World;
using System.Collections.Generic;
using UnityEngine;
using GameEnums;
public class BodyTaskable : BodyBase
{
	/// <summary>
	/// This is the list of task this body part can perform
	/// </summary>
	public List<TaskType> tasks = new List<TaskType>();
	
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