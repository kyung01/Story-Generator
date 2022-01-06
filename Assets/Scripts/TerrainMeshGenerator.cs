using StoryGenerator.Terrain;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyTools;
using System.Linq;

public class TerrainMeshGenerator : MonoBehaviour
{
	Mesh mesh;
	int[] triangles;

	int xSize = 3;
	int zSize = 3;

	TerrainVertex[] vertexsCenter;
	TerrainVertex[] vertexsEdges;
	TerrainVertex[] vertexDetailed;


	StoryGenerator.Terrain.TerrainInstance sterrain;

	// Start is called before the first frame update
	void Start()
	{
		/*
		mesh = new Mesh();
		GetComponent<MeshFilter>().mesh = mesh;
		CreateShape();
		UpdateMesh();
		 * */

	}
	public void Init(StoryGenerator.Terrain.TerrainInstance instance)
	{
		sterrain = instance;
		mesh = new Mesh();
		GetComponent<MeshFilter>().mesh= mesh;


		InitVerticies(instance);

		CreateShape();
		UpdateMesh();
	}

	void InitVerticies(StoryGenerator.Terrain.TerrainInstance instance)
	{
		xSize = instance.Width;
		zSize = instance.Height;
		vertexsCenter = new TerrainVertex[xSize  * zSize ];
		vertexsEdges = new TerrainVertex[(xSize + 1) * (zSize + 1)];
		vertexDetailed = new TerrainVertex[(1+2*xSize ) * (1+2*zSize )];
		int detailedRowSize = 1 + 2 * xSize;

		#region Initiate center
		for (int i = 0,
			z = 0; z < zSize; z++) for (int x = 0; x < xSize; x++)
			{
				vertexsCenter[i] = new TerrainVertex();
				vertexsCenter[i].position = new Vector3(x, 0, z);
				vertexsCenter[i].type = (int)instance.pieces[xSize * z + x].Type;
				i++;
			}
		#endregion

		#region Initiate vertex positions
		{
			//detailed vertex initialization 
			int i = 0;
			for (float z = 0; z <= zSize; z+= 0.5f)
			{
				for (float x = 0; x <= xSize; x+= 0.5f )
				{
					vertexDetailed[i] = new TerrainVertex();
					vertexDetailed[i].position = new Vector3(-.5f + x, 0, -.5f + z);
					i++;
				}
			}
			Debug.Log(i);
			for (int z = 0; z < zSize; z++) for (int x = 0; x < xSize; x++)
				{
					vertexDetailed[(1+2*x) + (1 + 2 * xSize) *(1+z*2)].type = (int)instance.pieces[xSize * z + x].Type;
				}
		}
		Debug.Log("detailed legnth " + vertexDetailed.Length);

		for (
		int i = 0,
		z = 0; z <= zSize; z++)
		{
			for (int x = 0; x <= xSize; x++)
			{
				vertexsEdges[i] = new TerrainVertex();
				vertexsEdges[i].position = new Vector3(-.5f + x, 0, -.5f + z);
				i++;
			}
		}
		#endregion

		#region Initiate vertex types
		for (int z = 0; z < zSize; z++)
		{
			for (int x = 0; x < xSize; x++)
			{
			}
		}

		for (int z = 0; z < zSize; z++)
		{
			for (int x = 0; x < xSize; x++)
			{
				Vector2[] adjTiles = new Vector2[] {
					new Vector2(x,z+1), 
					new Vector2(x+1,z),
					new Vector2(x,z-1), 
					new Vector2(x-1,z) 
				};
				int centerX = 1 + x * 2;
				int centerZ = 1 + z * 2;
				Vector2[][] adjEdges = new Vector2[][]
				{
					new Vector2[]{ new Vector2(centerX - 1, centerZ + 1),new Vector2(centerX + 0, centerZ + 1),new Vector2(centerX + 1, centerZ + 1) },
					new Vector2[]{new Vector2(centerX+1,centerZ+0),new Vector2(centerX + 1, centerZ + 1),new Vector2(centerX + 1, centerZ -1) },
					new Vector2[]{ new Vector2(centerX -1 , centerZ - 1),new Vector2(centerX+0, centerZ - 1),new Vector2(centerX + 1, centerZ -1) },
					new Vector2[]{new Vector2(centerX - 1, centerZ + 0), new Vector2(centerX - 1, centerZ + 1), new Vector2(centerX - 1, centerZ - 1) }
				};
				var tile = instance.pieces[instance.Width * z + x];

				for(int k = 0; k < 4; k++)
				{
					int xRowSize = 1 + 2 * xSize;
					int zColumnSize = 1 + 2 * zSize;
					var adjTileIndex = adjTiles[k];
					var adjEdgeIndexs = adjEdges[k].OfType<Vector2>().ToList(); 
					
					bool isAdjacentTileLegit = EzT.ChkWithinBoundaries(adjTileIndex, new Vector2(0, 0), new Vector2(xSize - 1, zSize - 1));
					
					var tileAdj = (isAdjacentTileLegit)?
						instance.pieces[instance.Width * (int)adjTileIndex.y + (int)adjTileIndex.x] : null;

					for (int i = adjEdgeIndexs.Count-1; i >= 0; i--)
					{
						var edgeIndex = adjEdgeIndexs[i];
						bool isAdjacentEdgeLegit = EzT.ChkWithinBoundaries(edgeIndex, new Vector2(0, 0), new Vector2(xRowSize-1, zColumnSize-1));
						if (!isAdjacentEdgeLegit)
						{
							adjEdgeIndexs.RemoveAt(i);
						}
					}
					//get vertices
					List<TerrainVertex> edges = adjEdgeIndexs.Select(v => this.vertexDetailed[(int)v.x + (int) ((v.y) * (1+2*xSize ))]).ToList();

					hprUpdateInfluencedVertexs(tile, tileAdj, edges);

				}
			}
		}
		/*
		 * for (int z = 0; z < zSize; z++)
		{
			for (int x = 0; x < xSize; x++)
			{
				Vector2[] adjTiles = new Vector2[] {
					new Vector2(x,z+1), 
					new Vector2(x+1,z),
					new Vector2(x,z-1), 
					new Vector2(x-1,z) 
				};
				Vector2[][] adjEdges = new Vector2[][]
				{
					new Vector2[]{new Vector2(x+0,z+1),new Vector2(x+1, z + 1) },
					new Vector2[]{new Vector2(x+1,z+0),new Vector2(x+1, z + 1) },
					new Vector2[]{new Vector2(x+0,z+0),new Vector2(x+1, z + 0) },
					new Vector2[]{new Vector2(x+0,z+0),new Vector2(x+0, z + 1) }
				};
				var tile = instance.pieces[instance.Width * z + x];

				for(int k = 0; k < 4; k++)
				{
					var adjTileIndex = adjTiles[k];
					var adjEdgeIndexs = adjEdges[k].OfType<Vector2>().ToList(); 
					
					bool isAdjacentTileLegit = EzT.ChkWithinBoundaries(adjTileIndex, new Vector2(0, 0), new Vector2(xSize - 1, zSize - 1));
					
					var tileAdj = (isAdjacentTileLegit)?
						instance.pieces[instance.Width * (int)adjTileIndex.y + (int)adjTileIndex.x] : null;

					for (int i = adjEdgeIndexs.Count-1; i >= 0; i--)
					{
						var edgeIndex = adjEdgeIndexs[i];
						bool isAdjacentEdgeLegit = EzT.ChkWithinBoundaries(edgeIndex, new Vector2(0, 0), new Vector2(xSize, zSize ));
						if (!isAdjacentEdgeLegit)
						{
							adjEdgeIndexs.RemoveAt(i);
						}
					}
					//get vertices
					List<TerrainVertex> edges = adjEdgeIndexs.Select(v => this.vertexsEdges[(int)v.x + (int)v.y * (xSize + 1)]).ToList();

					hprUpdateInfluencedVertexs(tile, tileAdj, edges);

				}
			}
		}
		 * */
		#endregion


	}
	void hprUpdateInfluencedVertexs(
		Piece piece, Piece pieceAdjacent, 
		List<TerrainVertex> vertexInfluenced)
	{
		int selectedType;
		if (pieceAdjacent == null)
		{
			selectedType = (int)piece.Type;
		}
		else
		{
			selectedType = 
				((int)piece.Type >= (int)pieceAdjacent.Type) ? 
				(int)piece.Type : (int)pieceAdjacent.Type;
		}
		for (int i = 0; i < vertexInfluenced.Count; i++)
		{
			if(vertexInfluenced[i].type > selectedType)
			{
				continue;
			}

			Debug.Log(vertexInfluenced[i].position.x + " " + vertexInfluenced[i].position.z + " COLOR = " + selectedType);
			vertexInfluenced[i].type = selectedType;
		}

	}


