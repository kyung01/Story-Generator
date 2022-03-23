using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class ThingExtensions
{
	static public float GetEatingSpeed(this Thing thing)
	{
		return 50;
	}
	static public float GetEatingDistance(this Thing thing)
	{
		return 1;
	}
}