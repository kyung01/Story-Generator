using StoryGenerator.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Once body is received SLEEPY_HORMONE, it turns SLEEPY_HORMONE to SLEEPY
/// </summary>
public class SleepNerve : BodyBase
{
	public override void Update(World world, Thing thing, float timeElapsed)
	{
		base.Update(world, thing, timeElapsed);
	}

}
