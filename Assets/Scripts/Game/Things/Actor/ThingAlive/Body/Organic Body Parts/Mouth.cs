using System.Collections;
using UnityEngine;
using System;
using GameEnums;

public class Mouth : BodyTaskable
{
	float jawPower = 1;
	float eatingSpeed = 1;
	public float JawPower { get { return jawPower; } }

	public Mouth()
	{
		this.type = Type.MOUTH;
		this.tasks.Add(TaskType.BITE);
		this.SetName("Mouth");
	}
	public void Bite(Thing me, Thing other)
	{
		if (!IsReady) return;
		Use();
		//bool isBitable = other is ThingDestructable;
		//var t = (ThingDestructable)other;
		//t.TakeHealthChange(me,-JawPower);
	}
}