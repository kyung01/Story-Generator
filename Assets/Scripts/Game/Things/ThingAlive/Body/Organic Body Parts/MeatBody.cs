using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//part of the body that's of meat
public class MeatBody : BodyBase
{
	float meat = 0;


	public override Dictionary<Game.Keyword, float> GetKeywords()
	{
		var allKeywords = base.GetKeywords();
		if(!allKeywords.ContainsKey(Game.Keyword.FOOD_MEAT))
		{
			allKeywords.Add(Game.Keyword.FOOD_MEAT, 0);
		}
		allKeywords[Game.Keyword.FOOD_MEAT] += meat;
		//Debug.Log("get keywords " + meat);
		return allKeywords;
	}

	public override void ConsumeKeyword(Game.Keyword keyword, float amount)
	{
		//Debug.Log("ConsumeKeyword " + amount);
		base.ConsumeKeyword(keyword, amount);
		this.meat += amount;

	}
	public override float TakenKeyword(Game.Keyword keywordToRequest, float remainingDebt)
	{
		if(keywordToRequest == Game.Keyword.FOOD_MEAT)
		{
			remainingDebt -= Mathf.Min(meat, remainingDebt);
		}
		return base.TakenKeyword(keywordToRequest, remainingDebt);
	}
}