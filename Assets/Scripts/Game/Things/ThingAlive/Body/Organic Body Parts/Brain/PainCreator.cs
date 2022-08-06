using System;
using UnityEditor;
using UnityEngine;

public class PainCreator : BodyBase
{


	public PainCreator()
	{

		this.type = Type.BRAIN_PAIN_CREATOR;
	}
	public override void Init(Thing thing)
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
		me.Keyword_Receive(giver, Game.Keyword.PAIN, amount);
	}

}