using System.Collections.Generic;
using UnityEngine;
using GameEnums;

//part of the body that's of meat
public class MeatBody : BodyBase
{
	float meat = 0;

	public MeatBody()
	{
		this.type = Type.MEATBODY;
	}


	public override Dictionary<Keyword, float> GetKeywords()
	{
		var allKeywords = base.GetKeywords();
		if(!allKeywords.ContainsKey(Keyword.FOOD_MEAT))
		{
			allKeywords.Add(Keyword.FOOD_MEAT, 0);
		}
		allKeywords[Keyword.FOOD_MEAT] += meat;
		//Debug.Log("get keywords " + meat);
		return allKeywords;
	}

	public override void ConsumeKeyword(Keyword keyword, float amount)
	{
		//Debug.Log("ConsumeKeyword " + amount);
		base.ConsumeKeyword(keyword, amount);
		this.meat += amount;

	}
	public override float TakenKeyword(Keyword keywordToRequest, float remainingDebt)
	{
		if(keywordToRequest == Keyword.FOOD_MEAT)
		{
			remainingDebt -= Mathf.Min(meat, remainingDebt);
		}
		return base.TakenKeyword(keywordToRequest, remainingDebt);
	}
}