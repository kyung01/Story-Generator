using StoryGenerator.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Drop :Action
{
	public override void Do(World world, Thing thing, float timeElapsed)
	{
		base.Do(world, thing, timeElapsed);
		if (thing.IsCarrying)
		{
			thing.Drop();
			finish();
			return;
		}
	}
}