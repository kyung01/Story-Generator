using System.Collections;
using UnityEngine;

public class ThingRenderer : MonoBehaviour
{
	public static float Z_AXIS_LAYER = -0.001f;
	public static float SMOOTH_TIME = .05f;

	[SerializeField] InWorldTextFeedback PREFAB_IN_WORLD_TEXT_FEEDBACk;
	public Thing thing;
	public MeshRenderer meshRenderer;
	public TMPro.TextMeshPro textMesh;
	Vector2 speed = new Vector2();

	private void Awake()
	{
		meshRenderer = GetComponentInChildren<MeshRenderer>();
		textMesh = GetComponentInChildren<TMPro.TextMeshPro>();
		textMesh.text = "";
	}

	public void RenderThing(Thing thing, SpriteList SPRITE_LIST)
	{
		this.thing = thing;
		switch (thing.type)
		{
			case Thing.TYPE.UNDEFINED:
				break;
			case Thing.TYPE.ROCK:
				meshRenderer.material.mainTexture = SPRITE_LIST.Rock;
				break;
			case Thing.TYPE.HUMAN:
				meshRenderer.material.mainTexture = SPRITE_LIST.Human;
				break;
			case Thing.TYPE.GRASS:
				meshRenderer.material.mainTexture = SPRITE_LIST.Grass;
				break;
			case Thing.TYPE.BUSH:
				meshRenderer.material.mainTexture = SPRITE_LIST.Bush;
				break;
			case Thing.TYPE.REED:
				meshRenderer.material.mainTexture = SPRITE_LIST.Reed;
				break;
			case Thing.TYPE.RABBIT:
				meshRenderer.material.mainTexture = SPRITE_LIST.Rabbit;
				break;
			case Thing.TYPE.BEAR:
				meshRenderer.material.mainTexture = SPRITE_LIST.Bear;
				break;
			case Thing.TYPE.WALL:
				break;
			case Thing.TYPE.DOOR:
				break;
			default:
				break;
		}

		if(thing is ThingDestructable)
		{
			var tD = (ThingDestructable)thing;
			tD.OnHealthChanged.Add(hdrHealthChanged);
		}
		this.transform.position = new Vector3(thing.X,thing.Y, Z_AXIS_LAYER);

	}

	private void hdrHealthChanged(Thing me, Thing other, float dealtChange, float healthBefore, float healthAfter)
	{
		var effect = Instantiate(PREFAB_IN_WORLD_TEXT_FEEDBACk);
		effect.transform.position = this.transform.position;
		effect.text.text = "HP " + healthBefore + ((dealtChange > 0) ? " + " : " - ") + Mathf.Abs(dealtChange) + " = " + healthAfter;
	}

	public virtual void Update()
	{
		Vector2 pos = new Vector2(this.transform.position.x, this.transform.position.y);
		pos = Vector2.SmoothDamp(pos, thing.XY, ref speed, SMOOTH_TIME);
		this.transform.position = new Vector3(pos.x, pos.y, Z_AXIS_LAYER);

		try
		{

			var thingAlive = (ThingAlive)thing;
			if (thingAlive == null) return;

			if (!(thingAlive.type == Thing.TYPE.RABBIT  || thingAlive.type == Thing.TYPE.BEAR || thingAlive.type == Thing.TYPE.HUMAN)) return;
			textMesh.text = "";
			for (int i = 0; i < thing.TAM.actions.Count; i++)
			{
				var n = thing.TAM.actions[i];
				textMesh.text += n.name  +" \n";

			}
			for (int i = 0; i < thingAlive.needs.Count; i++)
			{
				var n = thingAlive.needs[i];
				textMesh.text += n.name + " " + n.demand + " \n";

			}
		}
		catch
		{

		}


	}
}