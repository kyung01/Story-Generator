using StoryGenerator.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Action
{
	internal static float ZEROf = 0.01f;
	internal static float ZEROf_SQUARE = ZEROf * ZEROf;
	bool isFinished = false;
	public string name = "Action";
	public bool IsFinished { get { return isFinished; } }

	
	internal void finish()
	{
		isFinished = true;
	}

	public virtual void Update(World world, Thing thing, float timeElapsed)
	{
		if (!IsFinished) Do(world,thing,timeElapsed);
	}
	public virtual void Do(World world, Thing thing, float timeElapsed)
	{
	}
}