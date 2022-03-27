using StoryGenerator.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class RequestKeywordTransfer : Action
{
	Thing targetThing;
	Game.Keyword keywordToRequest;
	float keywordAmount = 0;
	public RequestKeywordTransfer(Thing thing, Game.Keyword keyword, float amount)
	{
		this.targetThing = thing;
		this.keywordToRequest = keyword;
		this.keywordAmount = amount;

	}
	public override void Do(World world, Thing thing, float timeElapsed)
	{
		base.Do(world, thing, timeElapsed);
	}
}
