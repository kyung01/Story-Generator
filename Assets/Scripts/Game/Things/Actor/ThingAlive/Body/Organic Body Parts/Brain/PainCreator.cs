using GameEnums;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Turns negative health change to pain
/// </summary>

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

	private void hdrReceiveKeyword(Thing me,Thing giver, Keyword keyword, float amount)
	{
		if (keyword != Keyword.NEGATIVE_HEALTH_CHANGE)
			return;
		//Health negative change was received
		//Create "pain"
		me.Keyword_Receive(giver, Keyword.PAIN, amount);
	}

}