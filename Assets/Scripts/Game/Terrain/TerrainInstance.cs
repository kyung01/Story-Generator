using System.Collections;
using UnityEngine;

namespace StoryGenerator.Terrain
{
	/// <summary>
	/// Piece of terrain, contains information about the particular terrain piece 
	/// </summary>
	public class TerrainInstance
	{
		int width, height;
		
		public int Width
		{
			get { return this.width; }
		}
		public int Height
		{
			get { return this.height;}
		}
		

		public Piece[] pieces;
		public bool[] mountain;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="seed"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="scale">Scale is not amplifier, it defines how wider the map is</param>
		/// <returns></returns>
		float[][] getPerlinNoise(int seed, int width, int height, float scale = 1, float amplifier = 1, float offset = 0)
		{
			Random.InitState(seed);
			float ranX = Random.Range(0, 100.0f);
			float ranY = Random.Range(0, 100.0f);

			float[][] noiseMap = new float[width][];
			mountain = new bool[width * height];
			for (int i = 0; i < width; i++){
				noiseMap[i] = new float[height];
				for (int j =0; j < height; j++)
				{
					noiseMap[i][j] = Mathf.PerlinNoise(ranX + (float)i * scale, ranY + (float)j * scale) * amplifier + offset;
				}
			}
			return noiseMap;
		}
		public void Init(int width, int height)
		{
			this.width = width;
			this.height = height;

			pieces = new Piece[width * height];
			for(int i = 0; i< pieces.Length; i++)
			{
				pieces[i] = new Piece();
			}
			float probToBeFertile = 0.4f;


			float probToBeRocky = 0.3f;

			float probToBeMountainGround = 0.7f;
			float probToBeMountainRocks = probToBeMountainGround - 0.05f;


			float probClay = 0.5f;
			float probWater = probClay - 0.05f;
			float probWaterDeep = probWater - 0.05f;

			Random.InitState((int)System.DateTime.Now.Second);
			int seed = Random.Range(0, 100000);

			var perlinMap_fertility = getPerlinNoise(seed +0, width, height, 0.1f);
			var perlinMap_Rocky = getPerlinNoise(seed + 1, width, height, 0.1f);
			var perlinMap_Mountain = getPerlinNoise(seed + 2, width, height, 0.05f,2,-1f);
			var perlinMap_Water = perlinMap_Mountain;// getPerlinNoise(seed + 3, width, height, 0.05f);
			for (int i = 0; i < width; i++)
			{
				for(int j = 0; j < height; j++)
				{
					if (perlinMap_fertility[i][j] > (1 - probToBeFertile))
					{
						pieces[i + j * width].SetType(Piece.KType.FERTILE);
					}
					if (perlinMap_Rocky[i][j] > (1 - probToBeRocky))
					{
						pieces[i + j * width].SetType(Piece.KType.ROCKY);
					}
					if (perlinMap_Mountain[i][j] > (1 - probToBeMountainGround))
					{
						pieces[i + j * width].SetType(Piece.KType.MOUNTAIN_GROUND);
					}
					if (perlinMap_Mountain[i][j] > (1 - probToBeMountainRocks))
					{
						mountain[i + j * width] = true;
						pieces[i + j * width].SetType(Piece.KType.MOUNTAIN);
					}
					/*
					 * */
					if (perlinMap_Water[i][j] < -( 1- probClay))
					{
						pieces[i + j * width].SetType(Piece.KType.CLAY);
						mountain[i + j * width] = false;
					}
					if (perlinMap_Water[i][j] < -( 1- probWater))
					{
						pieces[i + j * width].SetType(Piece.KType.WATER_SHALLOW);
					}
					if (perlinMap_Water[i][j] < -( 1- probWaterDeep))
					{
						pieces[i + j * width].SetType(Piece.KType.WATER_DEEP);
					}
				}
			}

		}

		public Piece GetPieceAt(int x, int y)
		{
			return pieces[x + y * width];
		}
	}
}