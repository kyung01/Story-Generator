using StoryGenerator.World;
using System;
using System.Collections.Generic;
using UnityEngine;



public class ThingWithBody : ThingDestructable
{
	internal Body body = new Body();

	public virtual bool IsReadyToProvideWithBody
	{
		get
		{
			return false;
		}

	}

	public override Dictionary<Game.Keyword, float> GetKeywordsForHunter()
	{
		var keywords = base.GetKeywordsForHunter();
		var keywordsBody = body.GetKeywords();
		foreach (var pair in keywordsBody)
		{
			keywords[pair.Key] += pair.Value;

		}
		return keywords;
	}
	public override float TakenKeyword(Game.Keyword keywordToRequest, float requestedAmount)
	{
		float amountIProvidedWithItemsIHave = base.TakenKeyword(keywordToRequest, requestedAmount);
		float remainingDebt = requestedAmount - amountIProvidedWithItemsIHave;
		float debtPaied = 0;
		if(remainingDebt > 0)
		{
			//here I must provide things with my body lol
			if (!IsReadyToProvideWithBody)
			{
				//However some conditons must be mat
				return amountIProvidedWithItemsIHave;
			}
			var availableKeywords = this.body.GetKeywords();
			if(!availableKeywords.ContainsKey(keywordToRequest) || availableKeywords[keywordToRequest] == 0)
			{
				//my body does not have what's requested here
				return amountIProvidedWithItemsIHave;
			}
			float paied = this.body.TakenKeyword(keywordToRequest, remainingDebt);

		}
		return base.TakenKeyword(keywordToRequest, requestedAmount);
	}

	public override void Init(World world)
	{
		base.Init(world);
		this.body.Init(this);
	}

	public override void Update(World world, float timeElapsed)
	{
		base.Update(world, timeElapsed);

	}


}