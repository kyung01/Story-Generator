using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Plants provide basic keywords for free as a form of "fruit"
public partial class Thing
{
	HouseModule houseModule;
	public HouseModule House { get { return this.houseModule; } }

	public void InitHouse()
	{
		houseModule = new HouseModule();
		houseModule.Init(this);
	}
}
public class HouseModule : ThingModule
{
	HouseZone house;

	public void SetHouseZone(HouseZone houseZone)
	{
		this.house = houseZone;
	}
}
