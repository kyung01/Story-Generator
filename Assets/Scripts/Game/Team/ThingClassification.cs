using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ThingClassification
{
	public Thing thing;
	public List<Team.ThingRole> roles = new List<Team.ThingRole>();
	public ThingClassification(Thing thing, Team.ThingRole role)
	{
		this.thing = thing;
		this.roles.Add( role);

	}

}