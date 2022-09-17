using StoryGenerator.World;
using System.Collections.Generic;
using UnityEngine;
using GameEnums;
using System;

public class RenderedTerrainPieceInfo
{
	public GameObject obj;
	public Rect rect;

	public RenderedTerrainPieceInfo(GameObject obj, Vector2 bl, Vector2 tR)
	{
		this.obj = obj;
		rect = new Rect(bl.x, bl.y, tR.x - bl.x, tR.y - bl.y);
	}
}


public class WorldRenderer : MonoBehaviour
{
	static public WorldRenderer Instance;

	[SerializeField] SpriteList SPRITE_LIST;
	[SerializeField] ThingRenderer PREFAB_THING_RENDERER;
	[SerializeField] ThingRenderer PREFAB_GRASS_RENDERER;


	[SerializeField] WallRenderer PREFAB_WALL_RENDERER;
	[SerializeField] DoorRenderer PREFAB_DOOR_RENDERER;
	[SerializeField] RoofRenderer PREFAB_ROOF_RENDERER;

	[SerializeField] ZoneRenderer PREFAB_ZONE_RENDERER;

	[SerializeField] TerrainMeshGenerator PREFAB_TMG;
	//[SerializeField] TerrainMeshGenerator terrainMeshGenerator;
	[SerializeField] GameObject tempMountainRock;

	List<RenderedTerrainPieceInfo> renderedTerrainPieceInfo = new List<RenderedTerrainPieceInfo>();

	Dictionary<Zone, ZoneRenderer> dicZone_ZoneRen = new Dictionary<Zone, ZoneRenderer>();

	Vector3 worldMin = new Vector3();
	Vector3 worldMax = new Vector3();

	private void Awake()
	{

		Instance = this;
	}
	
	internal void hdrWorldThingAdded(Thing thing)
	{
		InitRender(thing);
	}

	public void SetZoneRendereEnabledTo(bool value)
	{
		foreach(var p in dicZone_ZoneRen)
		{
			p.Value.SetEnabled(value);
		}
	}

	private void InitRenderTerrain(StoryGenerator.NTerrain.TerrainSystem terrain)
	{
		//terrainMeshGenerator.Init(terrain);
		int maxSize = 100;
		int numTMGWidth = terrain.Width / maxSize;
		int numTMGHeight = terrain.Height / maxSize;
		int xBegin = 0;
		int yBegin = 0;
		//Debug.Log("numTMGWidth " + numTMGWidth + " " + numTMGHeight);

		List<StoryGenerator.NTerrain.TerrainSystem> terrainBrokenInto = new List<StoryGenerator.NTerrain.TerrainSystem>();
		for (int j = 0; j < numTMGHeight; j++)
		{
			//Debug.Log("CycleBegin j");
			xBegin = 0;
			for (int i = 0; i < numTMGWidth; i++)
			{
				//Debug.Log("CycleBegin i");
				Vector2 positionBegin = new Vector2(xBegin, yBegin);
				var newTerrain = new StoryGenerator.NTerrain.TerrainSystem();
				terrainBrokenInto.Add(newTerrain);
				//newTerrain.Init(maxSize, maxSize);
				newTerrain.Init(terrain.Width,terrain.Height);
				//newTerrain.PositionBegin = positionBegin;
				for (int x = 0; x< terrain.Width; x++)
				{
					for(int y = 0; y < terrain.Height; y++)
					{

						newTerrain.pieces[x + y * terrain.Width] = terrain.pieces[x + y * terrain.Width];
					}
				}

				for (int x = xBegin; x -xBegin <   maxSize && x < terrain.Width; x++)
				{
					for (int y = yBegin; y -yBegin <   maxSize && y < terrain.Height; y++)
					{
						//Debug.Log(x + " " + y);
						//Debug.Log("MAX"+(xBegin + maxSize) + " " + (yBegin + maxSize));
						//newTerrain.pieces[(x - xBegin) + (y - yBegin) * maxSize] =  terrain.pieces[x + y * terrain.Width];

					}
				}
				{

					var terrainMeshGenerator = Instantiate(PREFAB_TMG);
					terrainMeshGenerator.transform.position = Vector3.zero;
					terrainMeshGenerator.InitWithinRange(terrain, xBegin,yBegin,xBegin+maxSize-1, yBegin+maxSize-1);
					//terrainMeshGenerator.gameObject.SetActive(false);
					this.renderedTerrainPieceInfo.Add(new RenderedTerrainPieceInfo(terrainMeshGenerator.gameObject, positionBegin, positionBegin + new Vector2(maxSize,maxSize)));
				}
				xBegin += maxSize;

			}
			yBegin += maxSize;
		}
	}