	// Update is called once per frame
	void Update()
	{

	}

	void CreateShape()
	{
		//verticies = new Vector3[(xSize + 1) * (zSize + 1)];
		triangles = new int[(xSize) * (zSize) * 8*3]; // I need 8 triangles for each square



		int totalVerticesUsedToExpressSquare = 8 * 3;
		int triangleIndex = 0;
		int horizontalVertexRawCount = 1+2*xSize;

		for (int z = 0; z < zSize ; z++)
		{
			int zIndex = horizontalVertexRawCount * z;
			for (int x = 0; x < xSize; x++)
			{
				int centerIndex = x + xSize * z;

				int center = 1+2*x + horizontalVertexRawCount*(2*z+1);
				//int cornerB = cornerStartIndex+zIndex + x + 1;

				triangles[triangleIndex + 0] = center - horizontalVertexRawCount - 1;
				triangles[triangleIndex + 1] = center;
				triangles[triangleIndex + 2] = triangles[triangleIndex + 0]+1;



				//triangles[triangleIndex + 3] = cornerB ;
				//triangles[triangleIndex + 4] = cornerA + horizontalVertexRawCount;
				//triangles[triangleIndex + 5] = cornerB + horizontalVertexRawCount;

				triangleIndex += totalVerticesUsedToExpressSquare;
			}
		}

	}

