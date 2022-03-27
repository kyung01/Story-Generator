using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoryGenerator.World;

public class Work
{
	public Thing assignedThing;

	public static float ZEROf = 0.01f;
	public virtual bool IsFinished
	{
		get {
			return true;
		}
	}

	public Work(Thing assignedThing)
	{
		this.assignedThing = assignedThing;
	}

	public void Update(World world, float timeElapsed)
	{
		if (!IsFinished)
		{
			Do(world, timeElapsed);
		}
	}
	public virtual void Do(World world, float timeElapsed){

	}

}
