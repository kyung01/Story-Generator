using StoryGenerator.World;
using System.Collections;
using UnityEngine;

public class Game : MonoBehaviour
{
	public World world;

	public void StartGame()
	{
		world = new World();
		world.InitTerrain();


	}
	// Use this for initialization
	void Start()
	{
		
	}


	// Update is called once per frame
	void Update()
	{

	}
}