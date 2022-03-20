using System.Collections;
using UnityEngine;

public class Thing 
{
	public enum TYPE { UNDEFINED, ROCK,GRASS,BUSH,
		REED
	}
	public TYPE type = TYPE.UNDEFINED;

	public float x, y;

	public Thing()
	{
		this.type = TYPE.UNDEFINED;
		this.x = 0;
		this.y = 0;
	}
	public Thing (TYPE type, float x = 0, float y=0)
	{
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
}