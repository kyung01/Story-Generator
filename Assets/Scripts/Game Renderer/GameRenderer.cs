using System.Collections;
using UnityEngine;
using StoryGenerator.World;
using System;

public class GameRenderer : MonoBehaviour
{
	[SerializeField] SpriteList SPRITE_LIST;
	[SerializeField] ThingRenderer PREFAB_THING_RENDERER;
	[SerializeField] ThingRenderer PREFAB_GRASS_RENDERER;


	[SerializeField] WallRenderer PREFAB_WALL_RENDERER;
	[SerializeField] DoorRenderer PREFAB_DOOR_RENDERER;
	[SerializeField] RoofRenderer PREFAB_ROOF_RENDERER;

	[SerializeField] TerrainMeshGenerator terrainMeshGenerator;
	[SerializeField] GameObject tempMountainRock;

	internal void hdrWorldThingAdded(Thing thing)
	{
		Render(thing);
	}

	public void RenderGame(Game game)
	{
		terrainMeshGenerator.Init(game.world.terrain);
		for(int i = 0; i < game.world.width; i++)for(int j = 0; j < game.world.height; j++)
			{
				if(game.world.terrain.GetPieceAt(i,j).Type == StoryGenerator.Terrain.Piece.KType.MOUNTAIN)
				{
					var rock = Instantiate(tempMountainRock);
					rock.transform.position = new Vector3(i, j, 0);
				}

			}
		RenderWorld(game.world);
	}

	void Render(Thing t)
	{
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
}