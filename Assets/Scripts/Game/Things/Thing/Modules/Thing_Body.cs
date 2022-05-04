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
	}
}
public class ThingBodyManager : ThingModule
{
	Body body = new Body();
	public Body MainBody { get { return this.body; } }

	public void AddBody(Body b)
	{
		body.addBody(b);
	}

	public bool IsBodyAvailableForKeywordExchanges()
	{
		//Check if there is a consciousness in the body,
		//if there is a consciousness, then refuse, if there is no consciousness, then give up
		return false;
	}


	public override void Init(Thing thing)
	{
		base.Init(thing);

		this.body.Init(thing);
	}

	public override List<KeywordInformation> hdrGetKeywords()
	{
		var keywords = body.GetKeywords();
		List<KeywordInformation> keywordsReturn = new List<KeywordInformation>();
		foreach (var k in keywords)
		{
			keywordsReturn.Add(new KeywordInformation(k.Key, KeywordInformation.State.LOCKED, k.Value));
		}
		//return bodyOld.GetKeywords();
		return keywordsReturn;
	}

	public override float hdrTakenKeyword(Game.Keyword keywordToRequest, float requestedAmount)
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
			var availableKeywords = this.body.GetKeywords();
			if (!availableKeywords.ContainsKey(keywordToRequest) || availableKeywords[keywordToRequest] == 0)
			{
				//my body does not have what's requested here
				return amountIProvidedWithItemsIHave;
			}
			float paied = this.body.TakenKeyword(keywordToRequest, remainingDebt);

		}
		return amountIProvidedWithItemsIHave;
	}


	public override void hdrUpdate(World world, Thing thing, float timeElapsed)
	{
		body.Update(world, thing, timeElapsed);

	}


}