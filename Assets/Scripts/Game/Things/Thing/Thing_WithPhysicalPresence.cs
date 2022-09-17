using StoryGenerator.World;
using GameEnums;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ThingWithPhysicalPresence : Thing_Describable
{
	public void GetRelativePosition(
		int x, int y, 
		out int xNew, out int yNew)
	{
		var newPoint = EasyMath.rotate_point(new UnityEngine.Vector2(x, y), (int)this.DirectionFacing * 45);
		xNew = (int)Mathf.RoundToInt(newPoint.x) + this.X_INT;
		yNew = (int)Mathf.RoundToInt(newPoint.y) + this.Y_INT;
	}

	Direction dirFacing;
	public Direction DirectionFacing { get { return this.dirFacing; } }

	/// <summary>
	/// set the facing direction without touching any in-game mechanics used when initializing 
	/// </summary>
	public ThingWithPhysicalPresence SetFacingDirection(Direction dir)
	{
		this.dirFacing = dir;
		return this;
	}

	public ThingWithPhysicalPresence(ThingCategory category) : base(category)
	{
	}

	public List<DEL_CAN_FACE> OnCanFace = new List<DEL_CAN_FACE>();
	public List<DEL_FACE> OnFace = new List<DEL_FACE>();

	public virtual bool Face(World world, Direction direction)
	{
		if (direction == this.dirFacing)
		{
			return true;
		}
		if (!canFace(world, direction))
		{
			return false;
		}


		for (int i = 0; i < OnFace.Count; i++)
		{
			OnFace[i](world, this, this.dirFacing, direction);
		}
		this.dirFacing = direction;
		return true;
	}

	public virtual bool canFace(World world, Direction direction)
	{
		for (int i = 0; i < OnCanFace.Count; i++)
		{
			if (!OnCanFace[i](world, this, direction))
			{
				return false;
			}
		}
		return true;
	}
}

