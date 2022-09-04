using StoryGenerator.World;
using System;
using System.Collections;
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

	

	private void hdrSelectedWorld(int xBegin, int yBegin, int xEnd, int yEnd)
	{
		Debug.Log(this + " hdrSelectedWorld " + bttnFeedback);
		if (bttnFeedback.IsZONES())
		{
			CreateZones(xBegin, yBegin, xEnd, yEnd);
		}
		if (bttnFeedback.isBUILDS())
		{
			CreateBuildings(xBegin, yBegin, xEnd, yEnd);
		}
		if (bttnFeedback.isTASK())
		{
			CreateTask(xBegin, yBegin, xEnd, yEnd);
		}


	}
	
	void CreateBuildings(int xBegin, int yBegin, int xEnd, int yEnd)
	{
		Debug.Log(this + " CreateBuildings");
		Game.CATEGORY buildThis = Game.CATEGORY.UNDEFINED;
		if (bttnFeedback == UIEnums.FEEDBACK.BUILD_ROOF)
		{
			buildThis = Game.CATEGORY.ROOF;
		}
		else if (bttnFeedback == UIEnums.FEEDBACK.BUILD_DOOR)
		{
			buildThis = Game.CATEGORY.DOOR;
		}
		else if (bttnFeedback == UIEnums.FEEDBACK.BUILD_WALL)
		{
			buildThis = Game.CATEGORY.WALL;
		}
		else if (bttnFeedback == UIEnums.FEEDBACK.BUILD_BED)
		{
			buildThis = Game.CATEGORY.BED;
		}

		for (int x = xBegin; x <= xEnd; x++)
		{
			for (int y = yBegin; y <= yEnd; y++)
			{

				world.Build(buildThis, x, y);
			}
		}
	}

	private void CreateTask(int xBegin, int yBegin, int xEnd, int yEnd)
	{
		Debug.Log(this + " CreateTask");
		WorldThingSelector selector = new WorldThingSelector();
		selector.Select(this.world, xBegin, yBegin, 1 + xEnd - xBegin, 1 + yEnd - yBegin);
		var things = selector.ThingsCurrentlySelected;

		switch (this.bttnFeedback)
		{
			case UIEnums.FEEDBACK.TASKS:
				break;
			case UIEnums.FEEDBACK.TASKS_HAUL:
				foreach(var t in things)
				{
					if(t is Thing_Interactable)
					{
						Debug.Log(this + " WorkMagerHowl " + t.Category + " " + t.XY_Int);
						world.PlayerTeam.WorkManager.Howl((Thing_Interactable)t);

					}
				}
				break;
			default:
				break;
		}
	}

	private void CreateZones(int xBegin, int yBegin, int xEnd, int yEnd)
	{
		Debug.Log(this + " CreateZones");
		switch (this.bttnFeedback)
		{
			case UIEnums.FEEDBACK.ZONES_STOCKPILE:
				world.zoneOrganizer.BuildStockpileZone(xBegin, yBegin, xEnd, yEnd);
				break;
			case UIEnums.FEEDBACK.ZONES_HOUSING:
				break;
			case UIEnums.FEEDBACK.ZONES_HOUSING_HOUSE:
				world.zoneOrganizer.BuildHouseZone(xBegin, yBegin, xEnd, yEnd);
				break;
			case UIEnums.FEEDBACK.ZONES_HOUSING_BEDROOM:
				world.zoneOrganizer.BuildBedroom(xBegin, yBegin, xEnd, yEnd);
				break;
			case UIEnums.FEEDBACK.ZONES_HOUSING_BATHROOM:
				world.zoneOrganizer.BuildBathroom(xBegin, yBegin, xEnd, yEnd);
				break;
			case UIEnums.FEEDBACK.ZONES_HOUSING_LIVINGROOM:
				world.zoneOrganizer.BuildLivingroom(xBegin, yBegin, xEnd, yEnd);
				break;
			default:
				break;
		}
	}

	private void hdrBttnFeedback(UIEnums.FEEDBACK value)
	{
		OnBttnFeedback.Raise(value);
		Debug.Log(this + " hdrBttnFeedback " + value);
		bttnFeedback = value;
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