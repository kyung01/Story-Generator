using GameEnums;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class Structure : ThingWithPhysicalPresence
{
	CAIModel _CAIModel;
	public CAIModel CAIModel { get { return this._CAIModel; } }


	public Structure(CATEGORY category, CAIModel data) :base(category)
	{
		this._CAIModel = data;
	}
}