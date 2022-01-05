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

		public void init(int width, int height)
		{
			this.width = width;
			this.height = height;

			pieces = new Piece[width * height];
			for(int i = 0; i< pieces.Length; i++)
			{
				pieces[i] = new Piece();
			}

		}


	}
}