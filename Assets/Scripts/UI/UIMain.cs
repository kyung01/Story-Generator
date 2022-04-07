using StoryGenerator.World;
using System.Collections.Generic;
using UnityEngine;

public class UIMain : MonoBehaviour
{
	public enum State { 
		DEFAULT,
		SELECT_ITEMS,
		CREATE_ZONE,
		SELECT_ZONE,
		DELETE_ZONE,
		END
	}

	State state = State.DEFAULT;

	public World world;
	public ZoneOrganizer zOrg;
	[SerializeField] UISelectBox UISelectBox;

	[SerializeField] UnityEngine.UI.Button bttnSelectZone;
	[SerializeField] UnityEngine.UI.Button bttnCreateZone;
	[SerializeField] UnityEngine.UI.Button bttnDeleteZone;


	List<Thing> thingsSelected = new List<Thing>();

	//List<Zone> zones = new List<Zone>();

	//Zone zoneSelected = null;

	public void Init(int widthg, int height)
	{
	}
	private void Awake()
	{
		UISelectBox = GetComponentInChildren<UISelectBox>();
		UISelectBox.OnSelected.Add(hdrUISelectBox_Selected);

		//UISelectBox.enabled = false;

		bttnCreateZone.onClick.AddListener(hdrBttnZone_Create);
		bttnDeleteZone.onClick.AddListener(hdrBttnZone_Delete);
		bttnSelectZone.onClick.AddListener(hdrBttnZone_Select);
	}

	private void hdrBttnZone_Create()
	{
		state = State.CREATE_ZONE;
	}
	private void hdrBttnZone_Select()
	{
		state = State.SELECT_ZONE;
	}
	private void hdrBttnZone_Delete()
	{
		state = State.DELETE_ZONE;
	}
	private void hdrUISelectBox_Selected(int xBegin, int yBegin, int xEnd, int yEnd)
	{
		if(state == State.CREATE_ZONE)
		{
			zOrg.BuildZone(xBegin, yBegin, xEnd, yEnd);
		}
		/*
		//Createa a new zone, if a cell of a zone is included in a zone then include this new zone to the old zone
		for(int j = yBegin; j<= yEnd; j++)
		{
			for (int i = xBegin; i <= xEnd; i++)
			{
				Debug.Log("Selected " + i + " " +j);
				if (zoneSelected == null)
					zoneSelected = addToA_AnyZone(i, j);
				else zoneSelected.ExpandZone(i, j);
			}

		}
		 * */
	}

	public void Init(World world)
	{
		this.world = world;
	}

	// Use this for initialization
	void Start()
	{

	}
	Vector3 hprToViewport(Vector2 WorldPos)
	{
		return Camera.main.WorldToViewportPoint(new Vector3(WorldPos.x, WorldPos.y, 0));
	}
	// Update is called once per frame
	void Update()
	{
		for(int i = 0; i < thingsSelected.Count; i++)
		{
			float sizeOfSquare = 0.9f / 2.0f;
			var pos = thingsSelected[i].XY;
			var indent = new Vector2(sizeOfSquare, sizeOfSquare);
			var p1 = pos - indent;
			var p2 = pos + indent;
			var squareFrom = hprToViewport(p1);
			var squareTo = hprToViewport(p2);
			UIPostRenderer.RenderSquareLines(squareFrom, squareTo);
		}
		//Debug.Log(this + " " + zOrg.zones.Count);
		for(int i =0; i < zOrg.zones.Count; i++)
		{
			var zone = zOrg.zones[i];
			var c = UIPostRenderer.GetColor(i);
			
			foreach(var p in zone.positions)
			{
				var p1 = hprToViewport(new Vector2(p.x - .5f, p.y - .5f));
				var p2 = hprToViewport(new Vector2(p.x + .5f, p.y + .5f));

				UIPostRenderer.RenderSquare(c,p1, p2);

			}

		}
	}
}