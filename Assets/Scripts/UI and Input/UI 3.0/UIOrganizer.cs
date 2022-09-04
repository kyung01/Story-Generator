using StoryGenerator.World;
using System;
using System.Collections.Generic;
using UnityEngine;

public class UIOrganizer : MonoBehaviour
{
	public delegate void DelBttnFeedbackString(string feedback);
	public World world;
	UIEnums.FEEDBACK bttnFeedback;
	public List<UIEnums.DEL_FEEDBACK> OnBttnFeedback = new List<UIEnums.DEL_FEEDBACK>();
	public List<DelBttnFeedbackString> OnBttnFeedbackString = new List<DelBttnFeedbackString>();

	// Use this for initialization
	public void Init()
	{
		var buttons = GetComponentsInChildren<UIButtonFeedback>(true);
		foreach(var b in buttons)
		{
			//Debug.Log(this + "Init " + b.gameObject.name);
			//b.OnFeedback.Add(hdrBttnFeedback);
			b.OnFeedbackString.Add(hdrBttnFeedbackString);
		}
		//UISelectBox.OnSelectedEnd.Add(hdrSelectedWorld);
	}

	

	private void hdrBttnFeedbackString(string value)
	{
		for(int i = 0; i < OnBttnFeedbackString.Count; i++)
		{
			OnBttnFeedbackString[i](value);
			//bttnFeedback = value;
		}
		Debug.Log(this + " hdrBttnFeedbackString " + value);
	}


	// Update is called once per frame
	void Update()
	{
		UISky.SetTime((float)this.world.Time.Subtract(new DateTime(world.Time.Year,world.Time.Month,world.Time.Day) ).TotalSeconds );
		UIClock.Display(this.world.Time);
	}
}