using System.Collections;
using UnityEngine;

public class TerrainMeshGenerator : MonoBehaviour
{
	Mesh mesh;
	Vector3[] verticies;
	int[] triangles;

	int xSize = 10;
	int zSize = 10;

	// Start is called before the first frame update
	void Start()
	{
		mesh = new Mesh();
		GetComponent<MeshFilter>().mesh = mesh;
		CreateShape();
		UpdateMesh();

	}

	// Update is called once per frame
	void Update()
	{

	}

	void CreateShape()
	{
		verticies = new Vector3[(xSize + 1) * (zSize + 1)];
		triangles = new int[(xSize) * (zSize) * 6];

		for (
			int i=0, 
			z = 0;  z <= zSize; z++)
		{
			for(int x = 0; x <= xSize; x++)
			{
				verticies[i] = new Vector3(-.5f+x, 0, -.5f+z);
				i++;
			}
		}

		int triangleIndex = 0;
		int horizontalVertexRawCount = xSize + 1;
		for (int j = 0; j < zSize ; j++)
		{
			int zIndex = horizontalVertexRawCount * j;
			for (int i = 0; i < xSize; i++)
			{
				Debug.Log(triangleIndex + " " + xSize);
				int cornerA = zIndex + i;
				int cornerB = zIndex + i + 1;
				triangles[triangleIndex + 0] = cornerA;
				triangles[triangleIndex + 1] = cornerA + horizontalVertexRawCount;
				triangles[triangleIndex + 2] = cornerA + 1;

				triangles[triangleIndex + 3] = cornerB ;
				triangles[triangleIndex + 4] = cornerA + horizontalVertexRawCount;
				triangles[triangleIndex + 5] = cornerB + horizontalVertexRawCount;

				triangleIndex += 6;
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

		Gizmos.color = Color.red;
		for (int x = 0; x < xSize; x++)
		{
			for(int z = 0; z < zSize; z++)
			{
				Gizmos.DrawSphere(new Vector3(x,0,z), .1f);

			}
		}
		Gizmos.color = Color.white;
		for (int i = 0; i < verticies.Length; i++)
		{
			Gizmos.DrawSphere(verticies[i], .1f);
		}
	}

	
}
