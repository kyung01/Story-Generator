﻿using System.Collections;
using UnityEngine;


public class Main : MonoBehaviour
{
	public PrefabList prefabList;
	public Game game;
	public GameRenderer gameRenderer;
	public UIMain UIMain;

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
