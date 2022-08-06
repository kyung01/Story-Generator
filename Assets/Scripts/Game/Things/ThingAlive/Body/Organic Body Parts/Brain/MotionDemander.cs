using StoryGenerator.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

//Part of the brain, it requires a thingAlive to keep moving
//When a thing does not move it produces stress
public class MotionDemander : BodyBase
{
	static float REQUIRED_SPEED_TO_MOVE_AROUND = .3f;
	static float ZERO = 0.01f;

	static float HAPPY_DISTANCE_REDUCED_SPEED = .5f;
	static float STRESS_PER_TICK = 1;
	Vector2 position;

	float happyDistanceIMoved = 0;
	public MotionDemander()
	{

		this.type = Type.BRAIN_MOTION_DEMANDER;
		this.SetName("Brain Part (Motion Demander)");
	}
	public override void Init(Thing thing)
	{
		//Debug.Log("MotionDemander Init");
		base.Init(thing);
		position = thing.XY;
	}
	public override void Update(World world, Thing thing, float timeElapsed)
	{
		base.Update(world, thing, timeElapsed);
		happyDistanceIMoved += (thing.XY - this.position).magnitude;
		happyDistanceIMoved -= REQUIRED_SPEED_TO_MOVE_AROUND * timeElapsed;

		this.position = thing.XY;
		//Debug.Log(this + " Updating " + happyDistanceIMoved);
		if (happyDistanceIMoved < 0)
		{
			//happyDistanceIMoved += REQUIRED_SPEED_TO_MOVE_AROUND;
			thing.Keyword_Consume(Game.Keyword.STILL, STRESS_PER_TICK * timeElapsed);
			return;
		}


		if (happyDistanceIMoved > ZERO)
		{
			float distanceRemovedFromTotal = Mathf.Min(happyDistanceIMoved, HAPPY_DISTANCE_REDUCED_SPEED * timeElapsed);
			happyDistanceIMoved -= distanceRemovedFromTotal;
			thing.Keyword_Consume(Game.Keyword.MOVED, distanceRemovedFromTotal);
		}


	}
}
