using System.Collections;
using UnityEngine;
using StoryGenerator.World;
using System.Collections.Generic;
using GameEnums;
 
public partial class Thing
{

	public delegate void						DEL_UPDATE(World world, Thing thing, float timeElapsed);	
	public delegate List<KeywordInformation>	DEL_GET_KEYWORDS();
	public delegate float						DEL_TAKEN_KEYWORD(Keyword keywordToRequest, float requestedAmount);

	public delegate void DEL_POSITION_INDEX_CHANGED	(Thing thing, int xBefore, int yBefore, int xNew, int yNew);
	public delegate void DEL_POSITION_CHANGED		(Thing thing, float xBefore, float yBefore, float xNew, float yNew);

	public delegate void DEL_RECEIVE_KEYWORD(Thing me, Thing giver, Keyword keyword, float amount);
	//public delegate void DEL_CONSUME_KEYWORD(Thing me, Thing giver, Keyword keyword, float amount);

	public List<DEL_UPDATE>			OnUpdate		= new List<DEL_UPDATE>();
	public List<DEL_GET_KEYWORDS>	OnGetKeywords	= new List<DEL_GET_KEYWORDS>();
	public List<DEL_TAKEN_KEYWORD>	OnTakenKeyword	= new List<DEL_TAKEN_KEYWORD>();

	public List<DEL_POSITION_INDEX_CHANGED> OnPositionIndexChanged = new List<DEL_POSITION_INDEX_CHANGED>();
	public List<DEL_POSITION_CHANGED> OnPositionChanged = new List<DEL_POSITION_CHANGED>();

	public List<DEL_RECEIVE_KEYWORD> OnReceiveKeyword = new List<DEL_RECEIVE_KEYWORD>();


	static float ZEROf = 0.01f;

	CATEGORY category;



	bool isValid = true;

	float x, y;

	#region properties

	public CATEGORY Category { get { return this.category; } 
		//set { this.category = value; } 
	}


	public float X
	{
		get { return this.x; }
	}

	public float Y
	{
		get { return this.y; }
	}

	public int X_INT
	{
		get { return Mathf.RoundToInt(this.x); }
	}

	public int Y_INT
	{
		get { return Mathf.RoundToInt(this.y); }
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

	#endregion


	void baseInit()
	{
		this.x = 0;
		this.y = 0;
	}
	
	public Thing(CATEGORY type )
	{
		this.category = type;
		baseInit();

	}
	public Thing (CATEGORY type  , float x = 0, float y=0)
	{
		baseInit();
		this.category = type;
		this.x = x;
		this.y = y;
	}
	



	public delegate bool DEL_CAN_FACE(World world, Thing me, Direction direction);
	public delegate void DEL_FACE(World world, Thing me, Direction dirOld, Direction dirNow);

	

	public virtual void Init(World world)
	{
		if (moduleNeeds != null)
		{
			moduleNeeds.Init(this);
		}

	}
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

	//Giver game me keyword of X amount
	public virtual void		Keyword_Receive(Thing giver, Keyword keyword, float amount)
	{
		for(int i = 0; i< OnReceiveKeyword.Count; i++)
		{
			OnReceiveKeyword[i](this ,giver, keyword, amount);
		}

	}

	public virtual float	Keyword_Taken(Keyword keywordToRequest, float requestedAmount)
	{
		float remainingAmountToTakeFromMe = requestedAmount;
		//Debug.Log("TakenKeywords Called " + keywordToRequest + " for " + requestedAmount);
		//var availableKeywords = this.GetKeywords();
		for(int i = 0; i < OnTakenKeyword.Count; i++)
		{
			var takenAmount = OnTakenKeyword[i](keywordToRequest, remainingAmountToTakeFromMe);
			remainingAmountToTakeFromMe -= takenAmount;
			if (remainingAmountToTakeFromMe == 0)
			{
				break;
			}
		}
		return requestedAmount - remainingAmountToTakeFromMe;

	}


	public List<KeywordInformation> GetKeywords()
	{
		var keywords = new List<KeywordInformation>();
		for(int i = 0; i < OnGetKeywords.Count; i++)
		{
			var otherKeywords = OnGetKeywords[i]();
			if(otherKeywords == null) continue;
			foreach(var otherKW in otherKeywords)
			{
				bool foundCorrectOne = false;
				KeywordInformation info = null;
				foundCorrectOne = keywords.Contains(otherKW.keyword, otherKW.state);
				if (foundCorrectOne)
				{
					info.Combine(otherKW);
				}
				else
				{
					keywords.Add(otherKW);
				}
			}			
		}
		/*
		if(keywords.Count != 0)
		{
			Debug.Log("GetKeywords DEBUG " + keywords.Count);
			foreach (var info in keywords)
			{
				Debug.Log(info.keyword + " " + info.state + " " + info.amount);
			}

		}
		 * */

		return keywords;
	}

	public virtual void Update(World world, float timeElapsed)
	{
		OnUpdate.Raise(world, this, timeElapsed);

	}


}