using StoryGenerator.World;
using StoryGenerator.World.Things.Actors;
using GameEnums;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Drop :Action
{
	public Drop() : base(Type.DROP) { 
	}
	public override void Do(World world, ActorBase thing, float timeElapsed)
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