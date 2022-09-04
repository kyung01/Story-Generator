using System.Collections;
using UnityEngine;

public class GrassRenderer : ThingRenderer
{
	public override void Update()
	{
		base.Update();
		var g = this.thing;
		//if (!g.Category.IsPlant()) return;
		/*
		if (!g.moduleContainer.Contains(Game.Keyword.FOOD_VEGI))
		{
			this.textMesh.text = "0";
			this.meshRenderer.material.color = new Color(1.0f, 0.0f, 1);
			return;
		}
		this.textMesh.text = ""+(int)g.moduleContainer.Get(Game.Keyword.FOOD_VEGI);
		var grassAmount = g.moduleContainer.Get(Game.Keyword.FOOD_VEGI);
		this.meshRenderer.material.color = new Color(1.0f, grassAmount/100.0f,1);
		 */
	}
}