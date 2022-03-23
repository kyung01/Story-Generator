using System.Collections;
using UnityEngine;

public class Mouth : BodyTaskable
{
	float jawPower = 1;
	float eatingSpeed = 1;
	public float JawPower { get { return jawPower; } }

	public void Bite(Thing thingToBite)
	{
		bool isBitable = thingToBite is ThingDestructable;
		var t = (ThingDestructable)thingToBite;
		t.TakeHealthChange(JawPower);
	}
}