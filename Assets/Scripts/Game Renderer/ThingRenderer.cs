using StoryGenerator.World.Things.Actors;
using System.Collections;
using UnityEngine;

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
			case Thing.CATEGORY.UNDEFINED:
				Debug.LogError(thing + " ThingRenderer->RenderThing->RenderType being undefined " + thing.Category + thing.XY);
				break;
			case Thing.CATEGORY.ROCK:
				meshRenderer.material.mainTexture = SPRITE_LIST.Rock;
				break;
			case Thing.CATEGORY.HUMAN:
				meshRenderer.material.mainTexture = SPRITE_LIST.Human;
				break;
			case Thing.CATEGORY.GRASS:
				meshRenderer.material.mainTexture = SPRITE_LIST.Grass;
				break;
			case Thing.CATEGORY.BUSH:
				meshRenderer.material.mainTexture = SPRITE_LIST.Bush;
				break;
			case Thing.CATEGORY.REED:
				meshRenderer.material.mainTexture = SPRITE_LIST.Reed;
				break;
			case Thing.CATEGORY.RABBIT:
				meshRenderer.material.mainTexture = SPRITE_LIST.Rabbit;
				break;
			case Thing.CATEGORY.BEAR:
				meshRenderer.material.mainTexture = SPRITE_LIST.Bear;
				break;
			case Thing.CATEGORY.WALL:
				break;
			case Thing.CATEGORY.DOOR:
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

	Game.Direction dirFacing = Game.Direction.UP;
	public virtual void Update()
	{
		Vector2 pos = new Vector2(this.transform.position.x, this.transform.position.y);
		pos = Vector2.SmoothDamp(pos, thing.XY, ref speed, SMOOTH_TIME);
		this.transform.position = new Vector3(pos.x, pos.y, Z_AXIS_LAYER);

		textMesh.text = "";
		{
			if(thing.Category == Thing.CATEGORY.HUMAN)
			{
				var thing = (ThingWithPhysicalPresence)this.thing;
				if (thing.Category == Thing.CATEGORY.HUMAN)
				{
					textMesh.text += "" + thing.DirectionFacing + "\n";

				}
				if (thing.Category == Thing.CATEGORY.HUMAN && dirFacing != thing.DirectionFacing)
				{
					dirFacing = thing.DirectionFacing;
					if (thing.DirectionFacing == Game.Direction.UP)
					{
						meshRenderer.material.mainTexture = SPRITE_LIST.facingUp;

					}
					if (thing.DirectionFacing == Game.Direction.UP_RIGHT)
					{
						meshRenderer.material.mainTexture = SPRITE_LIST.facingUpRight;

					}
					if (thing.DirectionFacing == Game.Direction.RIGHT)
					{
						meshRenderer.material.mainTexture = SPRITE_LIST.facingRight;

					}
					if (thing.DirectionFacing == Game.Direction.RIGHT_DOWN)
					{
						meshRenderer.material.mainTexture = SPRITE_LIST.facingRightDown;

					}
					if (thing.DirectionFacing == Game.Direction.DOWN)
					{
						meshRenderer.material.mainTexture = SPRITE_LIST.facingDown;

					}
					if (thing.DirectionFacing == Game.Direction.DOWN_LEFT)
					{
						meshRenderer.material.mainTexture = SPRITE_LIST.facingDownLeft;

					}
					if (thing.DirectionFacing == Game.Direction.LEFT)
					{
						meshRenderer.material.mainTexture = SPRITE_LIST.facingLeft;

					}
					if (thing.DirectionFacing == Game.Direction.LEFT_UP)
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

			if (!(thingAlive.Category == Thing.CATEGORY.RABBIT  || thingAlive.Category == Thing.CATEGORY.BEAR 
				|| thingAlive.Category == Thing.CATEGORY.HUMAN
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
			
			if(thing.moduleNeeds != null)
			{
				for (int i = 0; i < thing.moduleNeeds.needs.Count; i++)
				{
					var n = thing.moduleNeeds.needs[i];
					textMesh.text += n.name + " " + n.fullfillment + " \n";

				}
			}
			
		}
		catch
		{

		}


	}
}