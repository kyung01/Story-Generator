using StoryGenerator.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Action
{
	public virtual bool IsFinished { get { return true; } }

	public virtual void Update(World world, Thing thing, float timeElapsed)
	{
	}
}