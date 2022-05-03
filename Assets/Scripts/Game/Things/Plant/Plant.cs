using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Plants provide basic keywords for free as a form of "fruit"

public class Plant : ThingWithNeeds_ShouldBeJust_ThingWithBody
{
	/// <summary>
	/// Plants provide basic keywords for free as a form of "fruit"
	/// </summary>
	internal Dictionary<Game.Keyword, float> resources = new Dictionary<Game.Keyword, float>();

	public override Dictionary<Game.Keyword, float> GetKeywordsOld()
	{
		return resources;
	}
}
