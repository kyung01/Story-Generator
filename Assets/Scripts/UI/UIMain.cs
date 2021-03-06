using StoryGenerator.World;
using System;
using System.Collections.Generic;
using UnityEngine;

public class UIMain : MonoBehaviour
{
	public enum State
	{
		DEFAULT,
		SELECT_THINGS,
		CREATE_ZONE,
		SELECT_ZONE,
		DELETE_ZONE,

		ORDER_HOWL,

		BUILD,

		END
	}

	State state = State.DEFAULT;
	Thing.TYPE thingToBuild = Thing.TYPE.WALL;

	World world;
	//ZoneOrganizer zOrg;
	[SerializeField] UISelectBox UISelectBox;
	List<ZoneRenderer> zoneRenderers = new List<ZoneRenderer>();

	[SerializeField] UnityEngine.UI.Button bttnSelectThings;
	[SerializeField] UnityEngine.UI.Button bttnHowl;
	[SerializeField] UnityEngine.UI.Button bttnSelectZone;
	[SerializeField] UnityEngine.UI.Button bttnCreateZone;
	[SerializeField] UnityEngine.UI.Button bttnDeleteZone;


	[SerializeField] UnityEngine.UI.Button bttnBuild;
	[SerializeField] UnityEngine.UI.Button bttnBuild_SelectWall;
	[SerializeField] UnityEngine.UI.Button bttnBuild_SelectDoor;
	[SerializeField] UnityEngine.UI.Button bttnBuild_SelectRoof;


	[SerializeField] UIZoneSetting uiZoneSetting;


	List<Thing> thingsSelected = new List<Thing>();
	WorldThingSelector wrdThingSelector = new WorldThingSelector();
	//List<Zone> zones = new List<Zone>();

	//Zone zoneSelected = null;

	private void Awake()
	{

	}

	#region hprFunctions
	#endregion

	#region hdr

	private void hdrNoZoneSelected()
	{
		uiZoneSetting.enabled = false;
		uiZoneSetting.Unlink();
	}

	private void hdrSingleZoneSelected(Zone zone)
	{
		uiZoneSetting.enabled = true;
		uiZoneSetting.Link(zone);
	}

	private void hdrSelectThings()
	{
		state = State.SELECT_THINGS;
	}

	private void hdrBttnHaul()
	{
		state = State.ORDER_HOWL;
		var things = wrdThingSelector.ThingsCurrentlySelected;
		foreach (var thing in things)
		{
			world.teams[0].WorkManager.Howl(thing);

		}
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
			world.zoneOrganizer.BuildStockpileZone(xBegin, yBegin, xEnd, yEnd);
		}
		if (state == State.DELETE_ZONE)
		{
			world.zoneOrganizer.DeleteZone(xBegin, yBegin, xEnd, yEnd);
		}
		if (state == State.SELECT_THINGS)
		{
			wrdThingSelector.Select(world, xBegin, yBegin, 1 + xEnd - xBegin, 1 + yEnd - yBegin);
		}
		if (state == State.SELECT_ZONE)
		{
			world.zoneOrganizer.Select(xBegin, yBegin, xEnd, yEnd);
		}


