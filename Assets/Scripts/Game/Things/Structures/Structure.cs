﻿using GameEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class Structure : ThingWithPhysicalPresence
{
	CAIModel _CAIModel;
	public CAIModel CAIModel { get { return this._CAIModel; } }


	public Structure(ThingCategory category, CAIModel data) :base(category)
	{
		this._CAIModel = data;
	}

}