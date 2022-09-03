using System.Collections.Generic;
using UnityEngine;

public class ThingDestructable : Thing
{
	public delegate void DEL_HEALTH_CHANGED(Thing me, Thing other, float dealtChange, float healthBefore, float healthAfter);
	public List<DEL_HEALTH_CHANGED> OnHealthChanged = new List<DEL_HEALTH_CHANGED>();
	public float health = 100;

	public bool IsAlvie { get { return health > 0; } }
	public ThingDestructable() : base(CATEGORY.UNDEFINED)
	{

	}
	public void TakeHealthChange(Thing other, float amount)
	{
		float healthBefore = health;
		float healthAfter = health + amount;
		for (int i = 0; i < OnHealthChanged.Count; i++)
		{
			OnHealthChanged[i](this, other, amount, healthBefore, healthAfter);
		}
		health += amount;
		if (amount < 0)
		{
			Keyword_Receive(other, Game.Keyword.NEGATIVE_HEALTH_CHANGE, Mathf.Abs(amount));

		}
	}
}