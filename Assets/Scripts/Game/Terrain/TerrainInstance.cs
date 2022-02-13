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

		float[][] getPerlinNoise(int seed, int width, int height, float scale)
		{
			Random.InitState(seed);
			float ranX = Random.Range(0, 100.0f);
			float ranY = Random.Range(0, 100.0f);

			float[][] noiseMap = new float[width][];
			for(int i = 0; i < width; i++){
				noiseMap[i] = new float[height];
				for (int j =0; j < height; j++)
				{
					noiseMap[i][j] = Mathf.PerlinNoise(ranX + (float)i * scale, ranY + (float)j * scale);
				}
			}
			return noiseMap;
		}
		public void init(int width, int height)
		{
			this.width = width;
			this.height = height;

			pieces = new Piece[width * height];
			for(int i = 0; i< pieces.Length; i++)
			{
				pieces[i] = new Piece();
			}
			float chanceToBeFertile = 0.4f;
			float chanceToBeRocky = 0.3f;
			float chanceToBeMountain = 0.4f;
			int seed = Random.Range(0, 100);

			var perlinMap_fertility = getPerlinNoise(seed +0, width, height, 0.1f);
			var perlinMap_Rocky = getPerlinNoise(seed + 1, width, height, 0.1f);
			var perlinMap_Mountain = getPerlinNoise(seed + 2, width, height, 0.05f);
			for (int i = 0; i < width; i++)
			{
				for(int j = 0; j < height; j++)
				{
					if (perlinMap_fertility[i][j] > (1 - chanceToBeFertile))
					{
						pieces[i + j * width].SetType(Piece.KType.FERTILE);
					}
					if (perlinMap_Rocky[i][j] > (1 - chanceToBeRocky))
					{
						pieces[i + j * width].SetType(Piece.KType.ROCKY);
					}
					if (perlinMap_Mountain[i][j] > (1 - chanceToBeMountain))
					{
						pieces[i + j * width].SetType(Piece.KType.MOUNTAIN);
					}
				}
			}

		}


	}
}