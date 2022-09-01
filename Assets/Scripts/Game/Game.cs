using StoryGenerator.World;
using System.Collections;
using UnityEngine;

/// <summary>
/// My Game
/// </summary>
public partial class Game : MonoBehaviour
{
	static Game Instance;
	static public void SetTimeScale(float scale)
	{
		Instance.timeScale = scale;

	}
	public World world;
	float timeScale;

	public void StartGame()
	{
		world = new World();
		WorldController.Init(world);


	}
	
	// Use this for initialization
	
	void Start()
	{
		Instance = this;
	}
	private void FixedUpdate()
	{
		for(int i = 0; i < timeScale; i++)
		{
			world.Update(Time.fixedDeltaTime);

		}

	}

	// Update is called once per frame
	void Update()
	{
	}
}