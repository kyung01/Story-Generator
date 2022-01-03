using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainMeshGenerator : MonoBehaviour
{
	Mesh mesh;
	Vector3[] verticies;
	int[] triangles;

	public int xSize = 20;
	public int zSize = 20;

	// Start is called before the first frame update
	void Start()
	{
		mesh = new Mesh();
		GetComponent<MeshFilter>().mesh = mesh;
		CreateShape();
		UpdateMesh();

	}

	private void CreateShape()
	{
		verticies = new Vector3[(xSize + 1) * (zSize + 1)];

		for(int i=0, z = 0;  z <= zSize; z++)
		{
			for(int x = 0; x <= xSize; x++)
			{
				verticies[i] = new Vector3(x, 0, z);
				i++;
			}
		}

	}

	private void UpdateMesh()
	{
		mesh.Clear();
		mesh.vertices = verticies;
		mesh.triangles = triangles;
		mesh.RecalculateNormals();
	}
	private void OnDrawGizmos()
	{
		if( verticies == null)
		{
			return;
		}

		for(int i = 0; i < verticies.Length; i++)
		{
			Gizmos.DrawSphere(verticies[i], .1f);
		}
	}

	// Update is called once per frame
	void Update()
	{

	}
}
