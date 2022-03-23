using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//part of the body that's of meat
public class MeatBody : Body
{
	float meat = 0;


	public override Dictionary<Game.Keyword, float> GetKeywords()
	{
		var allKeywords = base.GetKeywords();
		allKeywords[Game.Keyword.FOOD_MEAT] += meat;
		return allKeywords;
	}
	public override void ConsumeKeyword(Game.Keyword keyword, float amount)
	{
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