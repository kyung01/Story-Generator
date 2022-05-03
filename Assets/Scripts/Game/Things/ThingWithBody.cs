using StoryGenerator.World;
using System;
using System.Collections.Generic;
using UnityEngine;



public class ThingWithBody : ThingDestructable
{
	internal Body bodyOld = new Body();

	public virtual  bool IsBodyAvailableForKeywordExchanges
	{
		get
		{
			return health < 30;
		}
	}


	public override Dictionary<Game.Keyword, float> GetKeywordsForHunter()
	{
		var keywords = base.GetKeywordsForHunter();
		var keywordsBody = bodyOld.GetKeywords();
		foreach (var pair in keywordsBody)
		{
			if (!keywords.ContainsKey(pair.Key))
			{
				keywords.Add(pair.Key, 0);

			}
			keywords[pair.Key] += pair.Value;

		}
		return keywords;
	}
	public override float TakenKeywordOld(Game.Keyword keywordToRequest, float requestedAmount)
	{
		float amountIProvidedWithItemsIHave = base.TakenKeywordOld(keywordToRequest, requestedAmount);
		float remainingDebt = requestedAmount - amountIProvidedWithItemsIHave;
		float debtPaied = 0;
		if(remainingDebt > 0)
		{
			//here I must provide things with my body lol
			if (!IsBodyAvailableForKeywordExchanges)
			{
				//However some conditons must be mat
				return amountIProvidedWithItemsIHave;
			}
			var availableKeywords = this.bodyOld.GetKeywords();
			if(!availableKeywords.ContainsKey(keywordToRequest) || availableKeywords[keywordToRequest] == 0)
			{
				//my body does not have what's requested here
				return amountIProvidedWithItemsIHave;
			}
			float paied = this.bodyOld.TakenKeyword(keywordToRequest, remainingDebt);

		}
		return amountIProvidedWithItemsIHave;
	}

	public override void Init(World world)
	{
		base.Init(world);
		this.bodyOld.Init(this);
	}

	public override void Update(World world, float timeElapsed)
	{
		base.Update(world, timeElapsed);
		bodyOld.Update(world, this, timeElapsed);

	}


}