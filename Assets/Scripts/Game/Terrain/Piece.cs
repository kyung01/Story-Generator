using System.Collections;
using UnityEngine;

namespace StoryGenerator.Terrain
{
	/// <summary>
	/// Piece of terrain, contains information about the particular terrain piece 
	/// </summary>
	public class Piece
	{
		public enum KType { DIRT = 0 , GRASS = 1, Water = 2 }
		KType type = KType.DIRT;
		float renderWeight = 0;

		public KType Type {get{return this.type;} }
		public float RenderWeight { get { return this.renderWeight; } }

		public Piece()
		{
			if(Random.Range(0,100) < 30)
			{

				this.type = (KType)Random.Range(1, 3);
			}
			else
			{
				this.type = KType.DIRT;
			}
			switch (this.type)
			{
				case KType.DIRT:
					renderWeight = 0;
					break;
				case KType.GRASS:
					renderWeight = 1;
					break;
				case KType.Water:
					renderWeight = 2;
					break;
				default:
					break;
			}
		}

	}
}