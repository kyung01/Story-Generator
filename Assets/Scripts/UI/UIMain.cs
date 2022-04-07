﻿using StoryGenerator.World;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMain : MonoBehaviour
{
	public enum State { 
		DEFAULT,
		SELECT_ITEMS,
		BUILD_ZONE,
		END
	}

	State state = State.DEFAULT;

	public World world;
	[SerializeField] UISelectBox UISelectBox;

	[SerializeField] UnityEngine.UI.Button bttnSelectRegion;


	List<Thing> thingsSelected = new List<Thing>();

	List<Zone> zones = new List<Zone>();

	Zone zoneSelected = null;

	public void Init(int widthg, int height)
	{
	}
	private void Awake()
	{
		UISelectBox = GetComponentInChildren<UISelectBox>();
		UISelectBox.OnSelected.Add(hdrUISelectBox_Selected);

		//UISelectBox.enabled = false;

		bttnSelectRegion.onClick.AddListener(hdrBttnCreatRegion);
	}

	private void hdrBttnCreatRegion()
	{
		switch (state)
		{
			case State.SELECT_ITEMS:
			case State.BUILD_ZONE:
				state = State.BUILD_ZONE;
				zoneSelected = null;
				break;
			case State.DEFAULT:
				state = State.DEFAULT;

				break;
			case State.END:
				break;
			default:
				break;
		}
	}
	Zone addToA_AnyZone(int x, int y)
	{
		if (zones.Count == 0)
		{
			zones.Add(new Zone());
		}
		for(int i = 0; i < zones.Count; i++)
		{
			if(zones[i].CanYouAddTo(x, y))
			{
				zones[i].addZone(x, y);
				return zones[i];
			}
		}
		return null ;
	}
	private void hdrUISelectBox_Selected(int xBeing, int yBegin, int xEnd, int yEnd)
	{
		//Createa a new zone, if a cell of a zone is included in a zone then include this new zone to the old zone
		for(int j = yBegin; j<= yEnd; j++)
		{
			for (int i = xBeing; i <= xEnd; i++)
			{
				Debug.Log("Selected " + i + " " +j);
				if (zoneSelected == null)
					zoneSelected = addToA_AnyZone(i, j);
				else zoneSelected.addZone(i, j);
			}

		}
	}

	public void Init(World world)
	{
		this.world = world;
	}

	// Use this for initialization
	void Start()
	{

	}
	Vector3 hprToViewport(Vector2 WorldPos)
	{
		return Camera.main.WorldToViewportPoint(new Vector3(WorldPos.x, WorldPos.y, 0));
	}
	// Update is called once per frame
	void Update()
	{
		for(int i = 0; i < thingsSelected.Count; i++)
		{
			float sizeOfSquare = 0.9f / 2.0f;
			var pos = thingsSelected[i].XY;
			var indent = new Vector2(sizeOfSquare, sizeOfSquare);
			var p1 = pos - indent;
			var p2 = pos + indent;
			var squareFrom = hprToViewport(p1);
			var squareTo = hprToViewport(p2);
			UIPostRenderer.RenderSquareLines(squareFrom, squareTo);
		}

		for(int i =0; i < zones.Count; i++)
		{
			Debug.Log("Zone " + zones.Count);
			var zone = zones[i];
			foreach(var p in zone.positions)
			{
				var p1 = hprToViewport(new Vector2(p.x - .5f, p.y - .5f));
				var p2 = hprToViewport(new Vector2(p.x + .5f, p.y + .5f));

				UIPostRenderer.RenderSquare(p1, p2);

			}

		}
	}
}