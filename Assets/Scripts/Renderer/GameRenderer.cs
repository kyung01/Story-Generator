using System.Collections;
using UnityEngine;

public class GameRenderer : MonoBehaviour
{
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
	}
}