using System.Collections.Generic;
using UnityEngine;


public class KeywordContainer : ThingModule
{
	/// <summary>
	/// Plants provide basic keywords for free as a form of "fruit"
	/// </summary>
	internal Dictionary<Game.Keyword, float> container = new Dictionary<Game.Keyword, float>();


	public override float hdrTakenKeyword(Game.Keyword keywordToRequest, float requestedAmount)
	{
		if (!container.ContainsKey(keywordToRequest)) return 0;
		float giveThisAmountTo = Mathf.Min(requestedAmount, container[keywordToRequest]);
		container[keywordToRequest] -= giveThisAmountTo;
		return giveThisAmountTo;
	}

	public override List<KeywordInformation> hdrGetKeywords()
	{
		List<KeywordInformation> returnList = new List<KeywordInformation>();
		foreach (var pair in container)
		{
			returnList.Add(new KeywordInformation(pair.Key, KeywordInformation.State.AVAILABLE, pair.Value));
		}
		return returnList;
	}
	public float Get(Game.Keyword keyword)
	{
		return this.container.ContainsKey(keyword) ? this.container[keyword] : 0;
	}
	public bool Contains(Game.Keyword keyword)
	{
		return this.container.ContainsKey(keyword);
	}

	public void Add(Game.Keyword keyword, int v)
	{
		if (!this.container.ContainsKey(keyword)) this.container.Add(keyword, 0);

		this.container[keyword] += v;
	}
}



public class Thing_Describable : Thing_Interactable {

	KeywordContainer thingContainer;
	public KeywordContainer KeywordContainer { get { return this.thingContainer; } }

	public Thing_Describable(Game.CATEGORY type) : base(type)
	{
		thingContainer = new KeywordContainer();
		thingContainer.Init(this);

	}
}
