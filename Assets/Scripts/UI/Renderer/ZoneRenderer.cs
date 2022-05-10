﻿using System;
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

public class ZoneRenderer : MonoBehaviour
{
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

	// Update is called once per frame
	void Update()
	{
		foreach (var p in zone.positions)
		{
			var p1 = hprToViewport(new Vector2(p.x - .5f, p.y - .5f));
			var p2 = hprToViewport(new Vector2(p.x + .5f, p.y + .5f));

			UIPostRenderer.RenderSquare(new Color(Color.r, Color.g, Color.b,0.5f), p1, p2);

		}
		foreach(var e in this.edges)
		{
			render(e);
		}

	}

	private void render(EdgeInformation e)
	{
		float lineWidth = 0.3f;
		Vector2 p = e.position;
		Vector2[] edgesLocations = new Vector2[] { p + Vector2.up, p + Vector2.right, p + Vector2.down, p + Vector2.left };
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
				UIPostRenderer.RenderSquare(this.Color, hprToViewport(edgesLine[0][0]), hprToViewport(edgesLine[0][1]));
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

	public void Init(Zone zone)
	{
		this.Color = UIPostRenderer.GetRandomColor();

		this.zone = zone;
		updateEdges(zone);
		//for each P in positions, check if it's a position with at least one empty adjacent
		//Record which edge is empty because we need that information to render edges


	}


}