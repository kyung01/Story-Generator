using System.Collections;
using UnityEngine;
using StoryGenerator.World;

public class GameRenderer : MonoBehaviour
{
	[SerializeField] SpriteList SPRITE_LIST;
	[SerializeField] ThingRenderer PREFAB_THING_RENDERER;
	[SerializeField] ThingRenderer PREFAB_GRASS_RENDERER;

	[SerializeField] TerrainMeshGenerator terrainMeshGenerator;
	[SerializeField] GameObject tempMountainRock;
	public void RenderGame(Game game)
	{
		terrainMeshGenerator.Init(game.world.terrain);
		for(int i = 0; i < game.world.width; i++)for(int j = 0; j < game.world.height; j++)
			{
				if(game.world.terrain.mountain[i + j * game.world.width])
				{
					var rock = Instantiate(tempMountainRock);
					rock.transform.position = new Vector3(i, j, 0);
				}

			}
		RenderWorld(game.world);
	}
	public void RenderWorld(World world)
	{
		for (int i = 0; i < world.allThings.Count; i++)
		{
			var t = world.allThings[i];
			if(t.type == Thing.TYPE.GRASS)
			{
				var thingRenderer = Instantiate(PREFAB_GRASS_RENDERER);
				thingRenderer.RenderThing(world.allThings[i], SPRITE_LIST);

			}
			else
			{
				var thingRenderer = Instantiate(PREFAB_THING_RENDERER);
				thingRenderer.RenderThing(world.allThings[i], SPRITE_LIST);

			}
		}
	}
}