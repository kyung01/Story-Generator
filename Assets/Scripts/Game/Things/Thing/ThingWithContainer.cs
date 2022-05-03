using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Plants provide basic keywords for free as a form of "fruit"

public class ThingWithContainer : Thing
{
	/// <summary>
	/// Plants provide basic keywords for free as a form of "fruit"
	/// </summary>
	internal Dictionary<Game.Keyword, float> container = new Dictionary<Game.Keyword, float>();
	public ThingWithContainer()
	{
		this.OnGetKeywords.Add(hdrGetKeyword);
		this.OnTakenKeyword.Add(hdrTakenKeyword);
	}

	private float hdrTakenKeyword(Game.Keyword keywordToRequest, float requestedAmount)
	{
		if (!container.ContainsKey(keywordToRequest)) return 0;
		float giveThisAmountTo = Mathf.Min(requestedAmount, container[keywordToRequest]);
		container[keywordToRequest] -= giveThisAmountTo;
		return giveThisAmountTo;
	}

	public List<KeywordInformation> hdrGetKeyword()
	{
		List<KeywordInformation> returnList = new List<KeywordInformation>();
		foreach (var pair in container)
		{
			returnList.Add(new KeywordInformation(pair.Key, KeywordInformation.State.AVAILABLE, pair.Value));
		}
		return returnList;
	}
}
