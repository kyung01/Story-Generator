using System.Collections;
using UnityEngine;

namespace StoryGenerator.Terrain
{
	/// <summary>
	/// Piece of terrain, contains information about the particular terrain piece 
	/// </summary>
	public class Piece
	{
		public enum KType { DIRT = 0 , GRASS = 1 }
		KType type = KType.DIRT;
		public KType Type {get{return this.type;} }
		public Piece()
		{
			this.type = (KType) Random.Range(0, 3);
		}

	}
}