		if (state == State.BUILD)
		{
			for (int x = xBegin; x <= xEnd; x++)
			{
				for (int y = yBegin; y <= yEnd; y++)
				{

					world.Build(this.thingToBuild, x, y);
				}
			}
			//world.zoneOrganizer.Select(xBegin, yBegin, xEnd, yEnd);
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


	#endregion

	public void Init(World world)
	{
		this.world = world;
		//this.zOrg = zOrg;

		UISelectBox = GetComponentInChildren<UISelectBox>();
		UISelectBox.OnSelectedEnd.Add(hdrUISelectBox_Selected);

		//UISelectBox.enabled = false;

		bttnSelectThings.onClick.AddListener(hdrSelectThings);
		bttnHowl.onClick.AddListener(hdrBttnHaul);

		bttnCreateZone.onClick.AddListener(hdrBttnZone_Create);
		bttnDeleteZone.onClick.AddListener(hdrBttnZone_Delete);
		bttnSelectZone.onClick.AddListener(hdrBttnZone_Select);

		bttnBuild.onClick.AddListener(hdrBttnBuild);
		bttnBuild_SelectWall.onClick.AddListener(hdrBttnBuild_SelectWall);
		bttnBuild_SelectDoor.onClick.AddListener(hdrBttnBuild_SelectDoor);
		bttnBuild_SelectRoof.onClick.AddListener(hdrBttnBuild_SelectRoof);


		world.zoneOrganizer.OnSingleZoneSelected.Add(hdrSingleZoneSelected);
		world.zoneOrganizer.OnNO_ZONE_SELECTED.Add(hdrNoZoneSelected);
		world.zoneOrganizer.OnZoneAdded.Add(hdrUpdateZone);



	}

	private void hdrUpdateZone(Zone zone)
	{
		zoneRenderers.Add(new ZoneRenderer().Init(zone, Color.red));
	}

	private void hdrBttnBuild()
	{
		this.state = State.BUILD;
	}

	private void hdrBttnBuild_SelectRoof()
	{
		this.state = State.BUILD;
		thingToBuild = Thing.TYPE.ROOF;
	}

	private void hdrBttnBuild_SelectWall()
	{
		this.state = State.BUILD;
		thingToBuild = Thing.TYPE.WALL;
	}

	private void hdrBttnBuild_SelectDoor()
	{
		this.state = State.BUILD;
		thingToBuild = Thing.TYPE.DOOR;
	}





	// Use this for initialization
	void Start()
	{

	}
	Vector3 hprToViewport(Vector2 WorldPos)
	{
		return Camera.main.WorldToViewportPoint(new Vector3(WorldPos.x, WorldPos.y, 0));
	}
	void UpdateZoneRenderers()
	{
		foreach (var zoneR in zoneRenderers)
		{
			zoneR.Update();
		}
		return;
		for (int i = 0; i < world.zoneOrganizer.zonesSelected.Count; i++)
		{
			var zone = world.zoneOrganizer.zonesSelected[i];
			var c = UIPostRenderer.GetColor(i);
			c = new Color(1, 1, 1, 0.8f);

			foreach (var p in zone.positions)
			{
				var p1 = hprToViewport(new Vector2(p.x - .5f, p.y - .5f));
				var p2 = hprToViewport(new Vector2(p.x + .5f, p.y + .5f));

				UIPostRenderer.RenderSquareViewportSpace(c, p1, p2);

			}

		}

		for (int i = 0; i < world.zoneOrganizer.zones.Count; i++)
		{
			var zone = world.zoneOrganizer.zones[i];
			var c = UIPostRenderer.GetColor(i);
			c = new Color(c.r, c.g, c.b, 0.25f);

			foreach (var p in zone.positions)
			{
				var p1 = hprToViewport(new Vector2(p.x - .5f, p.y - .5f));
				var p2 = hprToViewport(new Vector2(p.x + .5f, p.y + .5f));

				UIPostRenderer.RenderSquareViewportSpace(c, p1, p2);

			}

		}

	}
	// Update is called once per frame
	void Update()
	{
		UpdateZoneRenderers();


		for (int i = 0; i < thingsSelected.Count; i++)
		{
			float sizeOfSquare = 0.9f / 2.0f;
			var pos = thingsSelected[i].XY;
			var indent = new Vector2(sizeOfSquare, sizeOfSquare);
			var p1 = pos - indent;
			var p2 = pos + indent;
			var squareFrom = hprToViewport(p1);
			var squareTo = hprToViewport(p2);
			UIPostRenderer.RenderSquareLinesViewportSpace(squareFrom, squareTo);
		}
		//Debug.Log(this + " " + zOrg.zones.Count);

		//Debug.Log(zOrg.zonesSelected.Count);

		var thingsISelected = wrdThingSelector.ThingsCurrentlySelected;
		if (thingsISelected.Count != 0)
		{
			//Debug.Log("things Selected " + thingsISelected.Count);

		}
		foreach (var thing in thingsISelected)
		{
			var p1 = hprToViewport(new Vector2(thing.X - .5f, thing.Y - .5f));
			var p2 = hprToViewport(new Vector2(thing.X + .5f, thing.Y + .5f));
			UIPostRenderer.RenderSquareViewportSpace(new Color(1, 1, 1, 0.3f), p1, p2);

		}
	}
}