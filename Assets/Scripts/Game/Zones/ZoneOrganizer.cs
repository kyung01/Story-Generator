using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ZoneOrganizer
{
	List<Zone> zones = new List<Zone>();
	public void BuildZone (int xBeing, int yBeing, int xEnd, int yEnd){ 
	}

	Zone addToA_AnyZone(int x, int y)
	{
		if (zones.Count == 0)
		{
			zones.Add(new Zone());
		}
		for (int i = 0; i < zones.Count; i++)
		{
			if (zones[i].CanYouExpand(x, y))
			{
				zones[i].ExpandZone(x, y);
				return zones[i];
			}
		}
		return null;
	}
}
