using System.Collections;
using UnityEngine;

namespace StoryGenerator.Terrain
{
	/// <summary>
	/// Piece of terrain, contains information about the particular terrain piece 
	/// </summary>
	public class Piece
	{
		public enum KType { DIRT = 0 , ROCKY = 1, FERTILE = 2 }
		KType type = KType.DIRT;
		float renderWeight = 0;

		public KType Type {get{return this.type;} }
		public float RenderWeight { get { return this.renderWeight; } }

		public Piece()
		{
			if(Random.Range(0,100) < 50)
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
				case KType.ROCKY:
					renderWeight = 0;
					break;
				case KType.FERTILE:
					renderWeight = 0;
					break;
				default:
					break;
			}
		}

	}
}