using StoryGenerator.World;
using System.Collections;
using UnityEngine;

public class Game : MonoBehaviour
{
	World world;
	[SerializeField] TerrainMeshGenerator terrainMeshGenerator;

	// Use this for initialization
	void Start()
	{
		world = new World();
		world.InitTerrain();

		terrainMeshGenerator.Init(world.terrain);
	}


	// Update is called once per frame
	void Update()
	{

	}
}