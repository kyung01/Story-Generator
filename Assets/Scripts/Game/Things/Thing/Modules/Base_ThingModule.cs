using StoryGenerator.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ThingModule
{
	public virtual void Init(Thing thing)
	{
		thing.OnTakenKeyword.Add(hdrTakenKeyword);
		thing.OnGetKeywords.Add(hdrGetKeywords);
		thing.OnUpdate.Add(hdrUpdate);

	}
	public virtual List<KeywordInformation> hdrGetKeywords()
	{
		return null;
	}

	public virtual float hdrTakenKeyword(Game.Keyword keywordToRequest, float requestedAmount)
	{
		return 0;
	}

	public virtual void hdrUpdate(World world, Thing thing, float timeElapsed)
	{
	}
}