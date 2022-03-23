using System.Collections;
using UnityEngine;

public class ThingDestructable : Thing
{
	public float health = 100;

	public bool IsAlvie { get { return health > 0; } }
	public void TakeHealthChange(float amount)
	{
		health += amount;
	}
}