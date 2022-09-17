using StoryGenerator.World;
using System.Collections;
using UnityEngine;

/// <summary>
/// My Game
/// </summary>
public enum Classdowkqoe { }

public partial class Game : MonoBehaviour
{
	static Game Instance;
	static public void SetTimeScale(float scale)
	{
		Instance.timeScale = scale;

	}
	public World world;
	float timeScale = 1.0f;

	public void Init()
	{
		CAIModelSheet.Init();
		world = new World();

	}
	public void StartGame()
	{

		WorldController.Init(world);


	}
	public void Load()
	{
		world.LoadTestingSceneaerio();
	}

	// Use this for initialization

	Vector2 p = new Vector2(0, 3);
	void Start()
	{
		Instance = this;
		for (int i = 0; i < 8; i++)
		{
			Debug.Log(Mathf.RoundToInt(p.x) + " " + Mathf.RoundToInt(p.y));
			p = rotate_point(p, 45);

		}
		Debug.Log(Mathf.RoundToInt(p.x) + " " + Mathf.RoundToInt(p.y));
	}
	private void FixedUpdate()
	{
		for(int i = 0; i < timeScale; i++)
		{
			world.Update(Time.fixedDeltaTime);

		}

	}
	Vector2 rotate_point(Vector2 p, float angle)
	{
		angle *= -Mathf.PI / 180.0f;
		float s = Mathf.Sin(angle);
		float c = Mathf.Cos(angle);

		// translate point back to origin:

		// rotate point
		float xnew = p.x * c - p.y * s;
		float ynew = p.x * s + p.y * c;

		// translate point back:
		p.x = xnew;
		p.y = ynew;
		return p;
	}

	// Update is called once per frame
	void Update()
	{
		
	}
}