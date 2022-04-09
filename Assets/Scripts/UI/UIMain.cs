using StoryGenerator.World;
using System;
using System.Collections.Generic;
using UnityEngine;

public class UIMain : MonoBehaviour
{
	public enum State { 
		DEFAULT,
		SELECT_THINGS,
		CREATE_ZONE,
		SELECT_ZONE,
		DELETE_ZONE,
		HOWL,
		END
	}

	State state = State.DEFAULT;

	public World world;
	public ZoneOrganizer zOrg;
	[SerializeField] UISelectBox UISelectBox;

	[SerializeField] UnityEngine.UI.Button bttnSelectThings;
	[SerializeField] UnityEngine.UI.Button bttnHowl;
	[SerializeField] UnityEngine.UI.Button bttnSelectZone;
	[SerializeField] UnityEngine.UI.Button bttnCreateZone;
	[SerializeField] UnityEngine.UI.Button bttnDeleteZone;


	List<Thing> thingsSelected = new List<Thing>();
	InWorldUIRenderer inWorldUIRendere = new InWorldUIRenderer();
	WorldThingSelector wrdThingSelector = new WorldThingSelector();
	//List<Zone> zones = new List<Zone>();

	//Zone zoneSelected = null;

	private void Awake()
	{
		UISelectBox = GetComponentInChildren<UISelectBox>();
		UISelectBox.OnSelected.Add(hdrUISelectBox_Selected);

		//UISelectBox.enabled = false;

		bttnSelectThings.onClick.AddListener(hdrSelectThings);
		bttnHowl.onClick.AddListener(hdrBttnHowl);
		bttnCreateZone.onClick.AddListener(hdrBttnZone_Create);
		bttnDeleteZone.onClick.AddListener(hdrBttnZone_Delete);
		bttnSelectZone.onClick.AddListener(hdrBttnZone_Select);
	}

	private void hdrSelectThings()
	{
		state = State.SELECT_THINGS;
	}

	private void hdrBttnHowl()
	{
		state = State.HOWL;
	}

	private void hdrBttnZone_Create()
	{
		state = State.CREATE_ZONE;
	}
	private void hdrBttnZone_Select()
	{
		state = State.SELECT_ZONE;
	}
	private void hdrBttnZone_Delete()
	{
		state = State.DELETE_ZONE;
	}
	private void hdrUISelectBox_Selected(int xBegin, int yBegin, int xEnd, int yEnd)
	{
		if (state == State.CREATE_ZONE)
		{
			zOrg.BuildZone(xBegin, yBegin, xEnd, yEnd);
		}
		if (state == State.DELETE_ZONE)
		{
			zOrg.DeleteZone(xBegin, yBegin, xEnd, yEnd);
		}
		if(state == State.SELECT_THINGS)
		{
			wrdThingSelector.Select(world, xBegin, yBegin, 1 + xEnd - xBegin, 1 + yEnd - yBegin);
		}
		/*
		//Createa a new zone, if a cell of a zone is included in a zone then include this new zone to the old zone
		for(int j = yBegin; j<= yEnd; j++)
		{
			for (int i = xBegin; i <= xEnd; i++)
			{
				Debug.Log("Selected " + i + " " +j);
				if (zoneSelected == null)
					zoneSelected = addToA_AnyZone(i, j);
				else zoneSelected.ExpandZone(i, j);
			}

		}
		 * */
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
		//Debug.Log(this + " " + zOrg.zones.Count);
		for(int i =0; i < zOrg.zones.Count; i++)
		{
			var zone = zOrg.zones[i];
			var c = UIPostRenderer.GetColor(i);
			
			foreach(var p in zone.positions)
			{
				var p1 = hprToViewport(new Vector2(p.x - .5f, p.y - .5f));
				var p2 = hprToViewport(new Vector2(p.x + .5f, p.y + .5f));

				UIPostRenderer.RenderSquare(c,p1, p2);

			}

		}
		var thingsISelected = wrdThingSelector.ThingsCurrentlySelected;
		if(thingsISelected.Count!= 0)
		{
			//Debug.Log("things Selected " + thingsISelected.Count);

		}
		foreach (var thing in thingsISelected)
		{
			var p1 = hprToViewport(new Vector2(thing.X - .5f, thing.Y - .5f));
			var p2 = hprToViewport(new Vector2(thing.X + .5f, thing.Y + .5f));
			UIPostRenderer.RenderSquare(new Color(1,1,1,0.3f), p1, p2);

		}
	}
}