using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeInformation
{
	public Vector2 position = new Vector2();
	//True means it is an edge : true means it has an edge there
	public bool up = false, right = false, down = false, left = false;

	public EdgeInformation(Vector2 p)
	{
		this.position = p;
	}
	public bool IsHasEdge()
	{
		return up || right || down || left;
	}
	public bool GetEdge(int n)
	{
		switch (n)
		{
			case 0: return up;
			case 1: return right;
			case 2: return down;
			case 3: return left;
			default:
				return false;
		}
	}
	public void SetEdge(bool isEdgeForAll)
	{
		up = right = down = left = isEdgeForAll;

	}

	public void SetEdge(int n, bool isEdge)
	{
		switch (n)
		{
			case 0:
				up = isEdge;
				break;
			case 1:
				right = isEdge;
				break;
			case 2:
				down = isEdge;
				break;
			case 3:
				left = isEdge;
				break;
			default:
				break;
		}
	}

}

public class Square {
	public Vector2 begin, end;

	public float Width { get { return 1 + end.x - begin.x; }  }
	public float Height { get { return 1 + end.y - begin.y; }  }

	public Square(Vector2 begin, Vector2 end)
	{
		this.begin = begin;
		this.end = end;
	}
	public Square GetCopy()
	{
		return new Square(begin, end);
	}
	public int GetArea()
	{
		float width = 1 + end.x - begin.x;
		float he = 1 + end.y - begin.y;
		return (int)(width*he);
	}
	public int GetPerfectSquareArea()
	{

		float width = 1 + end.x - begin.x;
		float he = 1 + end.y - begin.y;
		float s = Mathf.Min(width, he);
		return (int)(s*s);
	}

	internal bool expandToRight(bool[] checkAvaility, int width, int height)
	{
		for(int j = (int)begin.y; j <= end.y; j++)
		{
			int index = (int)end.x+1 + j * width;
			if (index >= checkAvaility.Length) return false;
			if (!checkAvaility[index]) return false;
		}
		//passed the test
		end = new Vector2(end.x +1, end.y);
		return true;
	}

	internal bool expandToUp(bool[] checkAvaility, int width, int height)
	{
		for (int i = (int)begin.x; i <= end.x; i++)
		{
			int index = i + ((int)end.y + 1) * width;;
			if (index >= checkAvaility.Length) return false;
			if (!checkAvaility[index]) return false;
		}
		//passed the test
		end = new Vector2(end.x , end.y + 1);
		return true;
	}
}

public class ZoneRenderer :MonoBehaviour
{
	[SerializeField] TMPro.TextMeshPro textMeshPro;
	List<ZoneRenderer> zoneRenderers = new List<ZoneRenderer>();
	public void AddZoneRenderer(ZoneRenderer zoneRen)
	{
		this.zoneRenderers.Add(zoneRen);
		zoneRen.enabled = false;
		zoneRen.transform.SetParent(this.transform);
	}

	Zone zone;
	List<EdgeInformation> edges = new List<EdgeInformation>();
	Color Color;
	// Use this for initialization
	void Start()
	{

	}
	
	Vector3 hprToViewport(Vector2 WorldPos)
	{
		return Camera.main.WorldToViewportPoint(new Vector3(WorldPos.x, WorldPos.y, 0));
	}
	public void Update()
	{
		Render();
	}
	// Update is called once per frame
	public void Render()
	{
		foreach (var p in zone.positions)
		{
			var p1 = hprToViewport(new Vector2(p.x - .5f, p.y - .5f));
			var p2 = hprToViewport(new Vector2(p.x + .5f, p.y + .5f));

			UIPostRenderer.RenderSquare(new Color(Color.r, Color.g, Color.b,0.1f), p1, p2);

		}
		foreach (var e in this.edges)
		{
			render(e, this.Color, 0.2f);
			render(e, Color.black, 0.1f);
		}
		foreach(var c in this.zoneRenderers)
		{
			c.Update();
		}
		return;

	}

	private void render(EdgeInformation e, Color color,float lineWidth)
	{
		//float lineWidth = 0.3f;
		float edgeIndexWidth = 0.5f;
		Vector2 p = e.position;
		Vector2[] edgesLocations = new Vector2[] { p + Vector2.up* edgeIndexWidth, p + Vector2.right * edgeIndexWidth, p + Vector2.down* edgeIndexWidth, p + Vector2.left* edgeIndexWidth };
		Vector2[][] edgesLine = new Vector2[][] {
				new Vector2[] {
					edgesLocations [0]+Vector2.left*0.5f + Vector2.down * lineWidth,
					edgesLocations [0]+Vector2.right*0.5f},
				new Vector2[] {
					edgesLocations [1]+Vector2.down*0.5f+ Vector2.left * lineWidth,
					edgesLocations [1]+Vector2.up*0.5f },
				new Vector2[] {
					edgesLocations [2]+Vector2.left*0.5f,
					edgesLocations [2]+Vector2.right*0.5f+ Vector2.up * lineWidth },
				new Vector2[] {
					edgesLocations[3] + Vector2.down * 0.5f,
					edgesLocations[3] + Vector2.up * 0.5f + Vector2.right * lineWidth} };
		for (int i = 0; i < 4; i++)
		{
			if (e.GetEdge(i))
			{
				var edgeLocation = edgesLocations[i];
				
				UIPostRenderer.RenderSquare(color, hprToViewport(edgesLine[i][0]), hprToViewport(edgesLine[i][1]));
			}
		}
	}

