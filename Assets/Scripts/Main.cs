using System.Collections;
using UnityEngine;


public class Main : MonoBehaviour
{
	public PrefabList prefabList;
	public Game game;
	public GameRenderer gameRenderer;
	public UIMain UIMain;
//	public UIMain_2_0 UIMain2;
	public UIOrganizer uiOrganizer;

	// Use this for initialization
	void Start()
	{
		game.StartGame();
		gameRenderer.InitRender(game);
		//UIMain.Init(game.world); // Obsolete
		//UIMain2.Init(game.world);
		game.world.OnThingAdded.Add(gameRenderer.hdrWorldThingAdded);
		//UIMain.zOrg = game.zoneOrganizer;
		uiOrganizer.world = game.world;
		uiOrganizer.Init();
	}

	// Update is called once per frame
	void Update()
	{

	}
}
