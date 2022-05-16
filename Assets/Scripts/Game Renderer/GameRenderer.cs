using System.Collections;
using UnityEngine;
using StoryGenerator.World;
using System;
using StoryGenerator.Terrain;
using System.Collections.Generic;

public class TerrainPieceCullingInfo
{
	public GameObject obj;
	public Rect rect;

	public TerrainPieceCullingInfo(GameObject obj, Vector2 bl, Vector2 tR)
	{
		this.obj = obj;
		rect = new Rect(bl.x, bl.y, tR.x - bl.x, tR.y - bl.y);
	}
}
public class GameRenderer : MonoBehaviour
{
	[SerializeField] SpriteList SPRITE_LIST;
	[SerializeField] ThingRenderer PREFAB_THING_RENDERER;
	[SerializeField] ThingRenderer PREFAB_GRASS_RENDERER;


	[SerializeField] WallRenderer PREFAB_WALL_RENDERER;
	[SerializeField] DoorRenderer PREFAB_DOOR_RENDERER;
	[SerializeField] RoofRenderer PREFAB_ROOF_RENDERER;

	[SerializeField] TerrainMeshGenerator PREFAB_TMG;
	//[SerializeField] TerrainMeshGenerator terrainMeshGenerator;
	[SerializeField] GameObject tempMountainRock;

	List<TerrainPieceCullingInfo> brokenTerrainPieces = new List<TerrainPieceCullingInfo>();

	internal void hdrWorldThingAdded(Thing thing)
	{
		Render(thing);
	}


	private void RenderTerrain(TerrainInstance terrain)
	{
		//terrainMeshGenerator.Init(terrain);
		int maxSize = 10;
		int numTMGWidth = terrain.Width / maxSize;
		int numTMGHeight = terrain.Height / maxSize;
		int xBegin = 0;
		int yBegin = 0;
		//Debug.Log("numTMGWidth " + numTMGWidth + " " + numTMGHeight);
		List<TerrainInstance> terrainBrokenInto = new List<TerrainInstance>();
		for (int j = 0; j < numTMGHeight; j++)
		{
			//Debug.Log("CycleBegin j");
			xBegin = 0;
			for (int i = 0; i < numTMGWidth; i++)
			{
				//Debug.Log("CycleBegin i");
				Vector2 positionBegin = new Vector2(xBegin, yBegin);
				var newTerrain = new TerrainInstance();
				terrainBrokenInto.Add(newTerrain);
				newTerrain.Init(maxSize, maxSize);
				newTerrain.PositionBegin = positionBegin;

				for (int x = xBegin; x -xBegin <   maxSize && x < terrain.Width; x++)
				{
					for (int y = yBegin; y -yBegin <   maxSize && y < terrain.Height; y++)
					{
						//Debug.Log(x + " " + y);
						//Debug.Log("MAX"+(xBegin + maxSize) + " " + (yBegin + maxSize));
						newTerrain.pieces[(x - xBegin) + (y - yBegin) * maxSize] =  terrain.pieces[x + y * terrain.Width];

					}
				}
				{

					var tmg = Instantiate(PREFAB_TMG);
					tmg.transform.position = Vector3.zero;
					tmg.Init(newTerrain);
					tmg.gameObject.SetActive(false);
					this.brokenTerrainPieces.Add(new TerrainPieceCullingInfo(tmg.gameObject, positionBegin, positionBegin + new Vector2(maxSize,maxSize)));
				}
				xBegin += maxSize;

			}
			yBegin += maxSize;
		}
	}


	public void RenderGame(Game game)
	{
		RenderTerrain(game.world.terrain);


		for (int i = 0; i < game.world.width; i++) for (int j = 0; j < game.world.height; j++)
			{
				if (game.world.terrain.GetPieceAt(i, j).Type == StoryGenerator.Terrain.Piece.KType.MOUNTAIN)
				{
					var rock = Instantiate(tempMountainRock);
					rock.transform.position = new Vector3(i, j, 0);
				}

			}
		RenderWorld(game.world);
	}

	void Render(Thing t)
	{
		return;
		if (t.type == Thing.TYPE.GRASS)
		{
			var thingRenderer = Instantiate(PREFAB_GRASS_RENDERER);
			thingRenderer.RenderThing(t, SPRITE_LIST);

		}
		else if (t.type == Thing.TYPE.WALL)
		{

			var wallRenderer = Instantiate(PREFAB_WALL_RENDERER);
			wallRenderer.RenderThing(t);
		}
		else if (t.type == Thing.TYPE.DOOR)
		{
			var doorRenderer = Instantiate(PREFAB_DOOR_RENDERER);
			doorRenderer.RenderThing(t);

		}
		else if (t.type == Thing.TYPE.ROOF)
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
	public void RenderWorld(World world)
	{
		for (int i = 0; i < world.allThings.Count; i++)
		{
			var t = world.allThings[i];
			Render(t);

		}
	}
	Vector3 worldMin = new Vector3();
	Vector3 worldMax = new Vector3();
	public void Update()
	{
		//Debug.Log("M " + Input.mousePosition);
		var worldMin = Camera.main.ViewportToWorldPoint(new Vector3(-.1f, -.1f, 0));
		var worldMax = Camera.main.ViewportToWorldPoint(new Vector3(1.1f, 1.1f, 0));
		if(this.worldMin!= worldMin || this.worldMax != worldMax)
		{
			//refresh
			var screenRect = new Rect(worldMin.x, worldMin.y, worldMax.x-worldMin.x ,worldMax.y-worldMin.y);
			foreach(var tPiece in this.brokenTerrainPieces)
			{
				tPiece.obj.SetActive(tPiece.rect.Overlaps(screenRect));

			}
		}


	}
}