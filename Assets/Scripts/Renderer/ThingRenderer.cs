using System.Collections;
using UnityEngine;

public class ThingRenderer : MonoBehaviour
{
	public static float Z_AXIS_LAYER = -0.001f;
	public static float SMOOTH_TIME = .2f;
	public Thing thing;
	public MeshRenderer meshRenderer;
	Vector2 speed = new Vector2();
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
			case Thing.TYPE.RABBIT:
				meshRenderer.material.mainTexture = SPRITE_LIST.Rabbit;
				break;
			default:
				break;
		}
		this.transform.position = new Vector3(thing.X,thing.Y, Z_AXIS_LAYER);

	}
	private void Update()
	{
		Vector2 pos = new Vector2(this.transform.position.x, this.transform.position.y);
		pos = Vector2.SmoothDamp(pos, thing.XY, ref speed, SMOOTH_TIME);
		this.transform.position = new Vector3(pos.x, pos.y, Z_AXIS_LAYER);
	}
}