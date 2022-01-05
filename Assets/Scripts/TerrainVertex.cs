using System.Collections;
using UnityEngine;

public class TerrainVertex 
{
	public Vector3 position;
	public int type;
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
}