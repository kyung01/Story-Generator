using StoryGenerator.World;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WorldThingSelector
{
	List<Thing> thingsISelected = null;
	public List<Thing> ThingsCurrentlySelected { 
		get
		{
			if (thingsISelected == null) return new List<Thing>();
			return thingsISelected;
		} 
	}
	Thing.TYPE typeOfThingSelected = Thing.TYPE.UNDEFINED;
	int countOfThingsSelected = -1;
	
	List<List<Thing>> getAllThingsInWorld(World world, int x, int y, int width, int height)
	{
		List<Thing> things = new List<Thing>();
		Dictionary<Thing.TYPE, List<Thing>> sortedThings = new Dictionary<Thing.TYPE, List<Thing>>();
		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				int pX = x + i;
				int pY = y + j;
				Debug.Log(this + " " +pX + " " + pY+ "");
				things.AddRange(world.GetThingsAt(pX, pY));
			}
		}
		foreach (var t in things)
		{
			if (!sortedThings.ContainsKey(t.type))
			{
				sortedThings.Add(t.type, new List<Thing>());
			}
			sortedThings[t.type].Add(t);
		}
		List<List<Thing>> thingsByCount = new List<List<Thing>>();
		foreach (var pair in sortedThings)
		{
			thingsByCount.Add(pair.Value);
		}
		var d = thingsByCount.OrderByDescending(x => x.Count).ToList();
		return d;
	}

	bool hprIsSame (List<Thing> a, List<Thing> b)
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
		foreach(var otherThing in things)
		{
			if (hprIsSame(otherThing, this.thingsISelected))
				return true;
		}
		return false;
	}
	void select(List<Thing> things)
	{
		this.thingsISelected = things;
		this.countOfThingsSelected = things.Count;
		this.typeOfThingSelected = things[0].type;
	}

	public void Select(World world, int x, int y, int width, int height)
	{
		var thingsList = getAllThingsInWorld(world, x, y, width, height);

		for (int i = 0; i < thingsList.Count; i++)
		{
			Debug.Log(i + " " + thingsList[i][0].type + "  " + thingsList[i].Count);

		}

		if (thingsList.Count == 0){
			Debug.Log("Case Nothing to select");
			return;
		}
		if (!hprIsThingISelectedInTheList(thingsList))
		{
			Debug.Log("Case First time selecting");
			select(thingsList[0]);
			return;

		}
		else
		{
			Debug.Log("Case SelectTheNextElement");
			for (int i = 0; i< thingsList.Count; i++)
			{
				if(hprIsSame(thingsList[i], this.thingsISelected)){
					int nextIndex = (i + 1) % thingsList.Count;
					select(thingsList[nextIndex]);
					return;
				}
				
			}
		}


	}
}
