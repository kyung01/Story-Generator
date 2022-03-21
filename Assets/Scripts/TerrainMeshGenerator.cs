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
	int ySize = 3;

	TerrainVertex[] vertexEdgesAndCenters;


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

	void InitVerticies(StoryGenerator.Terrain.TerrainInstance terrainInstance)
	{
		xSize = terrainInstance.Width;
		ySize = terrainInstance.Height;
		vertexEdgesAndCenters = new TerrainVertex[(1 + 2 * xSize) * (1 + 2 * ySize)];
		int detailedRowSize = 1 + 2 * xSize;

		#region Initiate center

		#endregion

		#region Initiate vertex positions
		{
			//detailed vertex initialization 
			int i = 0;
			for (float z = 0; z <= ySize; z += 0.5f)
			{
				for (float x = 0; x <= xSize; x += 0.5f)
				{
					vertexEdgesAndCenters[i] = new TerrainVertex();
					vertexEdgesAndCenters[i].position = new Vector3(-.5f + x, -.5f + z, 0);
					i++;
				}
			}
			Debug.Log(i);
			int oneLine = (1 + 2 * xSize);
			for (int z = 0; z < ySize; z++) for (int x = 0; x < xSize; x++)
				{
					int index = oneLine * (1 + z * 2) + (1 + 2 * x);
					var p = terrainInstance.pieces[xSize * z + x];
					//vertexEdgesAndCenters[index].type = (int)terrainInstance.pieces[xSize * z + x].Type;
					//vertexEdgesAndCenters[index].addInfluencedType((int)terrainInstance.pieces[xSize * z + x].Type);
					vertexEdgesAndCenters[index].renderWeight = (int)terrainInstance.pieces[xSize * z + x].RenderWeight;
					vertexEdgesAndCenters[index].typeRendered.Add((int)terrainInstance.pieces[xSize * z + x].Type);
					vertexEdgesAndCenters[index].typePower[(int)p.Type] = 1;
				}
		}
		Debug.Log("detailed legnth " + vertexEdgesAndCenters.Length);


		#endregion

		#region Initiate vertex types
		for (int z = 0; z < ySize; z++)
		{
			for (int x = 0; x < xSize; x++)
			{
			}
		}
		
		for (int y = 0; y < ySize; y++)
		{
			for (int x = 0; x < xSize ; x++)
			{
				int centerX = 1 + x * 2;
				int centerY = 1 + y * 2;
				var surroundingVertices = new Vector2[] {
					new Vector2(centerX-1, centerY+1),
					new Vector2(centerX+0, centerY+1),
					new Vector2(centerX+1, centerY+1),
										 
					new Vector2(centerX-1, centerY),
					new Vector2(centerX+1, centerY),
										 
					new Vector2(centerX-1, centerY-1),
					new Vector2(centerX+0, centerY-1),
					new Vector2(centerX+1, centerY-1)
				}.ToList();
				for (int i = surroundingVertices.Count() - 1; i >= 0; i--)
				{
					var v = surroundingVertices[i];
					bool isLegit = EzT.ChkWithinBoundaries(v, new Vector2(0, 0), new Vector2(1 + (xSize * 2) - 1, 1 + (ySize * 2) - 1));
					if (!isLegit)
					{
						surroundingVertices.RemoveAt(i);

					}

				}
				var tile = terrainInstance.pieces[terrainInstance.Width * y + x];
				var e2 = surroundingVertices.Select(s => vertexEdgesAndCenters[(int)s.x + (int)s.y * (1 + 2 * xSize)]).ToList();
				for(int k = 0;k < e2.Count; k++)
				{
					float distance = (e2[k].position - new Vector3(x, y, 0)).magnitude / 0.5f;
					distance *= distance;
					e2[k].typePower[(int)tile.Type] = Mathf.Max(distance , e2[k].typePower[(int)tile.Type]);
				}
				hprUpdateInfluencedVertexs(tile, e2);

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
		for (int z = 0; z < ySize; z++)
			for (int x = 0; x < xSize; x++)
			{

			}
		#endregion

	}

	void hprUpdateInfluencedVertexs(
		Piece piece, List<TerrainVertex> vertexInfluenced)
	{
		for (int i = 0; i < vertexInfluenced.Count; i++)
		{
			vertexInfluenced[i].typeRendered.Add((int)piece.Type);
		}

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
		triangles = new int[(xSize) * (ySize) * 8 * 3]; // I need 8 triangles for each square



		int totalVerticesUsedToExpressSquare = 8 * 3;
		int triangleIndex = 0;
		int horizontalVertexRawCount = 1 + 2 * xSize;

		for (int z = 0; z < ySize; z++)
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


	void hprType(int tVertexType, ref Vector2 vec2, bool isFirst, int checkedType)
	{
		if (isFirst)
		{
			if (tVertexType == checkedType)
				vec2 = new Vector2(vec2.x + 1.0f, vec2.y);
		}
		else
		{
			if (tVertexType == checkedType)
				vec2 = new Vector2(vec2.x, vec2.y + 1.0f);
		}
	}

	private void UpdateMesh()
	{
		mesh.Clear();

		var positionsDetailedVertexs = vertexEdgesAndCenters.Select(s => s.position).ToArray();

		var colorsDetailed = vertexEdgesAndCenters.Select(s => hprIntToColor(s.type)).ToArray();

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

		for (int i = 0; i < vertexEdgesAndCenters.Count(); i++)
		{

			int typeIndex = 0;
			int n = vertexEdgesAndCenters[i].typeRendered.Count;
			var isMultipleColor = refreshHprIsMultipleColor(ref vertexEdgesAndCenters[i].typeRendered);

			//Debug.Log("[COUNT] " + n + " -> " + vertexEdgesAndCenters[i].typeRendered.Count);
			List<Vector2> vec2List = new List<Vector2>();
			for (int j = 0; j < 15; j++)
			{
				Vector2 uv = new Vector2(0, 0);
				for (int k = 0; k < vertexEdgesAndCenters[i].typeRendered.Count; k++)
				{

					int selectedRenderType = vertexEdgesAndCenters[i].typeRendered[k];
					hprType(selectedRenderType, ref uv, true, typeIndex);
					hprType(selectedRenderType, ref uv, false, typeIndex + 1);
				}
				//typeVec2[j].Add(uv);
				vec2List.Add(new Vector2(vertexEdgesAndCenters[i].typePower[typeIndex], vertexEdgesAndCenters[i].typePower[typeIndex+1]));
				typeIndex += 2;
			}
			float greatest = 0;

			for (int j = 0; j < 30; j++)
			{
				//vertexEdgesAndCenters[i].typePower[j] = Mathf.Pow(vertexEdgesAndCenters[i].typePower[j],3);
				//vec2List[j] = new Vector2(Mathf.Min(vec2List[j].x, 1), Mathf.Min(vec2List[j].y, 1f));

			}
			for (int j = 0; j < 15; j++)
			{
				//vec2List[j] = new Vector2(Mathf.Min(vec2List[j].x, 1), Mathf.Min(vec2List[j].y, 1f));

			}
			for (int j = 0; j < 15; j++)
			{
				//vec2List[j] = new Vector2(Mathf.Pow(vec2List[j].x, 2f), Mathf.Pow(vec2List[j].y, 2f));
				//vec2List[j] = new Vector2(vec2List[j].x*0.5f, vec2List[j].y*0.5f);

			}
			for (int j = 0; j < 15; j++)
			{

				//greatest = Mathf.Max(vertexEdgesAndCenters[i].typePower[j * 2], greatest);
				//greatest = Mathf.Max(vertexEdgesAndCenters[i].typePower[j * 2 + 1], greatest); ;
				greatest += vertexEdgesAndCenters[i].typePower[j * 2] + vertexEdgesAndCenters[i].typePower[j * 2 + 1];
				//greatest += vec2List[j].x + vec2List[j].y;

			}
			if (greatest != 0)
			{
				for (int j = 0; j < 15; j++)
				{
					vec2List[j] = new Vector2(vec2List[j].x / greatest, vec2List[j].y / greatest);
				}

			}

			for (int j = 0; j < 15; j++)
			{
				float pow = 3;
				vec2List[j] = new Vector2(Mathf.Pow(vec2List[j].x, pow), Mathf.Pow(vec2List[j].y, pow ) );
			}
			greatest = 0;
			for (int j = 0; j < 15; j++)
			{
				greatest += vec2List[j].x + vec2List[j].y;
			}
			if (greatest != 0)
			{
				for (int j = 0; j < 15; j++)
				{
					vec2List[j] = new Vector2(vec2List[j].x / greatest, vec2List[j].y / greatest);
				}

			}
			for (int j = 0; j < 15; j++)
			{
				typeVec2[j].Add(vec2List[j]);
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
		mesh.uv3 = typeVec2[2].ToArray();
		mesh.uv4 = typeVec2[3].ToArray();
		mesh.uv5 = typeVec2[4].ToArray();

		mesh.name = "I made this";
		mesh.triangles = triangles;
		mesh.RecalculateNormals();

	}

	private bool refreshHprIsMultipleColor(ref List<int> typeRendered)
	{
		if (typeRendered.Count == 0) return false;
		int n = typeRendered[0];

		bool isMultipleColor = false;
		for(int i = 0; i< typeRendered.Count; i++)
		{
			if (n != typeRendered[i])
			{
				isMultipleColor = true;
				break;
			}
		}
		if (isMultipleColor)
		{
			//typeRendered = new List<int>();
			return true;

		}
		typeRendered = new List<int>();
		typeRendered.Add(n);
		return false;
	}

	private void OnDrawGizmos()
	{
		if (sterrain == null) return;

		for (int x = 0; x < xSize; x++)
		{
			for (int z = 0; z < ySize; z++)
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
				Gizmos.DrawSphere(new Vector3(x, z, 0), .1f);

			}
		}
		return;
	}


}