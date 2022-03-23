﻿using System.Collections;
using UnityEngine;
using StoryGenerator.World;
using System.Collections.Generic;
using System;

[Serializable]
public partial class Thing
{
	static float ZEROf = 0.01f;
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

	public float TakenKeyword(Game.Keyword keywordToRequest, float requestedAmount)
	{
		var availableKeywords = this.GetKeywords();
		if (!availableKeywords.ContainsKey(keywordToRequest)) return 0;
		float givenAmount = Mathf.Min(requestedAmount, availableKeywords[keywordToRequest]);
		availableKeywords[keywordToRequest] -= givenAmount;
		if(availableKeywords[keywordToRequest] < ZEROf)
		{
			availableKeywords.Remove(keywordToRequest);
		}
		return givenAmount;
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
	public List<DEL_RECEIVE_KEYWORD> OnConsumeKeyword = new List<DEL_RECEIVE_KEYWORD>();

	public virtual void ConsumeKeyword(Game.Keyword keyword, float amount)
	{
		//Debug.Log("ConsumeKeyword " + keyword + " " + amount);
		for (int i = 0; i < OnConsumeKeyword.Count; i++)
		{
			OnConsumeKeyword[i](keyword, amount);
		}
	}

	public virtual Dictionary<Game.Keyword, float> ProvideKeyword(Game.Keyword keyword, float amount)
	{
		Dictionary<Game.Keyword, float> d = new Dictionary<Game.Keyword, float>();
		return d;
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