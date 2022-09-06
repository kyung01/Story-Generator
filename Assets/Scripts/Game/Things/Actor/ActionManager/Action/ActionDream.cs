using StoryGenerator.World;
using StoryGenerator.World.Things.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Dream : Action
{
	float duration = 10;
	float timeElapsedTotal = 0;
	public Dream() : base(Type.DREAMING)
	{
		this.name = "DREAMING (Action)";
		duration = UnityEngine.Random.Range(3, 10.0f);

	}
	public override void Do(World world, ActorBase thing, float timeElapsed)
	{
		timeElapsedTotal += timeElapsed;

		thing.Keyword_Receive(thing, GameEnums.Keyword.SLEEP, timeElapsed * 3f);


		if(timeElapsedTotal > duration)
		{
			finish();
		}




	}
}
