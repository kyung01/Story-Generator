using StoryGenerator.World;
using GameEnums;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class ThingWithPhysicalPresence : Thing_Describable
{

	Direction dirFacing;
	public Direction DirectionFacing { get { return this.dirFacing; } }

	/// <summary>
	/// set the facing direction without touching any in-game mechanics used when initializing 
	/// </summary>
	internal void SetFacingDirection(Direction dir)
	{
		this.dirFacing = dir;
	}

	public ThingWithPhysicalPresence(CATEGORY type) : base(type)
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

