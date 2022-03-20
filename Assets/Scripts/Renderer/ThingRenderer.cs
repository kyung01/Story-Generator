using System.Collections;
using UnityEngine;

public class ThingRenderer : MonoBehaviour
{
	public static float Z_AXIS_LAYER = -0.001f;
	public Thing thing;
	public MeshRenderer meshRenderer;
	private void Awake()
	{
		meshRenderer = GetComponentInChildren<MeshRenderer>();
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
			case Thing.TYPE.GRASS:
				meshRenderer.material.mainTexture = SPRITE_LIST.Grass;
				break;
			case Thing.TYPE.BUSH:
				meshRenderer.material.mainTexture = SPRITE_LIST.Bush;
				break;
			case Thing.TYPE.REED:
				meshRenderer.material.mainTexture = SPRITE_LIST.Reed;
				break;
			default:
				break;
		}
		this.transform.position = new Vector3(thing.x,thing.y, Z_AXIS_LAYER);

	}
}