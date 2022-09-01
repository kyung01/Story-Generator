using StoryGenerator.World;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIOrganizer : MonoBehaviour
{
	public World world;
	UIEnums.FEEDBACK bttnFeedback;
	public List<UIEnums.DEL_FEEDBACK> OnBttnFeedback = new List<UIEnums.DEL_FEEDBACK>();

	// Use this for initialization
	public void Init()
	{
		var buttons = GetComponentsInChildren<UIButtonFeedback>(true);
		foreach(var b in buttons)
		{
			//Debug.Log(this + "Init " + b.gameObject.name);
			b.OnFeedback.Add(hdrBttnFeedback);
		}
		//UISelectBox.OnSelectedEnd.Add(hdrSelectedWorld);
	}
	private void hdrSelectedWorld(int xBegin, int yBegin, int xEnd, int yEnd)
	{
		Debug.Log(this + " hdrSelectedWorld " + bttnFeedback);
		if (bttnFeedback.isZONES())
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
		Thing.TYPE buildThis = Thing.TYPE.UNDEFINED;
		if (bttnFeedback == UIEnums.FEEDBACK.BUILDS_ROOF)
		{
			buildThis = Thing.TYPE.ROOF;
		}
		else if (bttnFeedback == UIEnums.FEEDBACK.BUILDS_DOOR)
		{
			buildThis = Thing.TYPE.DOOR;
		}
		else if (bttnFeedback == UIEnums.FEEDBACK.BUILDS_WALL)
		{
			buildThis = Thing.TYPE.WALL;
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
					Debug.Log(this + " WorkMagerHowl " + t.type +  " " + t.XY_Int);
					world.PlayerTeam.WorkManager.Howl(t);
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

	// Update is called once per frame
	void Update()
	{
		UIClock.Display(this.world.Time);
	}
}