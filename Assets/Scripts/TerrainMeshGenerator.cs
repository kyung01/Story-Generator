using StoryGenerator.Terrain;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyTools;
using System.Linq;
using UnityEngine.Rendering;

public class TerrainMeshGenerator : MonoBehaviour
{
	Mesh mesh;
	int[] triangles;

	int xSize = 3;
	int zSize = 3;

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
		GetComponent<MeshFilter>().mesh = mesh;


		InitVerticies(instance);

		CreateShape();
		UpdateMesh();
	}

	void InitVerticies(StoryGenerator.Terrain.TerrainInstance instance)
	{
		xSize = instance.Width;
		zSize = instance.Height;
		vertexDetailed = new TerrainVertex[(1 + 2 * xSize) * (1 + 2 * zSize)];
		int detailedRowSize = 1 + 2 * xSize;

		#region Initiate center

		#endregion

		#region Initiate vertex positions
		{
			//detailed vertex initialization 
			int i = 0;
			for (float z = 0; z <= zSize; z += 0.5f)
			{
				for (float x = 0; x <= xSize; x += 0.5f)
				{
					vertexDetailed[i] = new TerrainVertex();
					vertexDetailed[i].position = new Vector3(-.5f + x, -.5f + z, 0);
					i++;
				}
			}
			Debug.Log(i);
			for (int z = 0; z < zSize; z++) for (int x = 0; x < xSize; x++)
				{
					int index = (1 + 2 * x) + (1 + 2 * xSize) * (1 + z * 2);
					vertexDetailed[index].type = (int)instance.pieces[xSize * z + x].Type;
					vertexDetailed[index].addInfluencedType((int)instance.pieces[xSize * z + x].Type);
					vertexDetailed[index].renderWeight = (int)instance.pieces[xSize * z + x].RenderWeight;
				}
		}
		Debug.Log("detailed legnth " + vertexDetailed.Length);


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
					new Vector2[]{new Vector2(centerX - 1, centerZ + 1),new Vector2(centerX + 0, centerZ + 1),new Vector2(centerX + 1, centerZ + 1) },
					new Vector2[]{new Vector2(centerX+1,centerZ+0),new Vector2(centerX + 1, centerZ + 1),new Vector2(centerX + 1, centerZ -1) },
					new Vector2[]{new Vector2(centerX -1 , centerZ - 1),new Vector2(centerX+0, centerZ - 1),new Vector2(centerX + 1, centerZ -1) },
					new Vector2[]{new Vector2(centerX - 1, centerZ + 0), new Vector2(centerX - 1, centerZ + 1), new Vector2(centerX - 1, centerZ - 1) }
				};
				var tile = instance.pieces[instance.Width * z + x];

				for (int k = 0; k < 4; k++)
				{
					int xRowSize = 1 + 2 * xSize;
					int zColumnSize = 1 + 2 * zSize;
					var adjTileIndex = adjTiles[k];
					var adjEdgeIndexs = adjEdges[k].OfType<Vector2>().ToList();

					bool isAdjacentTileLegit = EzT.ChkWithinBoundaries(adjTileIndex, new Vector2(0, 0), new Vector2(xSize - 1, zSize - 1));

					var tileAdj = (isAdjacentTileLegit) ?
						instance.pieces[instance.Width * (int)adjTileIndex.y + (int)adjTileIndex.x] : null;

					for (int i = adjEdgeIndexs.Count - 1; i >= 0; i--)
					{
						var edgeIndex = adjEdgeIndexs[i];
						bool isAdjacentEdgeLegit = EzT.ChkWithinBoundaries(edgeIndex, new Vector2(0, 0), new Vector2(xRowSize - 1, zColumnSize - 1));
						if (!isAdjacentEdgeLegit)
						{
							adjEdgeIndexs.RemoveAt(i);
						}
					}
					//get vertices
					List<TerrainVertex> edges = adjEdgeIndexs.Select(v => this.vertexDetailed[(int)v.x + (int)((v.y) * (1 + 2 * xSize))]).ToList();

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

		#region Iniritiate Vertex normal values
		for (int z = 0; z < zSize; z++)
			for (int x = 0; x < xSize; x++)
			{

			}
		#endregion

	}

	void hprUpdateInfluencedVertexs(
		Piece piece, Piece pieceAdjacent,
		List<TerrainVertex> vertexInfluenced)
	{
		int selectedType;
		Piece selectedPiece;
		bool isEqual = false;
		if (pieceAdjacent == null)
		{
			selectedType = (int)piece.Type;
			selectedPiece = piece;
		}
		else
		{
			isEqual = piece.RenderWeight == pieceAdjacent.RenderWeight;
			selectedPiece = (piece.RenderWeight >= pieceAdjacent.RenderWeight) ?
				piece : pieceAdjacent;
			selectedType = (int)selectedPiece.Type;
		}
		for (int i = 0; i < vertexInfluenced.Count; i++)
		{
			if (isEqual)
			{
				vertexInfluenced[i].addInfluencedType((int)piece.Type);
				vertexInfluenced[i].addInfluencedType((int)pieceAdjacent.Type);
			}
			if (vertexInfluenced[i].type > selectedType)
			{
				vertexInfluenced[i].addInfluencedType(vertexInfluenced[i].type);
				continue;
			}

			vertexInfluenced[i].addInfluencedType(selectedType);
			//Debug.Log(vertexInfluenced[i].position.x + " " + vertexInfluenced[i].position.z + " COLOR = " + selectedType);
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
		triangles = new int[(xSize) * (zSize) * 8 * 3]; // I need 8 triangles for each square



		int totalVerticesUsedToExpressSquare = 8 * 3;
		int triangleIndex = 0;
		int horizontalVertexRawCount = 1 + 2 * xSize;

		for (int z = 0; z < zSize; z++)
		{
			int zIndex = horizontalVertexRawCount * z;
			for (int x = 0; x < xSize; x++)
			{
				int centerIndex = x + xSize * z;

				int center = 1 + 2 * x + horizontalVertexRawCount * (2 * z + 1);
				//int cornerB = cornerStartIndex+zIndex + x + 1;

				//bottom
				triangles[triangleIndex + 0] = center - horizontalVertexRawCount - 1;
				triangles[triangleIndex + 1] = center;
				triangles[triangleIndex + 2] = triangles[triangleIndex + 0] + 1;

				triangles[triangleIndex + 3] = center;
				triangles[triangleIndex + 4] = center - horizontalVertexRawCount + 1;
				triangles[triangleIndex + 5] = center - horizontalVertexRawCount;

				//right
				triangles[triangleIndex + 6] = center;
				triangles[triangleIndex + 7] = center + 1;
				triangles[triangleIndex + 8] = center - horizontalVertexRawCount + 1;

				triangles[triangleIndex + 9] = center;
				triangles[triangleIndex + 10] = center + horizontalVertexRawCount + 1;
				triangles[triangleIndex + 11] = center + 1;

				//up
				triangles[triangleIndex + 12] = center;
				triangles[triangleIndex + 13] = center + horizontalVertexRawCount;
				triangles[triangleIndex + 14] = center + horizontalVertexRawCount + 1;

				triangles[triangleIndex + 15] = center;
				triangles[triangleIndex + 16] = center + horizontalVertexRawCount - 1;
				triangles[triangleIndex + 17] = center + horizontalVertexRawCount;

				//left
				triangles[triangleIndex + 18] = center;
				triangles[triangleIndex + 19] = center - 1;
				triangles[triangleIndex + 20] = center + horizontalVertexRawCount - 1;

				triangles[triangleIndex + 21] = center;
				triangles[triangleIndex + 22] = center - horizontalVertexRawCount - 1;
				triangles[triangleIndex + 23] = center - 1;



				//triangles[triangleIndex + 3] = cornerB ;
				//triangles[triangleIndex + 4] = cornerA + horizontalVertexRawCount;
				//triangles[triangleIndex + 5] = cornerB + horizontalVertexRawCount;

				triangleIndex += totalVerticesUsedToExpressSquare;
			}
		}

	}

	Color hprIntToColor(int n)
	{
		if (n == 0) return new Color(1, 0, 0);
		if (n == 1) return new Color(0, 1, 0);
		if (n == 2) return new Color(0, 0, 1);
		return Color.black;
	}

	void hpr(TerrainVertex tVertex, ref Vector2 vec2, bool isFirst, int checkedType)
	{
		if (isFirst)
		{
			if (tVertex.type == checkedType)
				vec2 = new Vector2(1, vec2.y);
		}
		else
		{
			if (tVertex.type == checkedType)
				vec2 = new Vector2(vec2.x, 1);
		}
	}

	void hprType(int tVertexType, ref Vector2 vec2, bool isFirst, int checkedType)
	{
		if (isFirst)
		{
			if (tVertexType == checkedType)
				vec2 = new Vector2(1, vec2.y);
		}
		else
		{
			if (tVertexType == checkedType)
				vec2 = new Vector2(vec2.x, 1);
		}
	}

	private void UpdateMesh()
	{
		mesh.Clear();

		var positionsDetailedVertexs = vertexDetailed.Select(s => s.position).ToArray();

		var colorsDetailed = vertexDetailed.Select(s => hprIntToColor(s.type)).ToArray();

		//center color
		//64개 필요함
		//mesh.uv2~8 // 2* 7 = 14
		//	mesh.normals// 4
		//mesh.tangents// 4
		//32개 까지 가능
		List<List<Vector2>> typeVec2 = new List<List<Vector2>>();
		for (int i = 0; i < 15; i++)
		{
			typeVec2.Add(new List<Vector2>());
		}
		var uv2Empty = new List<Vector2>();
		var uv3Empty = new List<Vector2>();

		for (int i = 0; i < vertexDetailed.Count(); i++)
		{

			int typeIndex = 0;
			for (int j = 0; j < 15; j++)
			{
				Vector2 uv = new Vector2(0, 0);
				for (int k = 0; k < vertexDetailed[i].typeRendered.Count; k++)
				{
					int selectedRenderType = vertexDetailed[i].typeRendered[k];
					hprType(selectedRenderType, ref uv, true, typeIndex);
					hprType(selectedRenderType, ref uv, false, typeIndex+1);
				}
				typeVec2[j].Add(uv);
				typeIndex += 2;
			}

			/*
			*/

			/*
			float uv2X = 0, uv2Y = 0;
			var uv2 = new Vector2(0, 0);
			var uv3 = new Vector2(0, 0);
			hpr(vertexDetailed[i], ref uv2, true, 0);
			hpr(vertexDetailed[i], ref uv2, false, 1);
			hpr(vertexDetailed[i], ref uv3, true, 2);
			hpr(vertexDetailed[i], ref uv3, false, 3);


			uv2Empty.Add(uv2);
			uv3Empty.Add(uv3);
			*/
		}

		//mesh.
		mesh.vertices = positionsDetailedVertexs;
		mesh.colors = colorsDetailed;

		//mesh.uv2 = uv2Empty.ToArray();
		//mesh.uv3 = uv3Empty.ToArray();
		mesh.uv = typeVec2[0].ToArray();
		mesh.uv2 = typeVec2[1].ToArray();

		mesh.name = "I made this";
		mesh.triangles = triangles;
		mesh.RecalculateNormals();

	}

	private void OnDrawGizmos()
	{
		return;
		if (sterrain == null) return;

		for (int x = 0; x < xSize; x++)
		{
			for (int z = 0; z < zSize; z++)
			{
				Gizmos.color = Color.white;
				if ((int)sterrain.pieces[x + z * (xSize)].Type == 0)
				{
					Gizmos.color = Color.red;

				}
				if ((int)sterrain.pieces[x + z * (xSize)].Type == 1)
				{
					Gizmos.color = Color.green;

				}
				if ((int)sterrain.pieces[x + z * (xSize)].Type == 2)
				{
					Gizmos.color = Color.blue;

				}
				Gizmos.DrawSphere(new Vector3(x, 0, z), .1f);

			}
		}
		return;
	}


}