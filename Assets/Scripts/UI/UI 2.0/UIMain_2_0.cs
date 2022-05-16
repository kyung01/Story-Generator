using StoryGenerator.World;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIMain_2_0 : MonoBehaviour
{
	[SerializeField] ZoneRenderer PREFAB_ZONERENDERER;
	[SerializeField] UILinker uiLinker;
	private UISelectBox UISelectBox;

	World world;

	UILinker.FEEDBACK selectedMode;
	UILinker.FEEDBACK selectedEditMode;

	List<StaticZoneRenderer> zoneRenderers = new List<StaticZoneRenderer>();
	List<ZoneRenderer> zoneRendererPrefabs = new List<ZoneRenderer>();

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
		world.zoneOrganizer.OnZoneAdded.Add(hdrZoneAdded);
	}

	#region zoneOrganizerHdrs

	private void hdrZoneAdded(Zone zone)
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
		if (selectedMode == UILinker.FEEDBACK.FOOTER_ZONE_SELECTED)
		{
			world.zoneOrganizer.Select(xBegin, yBegin, xEnd, yEnd);
			return;
		}
		if (selectedMode == UILinker.FEEDBACK.HOUSING_HOUSE_SELECTED)
		{
			switch (selectedEditMode)
			{
				default:
				case UILinker.FEEDBACK.ANY: //Select mode?
					world.zoneOrganizer.Select(xBegin, yBegin, xEnd, yEnd, Zone.TYPE.HOUSE);
					break;
				case UILinker.FEEDBACK.ADD:
					world.zoneOrganizer.BuildHouseZone(xBegin, yBegin, xEnd, yEnd);
					break;
				case UILinker.FEEDBACK.REMOVE:
					break;

			}
		}
		if (selectedMode == UILinker.FEEDBACK.HOUSING_BEDROOM_SELECTED)
		{
			switch (selectedEditMode)
			{
				default:
				case UILinker.FEEDBACK.ANY: //Select mode?
					world.zoneOrganizer.Select(xBegin, yBegin, xEnd, yEnd, Zone.TYPE.HOUSE);
					break;
				case UILinker.FEEDBACK.ADD:
					world.zoneOrganizer.BuildBedroom(xBegin, yBegin, xEnd, yEnd);
					break;
				case UILinker.FEEDBACK.REMOVE:
					break;

			}
		}
		if (selectedMode == UILinker.FEEDBACK.HOUSING_BATHROOM_SELECTED)
		{
			switch (selectedEditMode)
			{
				default:
				case UILinker.FEEDBACK.ANY: //Select mode?
					world.zoneOrganizer.Select(xBegin, yBegin, xEnd, yEnd, Zone.TYPE.HOUSE);
					break;
				case UILinker.FEEDBACK.ADD:
					world.zoneOrganizer.BuildBathroom(xBegin, yBegin, xEnd, yEnd);
					break;
				case UILinker.FEEDBACK.REMOVE:
					break;

			}
		}
		if (selectedMode == UILinker.FEEDBACK.HOUSING_LIVINGROOM_SELECTED)
		{
			switch (selectedEditMode)
			{
				default:
				case UILinker.FEEDBACK.ANY: //Select mode?
					world.zoneOrganizer.Select(xBegin, yBegin, xEnd, yEnd, Zone.TYPE.HOUSE);
					break;
				case UILinker.FEEDBACK.ADD:
					world.zoneOrganizer.BuildLivingroom(xBegin, yBegin, xEnd, yEnd);
					break;
				case UILinker.FEEDBACK.REMOVE:
					break;

			}
		}
		if (selectedMode == UILinker.FEEDBACK.STOCKPILE_SELECTED)
		{
			switch (selectedEditMode)
			{
				default:
				case UILinker.FEEDBACK.ANY: //Select mode?
					world.zoneOrganizer.Select(xBegin, yBegin, xEnd, yEnd, Zone.TYPE.STOCKPILE);
					break;
				case UILinker.FEEDBACK.ADD:
					world.zoneOrganizer.BuildStockpileZone(xBegin, yBegin, xEnd, yEnd);
					break;
				case UILinker.FEEDBACK.REMOVE:
					break;

			}
		}

	}

	private void hdrUILinkerFeedback(UILinker.FEEDBACK feedback)
	{


		if (feedback != UILinker.FEEDBACK.ANY && feedback != UILinker.FEEDBACK.ADD && feedback != UILinker.FEEDBACK.REMOVE)
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

		UILinker.FEEDBACK[] housingZone = new UILinker.FEEDBACK[] {
			UILinker.FEEDBACK.HOUSING_SELECTED,
			UILinker.FEEDBACK.HOUSING_BATHROOM_SELECTED,
			UILinker.FEEDBACK.HOUSING_BEDROOM_SELECTED,
			UILinker.FEEDBACK.HOUSING_HOUSE_SELECTED,
			UILinker.FEEDBACK.HOUSING_LIVINGROOM_SELECTED
		};
		if (selectedMode == UILinker.FEEDBACK.FOOTER_ZONE_SELECTED)
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
		else if (selectedMode == UILinker.FEEDBACK.STOCKPILE_SELECTED)
		{
			//render all stockpiles
			updateZoneRendererTo(Zone.TYPE.STOCKPILE);
		}
		else
		{
			this.zoneRenderers = new List<StaticZoneRenderer>();
		}
	}

	private bool hprIsOneOfThese(UILinker.FEEDBACK check, UILinker.FEEDBACK[] testTo)
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

			var hz= (HouseZone)zone;
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

	private void addHouseZone(HouseZone zone, bool isColored, Color color)
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
		if(selectedMode == UILinker.FEEDBACK.FOOTER_TASK_SELECTED)
		{
			//render all tasks
		}

	}
}