using UnityEngine;
using StoryGenerator.World;

public class Howl :Work
{
	Thing thingToHowl;
	Vector2 positionToHowlTo;

	public Howl(Thing thing, Thing thingToHowl) : base(thing)
	{
		this.thingToHowl = thingToHowl;

	}
	public override bool IsFinished
	{
		get
		{
			bool isThingAtRightPosition = (thingToHowl.XY - positionToHowlTo).magnitude < ZEROf;

			return isThingAtRightPosition && !thingToHowl.IsBeingCarried;
		}
	}
	public override void Do(World world, float timeElapsed)
	{
		base.Do(world, timeElapsed);
	}


}
