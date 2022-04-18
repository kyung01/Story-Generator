using System.Collections;
using UnityEngine;
using StoryGenerator.World;
using System.Collections.Generic;
using System;

[Serializable]
public partial class Thing
{
	static float ZEROf = 0.01f;

	public TYPE type = TYPE.UNDEFINED;

	bool isValid = true;

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
		initCarryingFunctionality();
		this.thingActManager = new ThingActionManager();
		this.type = TYPE.UNDEFINED;
		this.x = 0;
		this.y = 0;
	}
	public Thing()
	{
		baseInit();

	}
	public Thing (TYPE type = TYPE.UNDEFINED , float x = 0, float y=0)
	{
		baseInit();
		this.type = type;
		this.x = x;
		this.y = y;
	}
	public Thing SetType(TYPE type)
	{
		this.type = type;
		return this;
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

	public Vector2 XY_Int
	{
		get
		{
			return new Vector2(Mathf.RoundToInt(X), Mathf.RoundToInt(Y));
		}
	}

	public bool MoveTo(Vector2 position)
	{
		return this.MoveTo(position.x, position.y);
	}
	public bool MoveTo(float x, float y)
	{
		if (IsBeingCarried)
		{
			return false;
		}
		this.XY = new Vector2(x, y);
		return true;
	}

	public virtual void Init(World world)
	{

	}
	public delegate void DEL_POSITION_INDEX_CHANGED(Thing thing, int xBefore, int yBefore, int xNew, int yNew);
	public delegate void DEL_POSITION_CHANGED(Thing thing, float xBefore, float yBefore, float xNew, float yNew);
	public List<DEL_POSITION_INDEX_CHANGED> OnPositionIndexChanged = new List<DEL_POSITION_INDEX_CHANGED>();
	public List<DEL_POSITION_CHANGED> OnPositionChanged = new List<DEL_POSITION_CHANGED>();

	public void SetPosition(float xValue, float yValue)
	{
		float xOld = this.x;
		float yOld = this.y;

		//Debug.Log("SetPosition");
		int xBefore = Mathf.RoundToInt(this.x);
		int yBefore = Mathf.RoundToInt(this.y);

		this.x = xValue;
		this.y = yValue;

		int xNew = Mathf.RoundToInt(xValue);
		int yNew = Mathf.RoundToInt(yValue);

		if(xBefore!= xNew || yBefore != yNew)
		{
			//position is chagned 
			//Debug.Log("SetPosition Changed");
			for (int i = 0; i< OnPositionIndexChanged.Count; i++)
			{
				OnPositionIndexChanged[i](this, xBefore, yBefore, xNew, yNew);
			}
		}

		for (int i = 0; i < OnPositionChanged.Count; i++)
		{
			OnPositionChanged[i](this, xOld,yOld, xValue, yValue);
		}
	}
	public void SetPosition(Vector2 vec2)
	{
		this.x = vec2.x;
		this.y = vec2.y;
	}

	public delegate void DEL_RECEIVE_KEYWORD(Thing me , Thing giver, Game.Keyword keyword, float amount);
	public List<DEL_RECEIVE_KEYWORD> OnReceiveKeyword = new List<DEL_RECEIVE_KEYWORD>();
	//Giver game me keyword of X amount
	public virtual void ReceiveKeyword(Thing giver, Game.Keyword keyword, float amount)
	{
		for(int i = 0; i< OnReceiveKeyword.Count; i++)
		{
			OnReceiveKeyword[i](this ,giver, keyword, amount);
		}

	}

	/// <summary>
	/// My keyword is being taken from me
	/// </summary>
	public virtual float TakenKeyword(Game.Keyword keywordToRequest, float requestedAmount)
	{
		var availableKeywords = this.GetKeywords();
		if (!availableKeywords.ContainsKey(keywordToRequest)) return 0;
		float givenAmount = Mathf.Min(requestedAmount, availableKeywords[keywordToRequest]);
		availableKeywords[keywordToRequest] -= givenAmount;
		if (availableKeywords[keywordToRequest] < ZEROf)
		{
			availableKeywords.Remove(keywordToRequest);
		}
		return givenAmount;
	}

	public delegate void DEL_CONSUME_KEYWORD(Game.Keyword keyword, float amount);
	public List<DEL_CONSUME_KEYWORD> OnConsumeKeyword = new List<DEL_CONSUME_KEYWORD>();

	public virtual void ConsumeKeyword(Game.Keyword keyword, float amount)
	{
		//Debug.Log("ConsumeKeyword " + keyword + " " + amount);
		for (int i = 0; i < OnConsumeKeyword.Count; i++)
		{
			OnConsumeKeyword[i](keyword, amount);
		}
	}


	public virtual Dictionary<Game.Keyword, float> GetKeywords()
	{
		Dictionary<Game.Keyword, float> d = new Dictionary<Game.Keyword, float>();
		return d;
	}

	/// Hunters need to check if the target has X when they become unconscious 
	public virtual Dictionary<Game.Keyword, float> GetKeywordsForHunter()
	{
		Dictionary<Game.Keyword, float> d = new Dictionary<Game.Keyword, float>();
		return d;
	}


	public virtual void Update(World world, float timeElapsed)
	{
		this.TAM.Update(world, this, timeElapsed);

	}
}