	void updateEdges(Zone zone)
	{
		this.edges = new List<EdgeInformation>();

		var positions = zone.Positions;
		foreach (var p in positions)
		{
			EdgeInformation edgeInfo = new EdgeInformation(p);
			edgeInfo.SetEdge(true);
			Vector2[] edgesLocation = new Vector2[] { p + Vector2.up, p + Vector2.right, p + Vector2.down, p + Vector2.left };
			foreach (var otherP in positions)
			{
				for (int i = 0; i < edgesLocation.Length; i++)
				{
					if (otherP.IsSame_INT(edgesLocation[i]))
					{
						edgeInfo.SetEdge(i, false);
					}

				}

				//is this going to compare the same element to itself? yes but I am lazy
			}
			if (edgeInfo.IsHasEdge())
			{
				this.edges.Add(edgeInfo);
			}
		}
	}

	void UpdateText(Zone zone)
	{
		Debug.Log("ZoneR UpdateText " + zone.type);
		switch (zone.type)
		{
			default:
			case Zone.TYPE.DEFAULT:
				this.textMeshPro.text = "";
				break;
			case Zone.TYPE.STOCKPILE:
				this.textMeshPro.text = "Stockpile";
				break;
			case Zone.TYPE.HOUSE:
				this.textMeshPro.text = "House";
				break;
			case Zone.TYPE.BEDROOM:
				this.textMeshPro.text = "Bedroom";
				break;
			case Zone.TYPE.LIVINGROOM:
				this.textMeshPro.text = "LivingRoom";
				break;
			case Zone.TYPE.BATHROOM:
				this.textMeshPro.text = "Bathroom";
				break;
		}
		var positions = zone.Positions;
		Vector2 min = new Vector2(float.MaxValue,float.MaxValue), max = new Vector2(float.MinValue,float.MinValue);
		foreach(var p in positions)
		{
			min = new Vector2(Mathf.Min(min.x, p.x), Mathf.Min(min.y, p.y));
			max = new Vector2(Mathf.Max(max.x, p.x), Mathf.Max(max.y, p.y));
		}
		int width = 1 + (int)max.x - (int)min.x;
		int height = 1 + (int)max.y - (int)min.y;

		bool[] checkAvaility = new bool[(int)width * (int)height];
		foreach(var p in positions)
		{
			var pp = p - min;
			checkAvaility[(int)(pp.x) + (int)(pp.y * width)] = true;
		}
		List<Square> squaresBegin = new List<Square>();
		List<Square> squaresResult = new List<Square>();
		foreach (var p in positions)
		{
			Vector2 seedPosition = p - min;
			Square s = new Square(seedPosition, seedPosition);
			squaresBegin.Add(s);
		}

		for(int i = 0; i < squaresBegin.Count; i++)
		{
			var squareUpward = squaresBegin[i].GetCopy();
			var squareRightward = squaresBegin[i].GetCopy();
			squaresResult.Add(squareUpward);
			squaresResult.Add(squareRightward);

			bool isExpanded = false;
			do
			{
				isExpanded = false;
				isExpanded = isExpanded || squareUpward.expandToUp(checkAvaility, width, height);
				isExpanded = isExpanded || squareUpward.expandToRight(checkAvaility, width, height);
				isExpanded = isExpanded || squareRightward.expandToRight(checkAvaility, width, height);
				isExpanded = isExpanded || squareRightward.expandToUp(checkAvaility, width, height);
			} while (isExpanded);

		}
		float bestArea = -1;
		Square square = null;
		foreach(var s in squaresResult)
		{
			if(s.GetArea() > bestArea)
			{
				bestArea = s.GetArea();
				square = s;
			}
		}
		this.transform.position = new Vector3(
			square.begin.x+
			min.x - 0.5f + square.Width/2.0f,
			square.begin.y +
			min.y-0.5f + square.Height / 2.0f, 0);
		this.textMeshPro.rectTransform.sizeDelta = new Vector2(square.Width*10,square.Height*10);



	}

	private void hprRecursive(
		bool[] checkAvaility, int width,int height, 
		List<Square> squares, Square initSquare)
	{
		bool isExpanded = false;
		isExpanded = isExpanded || initSquare.expandToRight(checkAvaility, width, height);
		isExpanded = isExpanded || initSquare.expandToUp(checkAvaility, width, height);
		if (isExpanded)
		{
			hprRecursive(checkAvaility, width, height, squares, initSquare);
		}
		else
		{
			for(int i = 0; i < width; i++)
			{
				for (int j = 0; j < height; j++)
				{
					if(checkAvaility[i + j * width])
					{
						var nextSquare = new Square(new Vector2(i, j), new Vector2(i, j));
						squares.Add(nextSquare);
						hprRecursive(checkAvaility, width, height, squares, nextSquare);
						return;
					}

				}
			}
		}
	}

	public ZoneRenderer Init(Zone zone, Color color)
	{
		this.Color = color;
		this.zone = zone;
		updateEdges(zone);
		UpdateText(zone);
		//for each P in positions, check if it's a position with at least one empty adjacent
		//Record which edge is empty because we need that information to render edges

		return this;
	}


}