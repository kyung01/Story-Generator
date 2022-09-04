using GameEnums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainVertex 
{
	public Vector3 position;
	public int type;
	public List<int> typeRendered = new List<int>();
	public List<int> typeAdjacent = new List<int>();
	/// <summary>
	/// n-th elemnt stores type n's power
	/// </summary>
	public List<float> influenceOfEachType = new List<float>();
	public float renderWeight;

	public TerrainVertex()
	{
		for (int i = 0; i < 50; i++)
		{
			influenceOfEachType.Add(0);
		}
		position = Vector3.zero;
		type = -1;
	}
	public TerrainVertex(Vector3 position, int type)
	{
		for(int i = 0; i < 50; i++)
		{
			influenceOfEachType.Add(0);
		}
		this.position = position;
		this.type = type; 
	}

	public void addInfluencedType(int type)
	{
		if (typeRendered.Contains(type)) return;
		typeRendered.Add(type);
	}
}