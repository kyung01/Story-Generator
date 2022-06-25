using StoryGenerator.World;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIMain_2_0 : MonoBehaviour
{
	[SerializeField] ZoneRenderer PREFAB_ZONERENDERER;
	[SerializeField] UILinkerOLD uiLinker;
	private UISelectBox UISelectBox;

	World world;

	UILinkerOLD.FEEDBACK selectedMode;
	UILinkerOLD.FEEDBACK selectedEditMode;

	List<StaticZoneRenderer> zoneRenderers = new List<StaticZoneRenderer>();
	List<ZoneRenderer> zoneRendererPrefabs = new List<ZoneRenderer>();
	WorldThingSelector wrdThingSelector = new WorldThingSelector();

	// Use this for initialization
	private void Awake()
	{
		uiLinker.OnFeedback.Add(hdrUILinkerFeedback);
	}
	public void Init(World world)
	{
		this.world = world;

		UISelectBox = GetComponentInChildren<UISelectBox>();
		UISelectBox.OnSelected.Add(hdrUISelectBox_Selected);

		world.zoneOrganizer.OnSingleZoneSelected.Add(hdrSingleZoneSelected);
		world.zoneOrganizer.OnNO_ZONE_SELECTED.Add(hdrNoZoneSelected);
		world.zoneOrganizer.OnZoneAdded.Add(hdrZoneAddedRemoved);
		world.zoneOrganizer.OnZoneRemoved.Add(hdrZoneAddedRemoved);
		world.zoneOrganizer.OnZoneEdited.Add(hdrZoneAddedRemoved);
	}

	#region zoneOrganizerHdrs

	private void hdrZoneAddedRemoved(Zone zone)
	{
		updateZoneRenderers();

	}


	private void hdrNoZoneSelected()
	{

	}

	private void hdrSingleZoneSelected(Zone zone)
	{

	}

	#endregion

	//SelectBox methods
	private void hdrUISelectBox_Selected(int xBegin, int yBegin, int xEnd, int yEnd)
	{
		SelectInGame(xBegin, yBegin, xEnd, yEnd);

	}

	private void SelectInGame(int xBegin, int yBegin, int xEnd, int yEnd)
	{
		Debug.Log(this + " " + selectedMode + " " + selectedEditMode);
		if (selectedMode == UILinkerOLD.FEEDBACK.FOOTER_ZONE_SELECTED)
		{
			world.zoneOrganizer.Select(xBegin, yBegin, xEnd, yEnd);
			return;
		}
		if (selectedMode == UILinkerOLD.FEEDBACK.TASK_HAUL)
		{
			wrdThingSelector.Select(world, xBegin, yBegin, 1 + xEnd - xBegin, 1 + yEnd - yBegin);
			var things = wrdThingSelector.ThingsCurrentlySelected;
			switch (selectedEditMode)
			{
				default:
				case UILinkerOLD.FEEDBACK.ANY: //Select mode?
					break;
				case UILinkerOLD.FEEDBACK.ADD:
					foreach (var thing in things)
					{
						world.teams[0].WorkManager.Howl(thing);

					}
					break;
				case UILinkerOLD.FEEDBACK.REMOVE:
					break;

			}
		}
		if (selectedMode == UILinkerOLD.FEEDBACK.HOUSING_HOUSE_SELECTED)
		{
			switch (selectedEditMode)
			{
				default:
				case UILinkerOLD.FEEDBACK.ANY: //Select mode?
					world.zoneOrganizer.Select(xBegin, yBegin, xEnd, yEnd, Zone.TYPE.HOUSE);
					break;
				case UILinkerOLD.FEEDBACK.ADD:
					world.zoneOrganizer.BuildHouseZone(xBegin, yBegin, xEnd, yEnd);
					break;
				case UILinkerOLD.FEEDBACK.REMOVE:
					world.zoneOrganizer.DeleteZone(xBegin, yBegin, xEnd, yEnd, Zone.TYPE.HOUSE);
					break;

			}
		}
		if (selectedMode == UILinkerOLD.FEEDBACK.HOUSING_BEDROOM_SELECTED)
		{
			switch (selectedEditMode)
			{
				default:
				case UILinkerOLD.FEEDBACK.ANY: //Select mode?
					world.zoneOrganizer.Select(xBegin, yBegin, xEnd, yEnd, Zone.TYPE.HOUSE);
					break;
				case UILinkerOLD.FEEDBACK.ADD:
					world.zoneOrganizer.BuildBedroom(xBegin, yBegin, xEnd, yEnd);
					break;
				case UILinkerOLD.FEEDBACK.REMOVE:
					break;

			}
		}
		if (selectedMode == UILinkerOLD.FEEDBACK.HOUSING_BATHROOM_SELECTED)
		{
			switch (selectedEditMode)
			{
				default:
				case UILinkerOLD.FEEDBACK.ANY: //Select mode?
					world.zoneOrganizer.Select(xBegin, yBegin, xEnd, yEnd, Zone.TYPE.HOUSE);
					break;
				case UILinkerOLD.FEEDBACK.ADD:
					world.zoneOrganizer.BuildBathroom(xBegin, yBegin, xEnd, yEnd);
					break;
				case UILinkerOLD.FEEDBACK.REMOVE:
					break;

			}
		}
		if (selectedMode == UILinkerOLD.FEEDBACK.HOUSING_LIVINGROOM_SELECTED)
		{
			switch (selectedEditMode)
			{
				default:
				case UILinkerOLD.FEEDBACK.ANY: //Select mode?
					world.zoneOrganizer.Select(xBegin, yBegin, xEnd, yEnd, Zone.TYPE.HOUSE);
					break;
				case UILinkerOLD.FEEDBACK.ADD:
					world.zoneOrganizer.BuildLivingroom(xBegin, yBegin, xEnd, yEnd);
					break;
				case UILinkerOLD.FEEDBACK.REMOVE:
					break;

			}
		}
		if (selectedMode == UILinkerOLD.FEEDBACK.STOCKPILE_SELECTED)
		{
			switch (selectedEditMode)
			{
				default:
				case UILinkerOLD.FEEDBACK.ANY: //Select mode?
					world.zoneOrganizer.Select(xBegin, yBegin, xEnd, yEnd, Zone.TYPE.STOCKPILE);
					break;
				case UILinkerOLD.FEEDBACK.ADD:
					world.zoneOrganizer.BuildStockpileZone(xBegin, yBegin, xEnd, yEnd);
					break;
				case UILinkerOLD.FEEDBACK.REMOVE:
					world.zoneOrganizer.DeleteZone(xBegin, yBegin, xEnd, yEnd, Zone.TYPE.STOCKPILE);
					break;

			}
		}

		if (selectedMode == UILinkerOLD.FEEDBACK.STOCKPILE_SELECTED)
		{
			switch (selectedEditMode)
			{
				default:
				case UILinkerOLD.FEEDBACK.ANY: //Select mode?
					world.zoneOrganizer.Select(xBegin, yBegin, xEnd, yEnd, Zone.TYPE.STOCKPILE);
					break;
				case UILinkerOLD.FEEDBACK.ADD:
					world.zoneOrganizer.BuildStockpileZone(xBegin, yBegin, xEnd, yEnd);
					break;
				case UILinkerOLD.FEEDBACK.REMOVE:
					world.zoneOrganizer.DeleteZone(xBegin, yBegin, xEnd, yEnd, Zone.TYPE.STOCKPILE);
					break;

			}
		}

	}

	private void hdrUILinkerFeedback(UILinkerOLD.FEEDBACK feedback)
	{


		if (feedback != UILinkerOLD.FEEDBACK.ANY && feedback != UILinkerOLD.FEEDBACK.ADD && feedback != UILinkerOLD.FEEDBACK.REMOVE)
		{
			selectedMode = feedback;
		}
		else
		{
			selectedEditMode = feedback;

		}


		updateZoneRenderers();
	}

	private void updateZoneRenderers()
	{
		Debug.Log("updateZoneRenderers");

		UILinkerOLD.FEEDBACK[] housingZone = new UILinkerOLD.FEEDBACK[] {
			UILinkerOLD.FEEDBACK.HOUSING_SELECTED,
			UILinkerOLD.FEEDBACK.HOUSING_BATHROOM_SELECTED,
			UILinkerOLD.FEEDBACK.HOUSING_BEDROOM_SELECTED,
			UILinkerOLD.FEEDBACK.HOUSING_HOUSE_SELECTED,
			UILinkerOLD.FEEDBACK.HOUSING_LIVINGROOM_SELECTED
		};
		if (selectedMode == UILinkerOLD.FEEDBACK.FOOTER_ZONE_SELECTED)
		{
			//render all zones

			updateZoneRendererTo(EnumUtil.GetValues<Zone.TYPE>().ToArray());
		}
		else if (hprIsOneOfThese(selectedMode, housingZone))
		{
			//render all zones
			Debug.Log("Render House");

			updateZoneRendererTo(Zone.TYPE.HOUSE);
		}
		else if (selectedMode == UILinkerOLD.FEEDBACK.STOCKPILE_SELECTED)
		{
			//render all stockpiles
			updateZoneRendererTo(Zone.TYPE.STOCKPILE);
		}
		else
		{
			this.zoneRenderers = new List<StaticZoneRenderer>();
		}
	}

	private bool hprIsOneOfThese(UILinkerOLD.FEEDBACK check, UILinkerOLD.FEEDBACK[] testTo)
	{
		foreach (var t in testTo)
		{
			if (check == t) return true;
		}
		return false;
	}

	private void updateZoneRendererTo(params Zone.TYPE[] givenTypes)
	{
		foreach (var p in zoneRendererPrefabs)
		{
			Destroy(p.gameObject);
		}
		zoneRendererPrefabs = new List<ZoneRenderer>();
		Color disabledColor = new Color(1f, 1f, 1f, 0.5f);
		zoneRenderers = new List<StaticZoneRenderer>();
		foreach (var zone in world.zoneOrganizer.zones)
		{
			bool isAdded = false;
			foreach (var correctType in givenTypes)
			{
				if (zone.type == correctType)
				{
					addZoneToZoneRenderes(zone,false ,Color.white);
					isAdded = true;
					break;

				}
			}
			if (!isAdded)
			{
				addZoneToZoneRenderes(zone, true, disabledColor);
				//zoneRenderers.Add(new StaticZoneRenderer(zone, disabledColor));

			}
		}
	}

	private void addZoneToZoneRenderes(Zone zone, bool isColored, Color color)
	{
		if (zone.type == Zone.TYPE.HOUSE)
		{
			//addHouseZone((HouseZone)zone, isColored,color);

			var prefab = Instantiate(PREFAB_ZONERENDERER);
			prefab.Init(zone, color);
			this.zoneRendererPrefabs.Add(prefab);

			var hz= (BaseHousingZone)zone;
			foreach(var r in hz.Rooms)
			{
				Debug.Log("Positions "+r.positions.Count);
				var zoneRendererChild = Instantiate(PREFAB_ZONERENDERER);
				zoneRendererChild.Init(r, color);
				prefab.AddZoneRenderer(zoneRendererChild);
				//this.zoneRendererPrefabs.Add(prefab2);
			}
		}
		else
		{
			var prefab = Instantiate(PREFAB_ZONERENDERER);
			prefab.Init(zone, color);
			this.zoneRendererPrefabs.Add(prefab);
			/*
			var r = new StaticZoneRenderer(zone);
			zoneRenderers.Add(r);
			if (isColored)
			{
				r.Color = color;

			}
			 * */

		}
	}

	private void addHouseZone(BaseHousingZone zone, bool isColored, Color color)
	{
		var rooms = zone.Rooms;
		var houseZoneRenderer = new StaticZoneRenderer(zone);
		if (isColored) houseZoneRenderer.Color = color;
		foreach (var childRoom in rooms)
		{
			var childRenderer = new StaticZoneRenderer(childRoom);
			if (isColored) childRenderer.Color = color;
			houseZoneRenderer.children.Add(childRenderer);

		}
		zoneRenderers.Add(houseZoneRenderer);
	}

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
		foreach(var zone in zoneRenderers)
		{
			zone.Update();
		}
		if(selectedMode == UILinkerOLD.FEEDBACK.FOOTER_TASK_SELECTED)
		{
			//render all tasks
		}

		var thingsISelected = wrdThingSelector.ThingsCurrentlySelected;
		foreach (var thing in thingsISelected)
		{
			var p1 = hprToViewport(new Vector2(thing.X - .5f, thing.Y - .5f));
			var p2 = hprToViewport(new Vector2(thing.X + .5f, thing.Y + .5f));
			UIPostRenderer.RenderSquare(new Color(1, 1, 1, 0.3f), p1, p2);

		}

	}
}