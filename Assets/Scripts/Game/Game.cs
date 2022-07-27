using StoryGenerator.World;
using System.Collections;
using UnityEngine;

public partial class Game : MonoBehaviour
{


	public World world;

	public void StartGame()
	{
		world = new World();
		WorldController.Init(world);


	}
	
	// Use this for initialization
	
	void Start()
	{
		
	}
	private void FixedUpdate()
	{
		world.Update(Time.fixedDeltaTime);

	}

	// Update is called once per frame
	void Update()
	{
	}
}