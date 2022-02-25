using StoryGenerator.World;
using System.Collections;
using UnityEngine;

public class Game : MonoBehaviour
{
	public World world;

	public void StartGame(PrefabList prefabList)
	{
		world = new World();
		world.InitTerrain(prefabList);


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