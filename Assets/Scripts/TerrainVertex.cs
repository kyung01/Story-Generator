using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainVertex 
{
	public Vector3 position;
	public int type;
	public List<int> typeRendered = new List<int>();
	public float renderWeight;

	public TerrainVertex()
	{
		position = Vector3.zero;
		type = -1;
	}
	public TerrainVertex(Vector3 position, int type)
	{
		this.position = position;
		this.type = type; 
	}

	public void addInfluencedType(int type)
	{
		if (typeRendered.Contains(type)) return;
		typeRendered.Add(type);
	}
}