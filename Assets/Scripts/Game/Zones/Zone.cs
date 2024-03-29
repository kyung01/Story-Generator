﻿using GameEnums;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Zone
{
	#region helper class
	public class Area {
		public List<Vector2> positions = new List<Vector2>();

		public bool IsConnected(Vector2 testVec2)
		{
			for (int i = 0; i < positions.Count; i++)
			{
				var p = positions[i];
				Vector2[] neighbours = new Vector2[] { p + Vector2.up, p + Vector2.right, p + Vector2.down, p + Vector2.left };
				for(int j = 0; j < neighbours.Length; j++)
				{
					if (testVec2.IsSame_INT(neighbours[j]))
					{
						return true;

					}
				}
				
			}
			return false;
		}
	}
	#endregion
	
	public enum TYPE { DEFAULT,STOCKPILE,
		HOUSE,
		BEDROOM,
		BATHROOM,
		LIVINGROOM
	}

	public Color Color;
	internal TYPE type = TYPE.DEFAULT;
	internal List<Vector2> positions = new List<Vector2>();
	List<Thing> thingsIn = new List<Thing>();
	public List<Thing> Things { get { return new List<Thing>(thingsIn); } }

	virtual public void Update(StoryGenerator.World.World world, float timeElapsed)
	{

	}

	public virtual void AddThing(Thing thing)
	{
		this.thingsIn.Add(thing);

	}
	public virtual void AddThingInRange(List<Thing> things)
	{
		this.thingsIn.AddRange(things);
	}
	public virtual void RemoveThing(Thing thing)
	{
		this.thingsIn.Remove(thing);

	}
	public void RefreshPositions()
	{
		if (this.positions.Count == 0) return;
		List<Area> areas = new List<Area>();
		Area area00 = new Area();
		areas.Add(area00);

		var availablePositions = this.Positions;
		area00.positions.Add(availablePositions[0]);
		availablePositions.RemoveAt(0);

		hprSort(areas, area00, availablePositions);
		Area bestArea = areas[0];
		for(int i = 0; i < areas.Count; i++)
		{
			if(areas[i].positions.Count> bestArea.positions.Count)
			{
				bestArea = areas[i];
			}
		}
		this.positions = new List<Vector2>();
		foreach(var p in bestArea.positions)
		{
			this.positions.Add(p);
		}
	}
	
	#region helper

	private void hprSort(List<Area> areas, Area selectedArea, List<Vector2> availablePositions)
	{
		bool isAreaEdited = false;
		for(int i = availablePositions.Count-1; i >=0; i--)
		{
			if (selectedArea.IsConnected(availablePositions[i]))
			{
				isAreaEdited = true;
				selectedArea.positions.Add(availablePositions[i]);
				availablePositions.RemoveAt(i);
			}
		}
		if (isAreaEdited)
		{
			hprSort(areas, selectedArea, availablePositions);
		}
		else
		{
			//area is not edited
			if (availablePositions.Count == 0) return;
			//There are more positions left
			Area newArea = new Area();
			areas.Add(newArea);
			newArea.positions.Add(availablePositions[0]);
			availablePositions.RemoveAt(0);
			hprSort(areas, newArea, availablePositions);
		}
	}

	

	#endregion

	#region GetSetRemove

	public List<Vector2> Positions {
		get {
			List<Vector2> l = new List<Vector2>(this.positions);
			return l; 
		} 
	}
	
	public void AddPosition(Vector2 p)
	{
		this.positions.Add(p);
	}
	
	public virtual void RemovePosition(Vector2 v)
	{
		for(int i = positions.Count-1;i >= 0; i--)
		{
			if(positions[i] == v)
			{
				positions.RemoveAt(i);
				break;
			}
		}
		RefreshPositions();
	}

	public bool ExpandZone(int x, int y)
	{
		if (IsInZone(x, y)) return false;
		positions.Add(new Vector2(x, y));
		return true;
	}
	
	#endregion

	#region Is
	
	public bool IsDead
	{
		get { return positions.Count == 0; }
	}
	
	public bool IsHavePositions
	{
		get { return positions.Count != 0; }
	}

	public bool IsInZone(int x, int y)
	{
		foreach (var p in positions)
		{
			if ((int)p.x == x && (int)p.y == y) return true;
		}
		return false;
	}

	public bool IsInZone(Zone zone)
	{
		foreach(var p in zone.positions)
		{
			if (!IsInZone((int)p.x, (int)p.y)) return false;
		}
		return true;
	}

	public bool IsOverlapping(Zone zone)
	{
		foreach (var p in zone.positions)
		{
			if (IsInZone((int)p.x, (int)p.y)) return true;
		}
		return false;
	}
	
	#endregion


	//required once you add/remove positions so that an isolated position-area is not remained

}