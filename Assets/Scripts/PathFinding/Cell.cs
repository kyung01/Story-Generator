using UnityEngine;
using System.Collections;

namespace PathFinder
{

	public class Cell
	{
		public bool isOccupied = false;
		public float weight = 0;
		public float weightInt = 0;
		public float Weight
		{
			set
			{
				weight = value;
			}
			get
			{
				return weight + weightInt;
			}
		}


	}

}
