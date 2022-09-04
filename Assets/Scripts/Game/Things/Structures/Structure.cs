using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class Structure : Thing_Describable
{
	CAIModel CAIModel;

	public Structure(Game.CATEGORY category, CAIModel data) :base(category)
	{
		this.CAIModel = data;
	}
}