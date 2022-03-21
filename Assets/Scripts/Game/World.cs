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
		public List<Thing>[] things;
		public List<Thing> allThings = new List<Thing>();

		public void InitTerrain()
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
							Thing[] vegitarians = new Thing[] {
								new Plant(Thing.TYPE.GRASS), new Plant(Thing.TYPE.BUSH),new Thing(Thing.TYPE.ROCK)};
							thing = vegitarians[Random.Range(0, vegitarians.Length)];
							break;
						case Piece.KType.ROCKY:
							thing = new Thing(Thing.TYPE.ROCK);
							break;
						case Piece.KType.CLAY:
							thing = new Thing(Thing.TYPE.REED);
							break;
						case Piece.KType.MOUNTAIN:
						case Piece.KType.WATER_SHALLOW:
						case Piece.KType.WATER_DEEP:
							break;
						default:
							break;
					}
					if (thing == null) continue;
					
					var obj = thing;
					obj.SetPosition(i, j);
					things[i + j * width].Add(obj);
					allThings.Add(obj);
					obj.Init(this);
					//things[i+j*width] 
				}

		}

		internal float GetThingsSpeed(Thing thing)
		{
			if (thing.type == Thing.TYPE.RABBIT)
			{
				return 3f;
			}
			return 1;
		}

		public void InitAnimals()
		{
			int numAnimal = 10;
			for(int i = 0; i < numAnimal; i++)
			{
				float x=0, y=0;
				bool isAppropriateLocationFound = false;
				while (!isAppropriateLocationFound)
				{
					x = Random.Range(0, width);
					y = Random.Range(0, height);
					var terrainType = terrain.pieces[(int)x + (int)y * width].Type;
					if(terrainType == Piece.KType.MOUNTAIN || terrainType == Piece.KType.WATER_DEEP ||  terrainType == Piece.KType.WATER_SHALLOW)
					{
						continue;
					}
					break;
				}
				Rabbit rabbit = new Rabbit();
				rabbit.SetPosition(x, y);
				rabbit.Init(this);
				allThings.Add(rabbit);

			}
		}

		public virtual void Update(float timeElapsed)
		{
			for(int i = 0; i< allThings.Count; i++)
			{
				allThings[i].Update(this, timeElapsed);
			}
		}
	}
}