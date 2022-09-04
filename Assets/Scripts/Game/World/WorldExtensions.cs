using GameEnums;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static StoryGenerator.World.World;

public static class WorldExtensions
{
	public static void Raise(this List<DEL_THING_ADDED> list, Thing thing)
	{
		foreach (var d in list)
		{
			d(thing);
		}
	}
}
