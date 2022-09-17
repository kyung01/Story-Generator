using System.Collections;
using UnityEngine;


public class Main : MonoBehaviour
{
	public PrefabList prefabList;
	public Game game;
	public GameRenderer gameRenderer;
//	public UIMain_2_0 UIMain2;
	public UIOrganizer uiOrganizer;

	// Use this for initialization
	void Start()
	{
		game.StartGame();
		gameRenderer.Init(game);
		uiOrganizer.Init(game.world);

		game.Load();
		//UIMain.Init(game.world); // Obsolete
		//UIMain2.Init(game.world);
		//UIMain.zOrg = game.zoneOrganizer;
	}

	// Update is called once per frame
	void Update()
	{

	}
}
