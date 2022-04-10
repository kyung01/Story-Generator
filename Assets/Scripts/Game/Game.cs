using StoryGenerator.World;
using System.Collections;
using UnityEngine;

public partial class Game : MonoBehaviour
{


	public World world;

	public void StartGame()
	{
		world = new World();
		world.InitTerrain();
		world.InitAnimals();


	}
	// Use this for initialization
	void Start()
	{
		
	}


	// Update is called once per frame
	void Update()
	{
		world.Update( Time.deltaTime);
	}
}