using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

static class CAIModelSheet
{
	public static CAIModel Bed;
	public static CAIModel DoubleBed;
	public static CAIModel Chair;
	public static CAIModel ChairDouble;
	public static CAIModel Table;

	public static void Init()
	{
		Bed = new CAIModel();
		Bed.setCollisionMap(new UnityEngine.Vector2(0, 0));
		Bed.setCollisionMap(new UnityEngine.Vector2(0, 1));

		DoubleBed = new CAIModel();
		DoubleBed.setCollisionMap(new UnityEngine.Vector2(0, 0));
		DoubleBed.setCollisionMap(new UnityEngine.Vector2(0, 1));
		DoubleBed.setCollisionMap(new UnityEngine.Vector2(1, 0));
		DoubleBed.setCollisionMap(new UnityEngine.Vector2(1, 1));

		Chair = new CAIModel();
		Chair.setCollisionMap(new UnityEngine.Vector2(0, 0));

		ChairDouble = new CAIModel();
		ChairDouble.setCollisionMap(new UnityEngine.Vector2(0, 0));
		ChairDouble.setCollisionMap(new UnityEngine.Vector2(1, 0));

		Table = new CAIModel();
		Table.setCollisionMap(new UnityEngine.Vector2(0, 0));


	}
}