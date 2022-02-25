﻿using System.Collections;
using UnityEngine;


public class Main : MonoBehaviour
{
	public PrefabList prefabList;
	public Game game;
	public GameRenderer gameRenderer;
	// Use this for initialization
	void Start()
	{
		game.StartGame(prefabList);
		gameRenderer.RenderGame(game);
	}

	// Update is called once per frame
	void Update()
	{

	}
}
