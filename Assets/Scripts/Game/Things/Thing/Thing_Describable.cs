using System.Collections.Generic;
using UnityEngine;
using GameEnums;


public class KeywordContainer : ThingModule
{
	/// <summary>
	/// Plants provide basic keywords for free as a form of "fruit"
	/// </summary>
	internal Dictionary<Keyword, float> keywordDescription = new Dictionary<Keyword, float>();


	public Dictionary<Keyword, float> GetDescription()
	{
		var d = new Dictionary<Keyword, float>();
		foreach (var pair in keywordDescription) d.Add(pair.Key, pair.Value);
		return d;
	}
	public override float hdrTakenKeyword(Keyword keywordToRequest, float requestedAmount)
	{
		if (!keywordDescription.ContainsKey(keywordToRequest)) return 0;
		float giveThisAmountTo = Mathf.Min(requestedAmount, keywordDescription[keywordToRequest]);
		keywordDescription[keywordToRequest] -= giveThisAmountTo;
		return giveThisAmountTo;
	}

	public override List<KeywordInformation> hdrGetKeywords()
	{
		List<KeywordInformation> returnList = new List<KeywordInformation>();
		foreach (var pair in keywordDescription)
		{
			returnList.Add(new KeywordInformation(pair.Key, KeywordInformation.State.AVAILABLE, pair.Value));
		}
		return returnList;
	}
	public float Get(Keyword keyword)
	{
		return this.keywordDescription.ContainsKey(keyword) ? this.keywordDescription[keyword] : 0;
	}
	public bool Contains(Keyword keyword)
	{
		return this.keywordDescription.ContainsKey(keyword);
	}

	public void Add(Keyword keyword, int v)
	{
		if (!this.keywordDescription.ContainsKey(keyword)) this.keywordDescription.Add(keyword, 0);

		this.keywordDescription[keyword] += v;
	}
}



public class Thing_Describable : Thing_Interactable {

	KeywordContainer thingContainer;
	public KeywordContainer KeywordContainer { get { return this.thingContainer; } }

	public Thing_Describable(ThingCategory type) : base(type)
	{
		thingContainer = new KeywordContainer();
		thingContainer.Init(this);

	}
}