	Color hprIntToColor(int n)
	{
		if (n == 0) return new Color(1,0,0);
		if (n == 1) return new Color(0,1, 0);
		if (n == 2) return new Color(0, 0, 1);
		return Color.black;
	}
	private void UpdateMesh()
	{
		mesh.Clear();

		var positionsCenter = vertexsCenter.Select(s => s.position).ToArray();
		var positionsEdges = vertexsEdges.Select(s => s.position).ToArray();
		var positionsDetailedVertexs = vertexDetailed.Select(s => s.position).ToArray();

		var colorCenter = vertexsCenter.Select(s => hprIntToColor(s.type)).ToArray();
		var colorsEdges = vertexsEdges.Select(s => hprIntToColor(s.type)).ToArray();
		var colorsDetailed= vertexDetailed.Select(s => hprIntToColor(s.type)).ToArray();

		var meshVertices	= positionsCenter.Concat(positionsEdges).ToArray();
		var meshColors = colorCenter.Concat(colorsEdges).ToArray();

		mesh.vertices = positionsDetailedVertexs;
		mesh.colors = colorsDetailed;

		mesh.triangles = triangles;
		mesh.RecalculateNormals();
	}

	private void OnDrawGizmos()
	{
		
		if (sterrain == null) return;

		for (int x = 0; x < xSize; x++)
		{
			for(int z = 0; z < zSize; z++)
			{
				Gizmos.color = Color.white;
				if ((int)sterrain.pieces[x + z * (xSize )].Type == 0)
				{
					Gizmos.color = Color.red;

				}
				if ((int)sterrain.pieces[x + z * (xSize)].Type == 1)
				{
					Gizmos.color = Color.green;

				}
				if ((int)sterrain.pieces[x + z * (xSize )].Type == 2)
				{
					Gizmos.color = Color.blue;

				}
				Gizmos.DrawSphere(new Vector3(x,0,z), .1f);

			}
		}
		return;
	}

	
}
