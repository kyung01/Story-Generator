using GameEnums;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

static class CAIModelSheet
{
	public static CAIModel Bed_Single;
	public static CAIModel DoubleBed;
	public static CAIModel Chair;
	public static CAIModel ChairDouble;
	public static CAIModel Table;
	public static CAIModel Storage;

	public static void Init()
	{
		Bed_Single = new CAIModel();
		Bed_Single.setCollisionMap(
			new UnityEngine.Vector2(0, 0), 
			new UnityEngine.Vector2(0, 1)
			);

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

		Storage = new CAIModel();
		Storage.setCollisionMap(new Vector2(0, 0));


	}
}