using System.Collections;
using UnityEngine;
using StoryGenerator.World;
using System.Collections.Generic;

public class Thing
{
	public enum TYPE { UNDEFINED, ROCK, GRASS, BUSH,
		REED,
		RABBIT
	}
	public TYPE type = TYPE.UNDEFINED;

	float x, y;
	ThingActionManager thingActManager;
	public ThingActionManager TAM
	{
		get
		{
			return thingActManager;
		}
	}

	void baseInit()
	{
		this.thingActManager = new ThingActionManager();

	}
	public Thing()
	{
		baseInit();
		this.type = TYPE.UNDEFINED;
		this.x = 0;
		this.y = 0;
	}
	public Thing (TYPE type, float x = 0, float y=0)
	{
		baseInit();
		this.type = type;
		this.x = x;
		this.y = y;
	}
	public float X
	{
		get { return this.x; }
	}
	public float Y
	{
		get { return this.y; }
	}

	public Vector2 XY
	{
		set
		{
			SetPosition(value.x, value.y);
		}
		get
		{
			return new Vector2(this.X, this.y);
		}
	}
	public virtual void Init(World world)
	{

	}
	public void SetPosition(float x, float y)
	{
		this.x = x;
		this.y = y;
	}

	public delegate void DEL_RECEIVE_KEYWORD(Game.Keyword keyword, float amount);
	public List<DEL_RECEIVE_KEYWORD> OnReceiveKeyword = new List<DEL_RECEIVE_KEYWORD>();
	
	public virtual void ReceiveKeyword(Game.Keyword keyword, float amount)
	{
		//Debug.Log("Received " + keyword + " " + amount);
		for(int i = 0; i< OnReceiveKeyword.Count; i++)
		{
			OnReceiveKeyword[i](keyword, amount);
		}

	}
	public virtual Dictionary<Game.Keyword, float> GetKeywords()
	{
		Dictionary<Game.Keyword, float> d = new Dictionary<Game.Keyword, float>();
		return d;

	}


	public virtual void Update(World world, float timeElapsed)
	{
		this.TAM.Update(world, this, timeElapsed);

	}
}