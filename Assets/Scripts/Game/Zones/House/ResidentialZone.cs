using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ResidentDescription { 
}

public class ResidentialZone : Zone
{
	public Dictionary<Thing, ResidentDescription> residents = new Dictionary<Thing, ResidentDescription>();
}
