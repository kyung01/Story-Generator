using StoryGenerator.World;
using StoryGenerator.World.Things.Actors;
using System;
using System.Collections.Generic;
using UnityEngine;

public class UIOrganizer : MonoBehaviour
{
	public delegate void DelBttnFeedbackString(string feedback);

	public List<UIEnums.DEL_FEEDBACK> OnBttnFeedback = new List<UIEnums.DEL_FEEDBACK>();
	public List<DelBttnFeedbackString> OnBttnFeedbackString = new List<DelBttnFeedbackString>();


	World world;

	public UIActorView actorView;

	// Use this for initialization
	public void Init(World world)
	{
		this.world = world;
		var buttons = GetComponentsInChildren<UIButtonFeedback>(true);
		foreach(var b in buttons)
		{
			//Debug.Log(this + "Init " + b.gameObject.name);
			//b.OnFeedback.Add(hdrBttnFeedback);
			b.OnFeedbackString.Add(hdrBttnFeedbackString);
		}
		foreach(var b in buttons)
		{
			b.GetComponent<UIButtonLinked>().SetOpen(false);
		}
		//UISelectBox.OnSelectedEnd.Add(hdrSelectedWorld);

		WorldThingSelector.OnThingsSelected.Add(hdrThingsSelected);
	}

	private void hdrThingsSelected(List<Thing> selectedThings)
	{
		if (selectedThings.Count == 1 && selectedThings[0] is ActorBase )
		{
			this.actorView.gameObject.SetActive(true);
			this.actorView.View((ActorBase)selectedThings[0]);
		}
		else
		{
			this.actorView.gameObject.SetActive(false);
		}
	}

	internal void CancellLastInput()
	{

	}

	private void hdrBttnFeedbackString(UIButtonFeedback button, string value)
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