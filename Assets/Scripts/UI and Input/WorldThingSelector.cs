﻿using StoryGenerator.World;
using GameEnums;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

public class WorldThingSelector
{
	public delegate void DEL_THING_SELECTED(List<Thing> selectedThings);
	public static List<DEL_THING_SELECTED> OnThingsSelected = new List<DEL_THING_SELECTED>();


	List<Thing> thingsISelected = null;

	public List<Thing> ThingsCurrentlySelected { 
		get
		{
			if (thingsISelected == null) return new List<Thing>();
			return thingsISelected;
		} 
	}




	#region Helpers
	
	bool hprIsSame(List<Thing> a, List<Thing> b)
	{
		if (a.Count == b.Count)
		{
			bool isSame = true;
			for (int i = 0; i < b.Count; i++)
			{
				if (a[i] != b[i])
				{
					isSame = false;
					break;
				}
			}
			if (isSame)
			{
				return true;
			}
		}
		return false;
	}

	bool hprIsThingISelectedInTheList(List<List<Thing>> things)
	{
		if (thingsISelected == null) return false;
		foreach (var otherThing in things)
		{
			if (hprIsSame(otherThing, this.thingsISelected))
				return true;
		}
		return false;
	}

	#endregion

	List<List<Thing>> getAllThingsInWorld(World world, int x, int y, int width, int height)
	{
		List<Thing> things = new List<Thing>();
		Dictionary<ThingCategory, List<Thing>> sortedThings = new Dictionary<ThingCategory, List<Thing>>();
		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				int pX = x + i;
				int pY = y + j;
				//Debug.Log(this + "::" +pX + " " + pY+ "");
				things.AddRange(world.GetThingsAt(pX, pY));
			}
		}
		foreach (var t in things)
		{
			if (!sortedThings.ContainsKey(t.Category))
			{
				sortedThings.Add(t.Category, new List<Thing>());
			}
			sortedThings[t.Category].Add(t);
		}
		List<List<Thing>> thingsByCount = new List<List<Thing>>();
		foreach (var pair in sortedThings)
		{
			thingsByCount.Add(pair.Value);
		}
		var d = thingsByCount.OrderByDescending(s => s.Count).ToList();
		return d;
	}

	void setSelectedThings(List<Thing> things)
	{
		this.thingsISelected = things;
		//this.countOfThingsSelected = things.Count;
		//this.typeOfThingSelected = things[0].Category;
	}

	public void SelectFromTo(World world, Vector2 from, Vector2 to)
	{
		this.Select(world, (int)from.x, (int)from.y, (int)(1 + to.x - from.x),(int)( 1 + to.y - from.y) );
	}
	
	public void Select(World world, int x, int y, int width, int height)
	{
		var thingsList = getAllThingsInWorld(world, x, y, width, height);

		for (int i = 0; i < thingsList.Count; i++)
		{
			//Debug.Log(i + " " + thingsList[i][0].Category + "  " + thingsList[i].Count);

		}

		if (thingsList.Count == 0){
			//Debug.Log("Case Nothing to select");
			setSelectedThings(new List<Thing>());
		}
		else if (!hprIsThingISelectedInTheList(thingsList))
		{
			//Debug.Log("Case First time selecting");
			setSelectedThings(thingsList[0]);

		}
		else
		{
			//Debug.Log("Case SelectTheNextElement");
			for (int i = 0; i< thingsList.Count; i++)
			{
				if(hprIsSame(thingsList[i], this.thingsISelected)){
					int nextIndex = (i + 1) % thingsList.Count;
					setSelectedThings(thingsList[nextIndex]);
					break;
				}
				
			}
		}
		var functions = WorldThingSelector.OnThingsSelected;

		foreach(var v in functions)
		{
			v(thingsISelected);
		}


	}

}
