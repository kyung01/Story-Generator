using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

static class CAIModelSheet
{
	public static CAIModel Bed;

	public static void Init()
	{
		Bed = new CAIModel();
		Bed.setCollisionMap(new UnityEngine.Vector2(0, 0));
		Bed.setCollisionMap(new UnityEngine.Vector2(0, 1));


	}
}