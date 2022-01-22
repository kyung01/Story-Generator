using System.Collections;
using StoryGenerator.Terrain;

namespace StoryGenerator.World
{
	/// <summary>
	/// Piece of terrain, contains information about the particular terrain piece 
	/// </summary>
	public class World
	{
		public TerrainInstance terrain = new TerrainInstance();

		public void InitTerrain()
		{
			//initialize the world 
			terrain.init(10, 10);

		}

	}
}