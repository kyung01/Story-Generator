using System.Collections;
using UnityEngine;

namespace StoryGenerator.Terrain
{
	/// <summary>
	/// Piece of terrain, contains information about the particular terrain piece 
	/// </summary>
	public class Piece
	{
		public enum KType { DIRT = 0 , ROCKY = 1, FERTILE = 2,
			MOUNTAIN = 3,
			CLAY = 4,
			WATER_SHALLOW = 5,
			WATER_DEEP = 6
		}
		KType type = KType.DIRT;

		float renderWeight = 0;

		public KType Type {get{return this.type;} }
		public float RenderWeight { get { return this.renderWeight; } }

		public Piece()
		{
			this.type = KType.DIRT;
			/*
			float chanceToBeNotNormal = 30;
			if (Random.Range(0, 100) < chanceToBeNotNormal)
			{
				this.type = (KType)Random.Range(1, 3);
			}
			*/
		}
		public void SetType(KType type)
		{
			this.type = type;	
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