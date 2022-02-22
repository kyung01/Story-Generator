using System.Collections;
using UnityEngine;


public class Main : MonoBehaviour
{
	public Game game;
	public GameRenderer gameRenderer;
	// Use this for initialization
	void Start()
	{
		game.StartGame();
		gameRenderer.RenderGame(game);
	}

	// Update is called once per frame
	void Update()
	{

	}
}
