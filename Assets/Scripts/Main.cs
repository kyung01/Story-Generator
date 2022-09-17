using System.Collections;
using UnityEngine;


public class Main : MonoBehaviour
{
	public PrefabList prefabList;
	public Game game;
	public WorldRenderer worldRenderer;
//	public UIMain_2_0 UIMain2;
	public UIOrganizer uiOrganizer;

	// Use this for initialization
	void Start()
	{
		game.Init();
		worldRenderer.Init(game.world);
		uiOrganizer.Init(game.world);

		game.StartGame();
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
