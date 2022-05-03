using StoryGenerator.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class Thing
{
	ThingBodyManager thingBodyManager;
	public ThingBodyManager MNGBody { get { return this.thingBodyManager; } }

	public void InitBodyManager()
	{
		thingBodyManager = new ThingBodyManager();
		thingBodyManager.Init(this);
		//this.OnUpdate.Add(thingNeedManager.Update);

	}
}
public class ThingBodyManager
{
	Body bodyOld = new Body();
	public Body MainBody { get { return this.bodyOld; } }

	public void AddBody(Body b)
	{
		bodyOld.addBody(b);
	}

	public bool IsBodyAvailableForKeywordExchanges()
	{
		//Check if there is a consciousness in the body,
		//if there is a consciousness, then refuse, if there is no consciousness, then give up
		return false;
	}
	public List<KeywordInformation> hdrGetKeywords()
	{
		//return bodyOld.GetKeywords();
		return null;
	}

	public float hdrTakenKeyword(Game.Keyword keywordToRequest, float requestedAmount)
	{
		float amountIProvidedWithItemsIHave = requestedAmount;
		float remainingDebt = requestedAmount - amountIProvidedWithItemsIHave;
		//float debtPaied = 0;
		if (remainingDebt > 0)
		{
			//here I must provide things with my body lol
			if (!IsBodyAvailableForKeywordExchanges())
			{
				//However some conditons must be mat
				return amountIProvidedWithItemsIHave;
			}
			var availableKeywords = this.bodyOld.GetKeywords();
			if (!availableKeywords.ContainsKey(keywordToRequest) || availableKeywords[keywordToRequest] == 0)
			{
				//my body does not have what's requested here
				return amountIProvidedWithItemsIHave;
			}
			float paied = this.bodyOld.TakenKeyword(keywordToRequest, remainingDebt);

		}
		return amountIProvidedWithItemsIHave;
	}

	public void Init(Thing thing)
	{
		this.bodyOld.Init(thing);
		thing.OnUpdate.Add(this.hdrUpdate);
		thing.OnGetKeywords.Add(this.hdrGetKeywords);
		thing.OnTakenKeyword.Add(this.hdrTakenKeyword);
	}

	internal void hdrUpdate(World world, Thing thing, float timeElapsed)
	{
		bodyOld.Update(world, thing, timeElapsed);

	}


}