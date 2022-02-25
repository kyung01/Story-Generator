using System.Collections;
using System.Collections.Generic;
using StoryGenerator.Terrain;
using UnityEngine;

namespace StoryGenerator.World
{
	/// <summary>
	/// Piece of terrain, contains information about the particular terrain piece 
	/// </summary>
	public class World
	{
		public TerrainInstance terrain = new TerrainInstance();
		public int width = 50;
		public int height = 50;
		List<Thing>[] things;

		public void InitTerrain(PrefabList prefabList)
		{
			//initialize the world 
			terrain.init(width,height);
			things = new List<Thing>[width * height];
			for(int i  = 0; i < width;i ++)for(int j = 0; j < height; j++)
				{
					things[i + j * width] = new List<Thing>();
					if (terrain.mountain[i + j * width])
					{
						//mountain is here
						continue;
					}
					var t = terrain.pieces[i + j * width];
					Thing thing = null;
					float chanceToSpawn = 30;
					if (Random.Range(0, 100) > chanceToSpawn) continue;

					switch (t.Type)
					{
						case Piece.KType.FERTILE:
						case Piece.KType.DIRT:
							//grass bush or tree
							Thing[] vegitarians = new Thing[] { prefabList.Grass00, prefabList.Grass00, prefabList.Grass00,prefabList.Grass00, prefabList.Tree00, prefabList.Tree01, prefabList.Bush00 };
							thing = vegitarians[Random.Range(0, vegitarians.Length)];
							break;
						case Piece.KType.ROCKY:
							thing = prefabList.Rock00;
							break;
						case Piece.KType.CLAY:
							thing = prefabList.Reed00;
							break;
						case Piece.KType.MOUNTAIN:
						case Piece.KType.WATER_SHALLOW:
						case Piece.KType.WATER_DEEP:
							break;
						default:
							break;
					}
					if (thing == null) continue;
					var obj = GameObject.Instantiate(thing);
					obj.transform.position = new Vector3(i, j, -0.001f);
					things[i + j * width].Add(obj);
					//things[i+j*width] 
				}

		}

	}
}