	public void Init(World world)
	{
		InitRenderTerrain(world.terrain);

		for (int i = 0; i < world.width; i++) for (int j = 0; j < world.height; j++)
			{
				if (world.terrain.GetPieceAt(i, j).Type == StoryGenerator.NTerrain.Piece.KType.MOUNTAIN)
				{
					var rock = Instantiate(tempMountainRock);
					rock.transform.position = new Vector3(i, j, 0);
				}

			}

		InitRenderWorld(world);
		InitRenderZoneOrganizer(world.zoneOrganizer);


		world.OnThingAdded.Add(hdrWorldThingAdded);
	}

	private void InitRenderZoneOrganizer(ZoneOrganizer zoneOrganizer)
	{
		zoneOrganizer.OnZoneAdded.Add(hdrZoneAdded);
		zoneOrganizer.OnZoneEdited.Add(hdrZoneEdited);
		zoneOrganizer.OnZoneRemoved.Add(hdrZoneRemoved);
	}


	private void hdrZoneAdded(Zone zone)
	{
		var renderer = Instantiate(PREFAB_ZONE_RENDERER);
		renderer.Init(zone, UIPostRenderer.GetRandomColor());
		this.dicZone_ZoneRen.Add(zone, renderer);
	}
	private void hdrZoneEdited(Zone zone)
	{
		dicZone_ZoneRen[zone].Init(zone, zone.Color);
	}

	private void hdrZoneRemoved(Zone zone)
	{
		dicZone_ZoneRen.Remove(zone);
	}

	void InitRender(Thing t)
	{
		if (t.Category == ThingCategory.GRASS)
		{
			var thingRenderer = Instantiate(PREFAB_GRASS_RENDERER);
			thingRenderer.RenderThing(t, SPRITE_LIST);

		}
		else if (t.Category == ThingCategory.WALL)
		{

			var wallRenderer = Instantiate(PREFAB_WALL_RENDERER);
			wallRenderer.RenderThing(t);
		}
		else if (t.Category == ThingCategory.DOOR)
		{
			var doorRenderer = Instantiate(PREFAB_DOOR_RENDERER);
			doorRenderer.RenderThing(t);

		}
		else if (t.Category == ThingCategory.ROOF)
		{
			var doorRenderer = Instantiate(PREFAB_ROOF_RENDERER);
			doorRenderer.RenderThing(t);

		}
		else
		{
			var thingRenderer = Instantiate(PREFAB_THING_RENDERER);
			thingRenderer.RenderThing(t, SPRITE_LIST);

		}
	}
	
	public void InitRenderWorld(World world)
	{
		for (int i = 0; i < world.allThings.Count; i++)
		{
			var t = world.allThings[i];
			InitRender(t);

		}
	}


	public void Update()
	{
		RenderWorldController();
		//Debug.Log(this + " : "+listOfThings.Count);
		
		//Debug.Log("M " + Input.mousePosition);
		var worldMin = Camera.main.ViewportToWorldPoint(new Vector3(-.1f, -.1f, 0));
		var worldMax = Camera.main.ViewportToWorldPoint(new Vector3(1.1f, 1.1f, 0));
		bool shouldReloadEntityListAndTerrain = (this.worldMin != worldMin || this.worldMax != worldMax);
		if (shouldReloadEntityListAndTerrain)
		{
			//refresh the game
			var screenRect = new Rect(worldMin.x, worldMin.y, worldMax.x-worldMin.x ,worldMax.y-worldMin.y);
			foreach(var tPiece in this.renderedTerrainPieceInfo)
			{
				tPiece.obj.SetActive(tPiece.rect.Overlaps(screenRect));

			}
		}

	}

	private void RenderWorldController()
	{
		var listOfThings = WorldController.GetCurrentlySelectedThings();
		foreach (var entity in listOfThings)
		{
			Vector3 entityXYZ = new Vector3(entity.X, entity.Y, 0);
			Vector3 size = new Vector3(.5f, .5f, 0);
			UIPostRenderer.Render_Shape_Square(new Color(1, 1, 1, 0.3f), entityXYZ - size, entityXYZ + size);
		}

	}
}