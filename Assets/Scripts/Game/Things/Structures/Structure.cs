using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class Structure : ThingWithPhysicalPresence
{
	CAIModel CAIModel;

	public Structure(Game.CATEGORY category, CAIModel data) :base(category)
	{
		this.CAIModel = data;
	}
}