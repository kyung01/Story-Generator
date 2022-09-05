using StoryGenerator.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class OccupyingSpot
{
	int relativeX, relativeY;

	public void GetXY(ThingWithPhysicalPresence me, out int x, out int y)
	{
		me.GetRelativePosition(relativeX, relativeY, out x, out y);

	}

	Thing thingOccupying = null;

	public Thing Occupier { get { return this.thingOccupying; } }

	public OccupyingSpot(int x, int y)
	{
		this.relativeX = x;
		this.relativeY = y;
	}

	public bool IsFree()
	{
		return thingOccupying == null;
	}
	public void GetOuccpiedBy(Thing thing)
	{
		this.thingOccupying = thing;
	}
	public void Update(World world, Thing thing)
	{

	}

	internal void Free()
	{
		this.thingOccupying = null;
	}
}