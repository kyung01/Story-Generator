using StoryGenerator.World.Things.Actors;
using UnityEngine;
using GameEnums;

public class ThingRenderer : MonoBehaviour
{
	public static float Z_AXIS_LAYER = -0.001f;
	public static float SMOOTH_TIME = .05f;

	[SerializeField] InWorldTextFeedback PREFAB_IN_WORLD_TEXT_FEEDBACk;
	public SpriteList SPRITE_LIST;

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
		//Debug.Log("I have a " +thing.T);
		switch (thing.Category)
		{
			case ThingCategory.UNDEFINED:
				Debug.LogError(thing + " ThingRenderer->RenderThing->RenderType being undefined " + thing.Category + thing.XY);
				break;
			case ThingCategory.ROCK:
				meshRenderer.material.mainTexture = SPRITE_LIST.Rock;
				break;
			case ThingCategory.HUMAN:
				meshRenderer.material.mainTexture = SPRITE_LIST.Human;
				break;
			case ThingCategory.GRASS:
				meshRenderer.material.mainTexture = SPRITE_LIST.Grass;
				break;
			case ThingCategory.BUSH:
				meshRenderer.material.mainTexture = SPRITE_LIST.Bush;
				break;
			case ThingCategory.REED:
				meshRenderer.material.mainTexture = SPRITE_LIST.Reed;
				break;
			case ThingCategory.RABBIT:
				meshRenderer.material.mainTexture = SPRITE_LIST.Rabbit;
				break;
			case ThingCategory.BEAR:
				meshRenderer.material.mainTexture = SPRITE_LIST.Bear;
				break;
			case ThingCategory.BED:
				meshRenderer.material.mainTexture = SPRITE_LIST.Bed_Single;
				meshRenderer.gameObject.transform.localScale = new Vector3(1,2,1);
				meshRenderer.gameObject.transform.localPosition = new Vector3(0, .5f, 0); ;
				break;
			case ThingCategory.EGG:
				meshRenderer.material.mainTexture = SPRITE_LIST.Egg;
				break;
			case ThingCategory.WALL:
				break;
			case ThingCategory.DOOR:
				break;
			default:
				break;
		}

		if(thing is ThingWithPhysicalPresence)
		{
			Debug.Log("ThingRenderer " + ((ThingWithPhysicalPresence)thing).DirectionFacing);
			this.gameObject.transform.rotation = Quaternion.Euler(0, 0, -45 * (int)((ThingWithPhysicalPresence)thing).DirectionFacing );
		}

		/*
		if(thing is ThingDestructable)
		{
			var tD = (ThingDestructable)thing;
			tD.OnHealthChanged.Add(hdrHealthChanged);
		}
		 * */
		this.transform.position = new Vector3(thing.X,thing.Y, Z_AXIS_LAYER);

	}

	private void hdrHealthChanged(Thing me, Thing other, float dealtChange, float healthBefore, float healthAfter)
	{
		var effect = Instantiate(PREFAB_IN_WORLD_TEXT_FEEDBACk);
		effect.transform.position = this.transform.position;
		effect.text.text = "HP " + healthBefore + ((dealtChange > 0) ? " + " : " - ") + Mathf.Abs(dealtChange) + " = " + healthAfter;
	}

	Direction dirFacing = Direction.UP;
	public virtual void Update()
	{
		Vector2 pos = new Vector2(this.transform.position.x, this.transform.position.y);
		pos = Vector2.SmoothDamp(pos, thing.XY, ref speed, SMOOTH_TIME);
		this.transform.position = new Vector3(pos.x, pos.y, Z_AXIS_LAYER);

		textMesh.text = "";
		{
			if(thing.Category == ThingCategory.HUMAN)
			{
				var thing = (ThingWithPhysicalPresence)this.thing;
				if (thing.Category == ThingCategory.HUMAN)
				{
					textMesh.text += "" + thing.DirectionFacing + "\n";

				}
				if (thing.Category == ThingCategory.HUMAN && dirFacing != thing.DirectionFacing)
				{
					dirFacing = thing.DirectionFacing;
					if (thing.DirectionFacing == Direction.UP)
					{
						meshRenderer.material.mainTexture = SPRITE_LIST.facingUp;

					}
					if (thing.DirectionFacing == Direction.UP_RIGHT)
					{
						meshRenderer.material.mainTexture = SPRITE_LIST.facingUpRight;

					}
					if (thing.DirectionFacing == Direction.RIGHT)
					{
						meshRenderer.material.mainTexture = SPRITE_LIST.facingRight;

					}
					if (thing.DirectionFacing == Direction.RIGHT_DOWN)
					{
						meshRenderer.material.mainTexture = SPRITE_LIST.facingRightDown;

					}
					if (thing.DirectionFacing == Direction.DOWN)
					{
						meshRenderer.material.mainTexture = SPRITE_LIST.facingDown;

					}
					if (thing.DirectionFacing == Direction.DOWN_LEFT)
					{
						meshRenderer.material.mainTexture = SPRITE_LIST.facingDownLeft;

					}
					if (thing.DirectionFacing == Direction.LEFT)
					{
						meshRenderer.material.mainTexture = SPRITE_LIST.facingLeft;

					}
					if (thing.DirectionFacing == Direction.LEFT_UP)
					{
						meshRenderer.material.mainTexture = SPRITE_LIST.facingLeftUp;

					}
				}

			}

		}
		try
		{

			var thingAlive = thing;
			if (thingAlive == null) return;

			if (!(thingAlive.Category == ThingCategory.RABBIT  || thingAlive.Category == ThingCategory.BEAR 
				|| thingAlive.Category == ThingCategory.HUMAN
				)) return;
			if(thing is ActorBase)
			{
				var actor = (ActorBase)thing;
				for (int i = 0; i < actor.TAM.actions.Count; i++)
				{
					var n = actor.TAM.actions[i];
					if (n is MoveTo)
					{

						textMesh.text += n.name + " " + ((MoveTo)n).NextDestinationXY + " \n";
					}
					else
					{
						textMesh.text += n.name + " \n";

					}

				}
			}

			/*
			if(thing.moduleNeeds != null)
			{
				for (int i = 0; i < thing.moduleNeeds.needs.Count; i++)
				{
					var n = thing.moduleNeeds.needs[i];
					textMesh.text += n.name + " " + n.fullfillment + " \n";

				}
			}
			 * */

		}
		catch
		{

		}


	}
}