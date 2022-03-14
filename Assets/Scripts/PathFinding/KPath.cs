using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class KPath
{
	public int id = 0;
	public float weight = 0;
	public KPath before, after;
	public List<KPath> neighbours = new List<KPath>();
	public List<KPath> neighboursDiagonal = new List<KPath>();
	Vector2 location;
	public Vector2 Location { get { return location; } }


	public static implicit operator Vector2(KPath p) => new Vector2(p.x, p.y);


	public int x
	{
		get { return (int)location.x; }
	}

	public int y
	{
		get { return (int)location.y; }
	}

	public Vector2 vec2
	{
		get
		{
			return new Vector2(x, y);

		}
	}

	int recurGetCount()
	{

		return (this.location == null) ? 0 : 1 +
			(
			(this.after == null) ? 0 : this.after.recurGetCount()
			);
	}


	public int Count
	{
		get
		{
			return recurGetCount();

		}

	}




	public KPath(int x, int y)
	{
		location = new Vector2(x, y);
	}
	public void setBegin(Vector2 location)
	{
		this.location = location;
	}
	public void reset(int newId, float newWeight, KPath newBefore, KPath newAfter)
	{
		this.id = newId;
		this.weight = newWeight;
		this.before = newBefore;
		this.after = newAfter;
	}


}