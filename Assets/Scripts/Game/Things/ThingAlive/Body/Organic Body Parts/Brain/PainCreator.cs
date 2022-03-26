using System;
using UnityEditor;
using UnityEngine;

public class PainCreator : Body
{


	public override void Init(ThingWithBody thing)
	{
		base.Init(thing);
		thing.OnReceiveKeyword.Add(hdrReceiveKeyword);
	}

	private void hdrReceiveKeyword(Thing me,Thing giver, Game.Keyword keyword, float amount)
	{
		if (keyword != Game.Keyword.NEGATIVE_HEALTH_CHANGE)
			return;
		//Health negative change was received
		//Create "pain"
		me.ReceiveKeyword(giver, Game.Keyword.PAIN, amount);
	}

}