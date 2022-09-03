using StoryGenerator.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class ThingWithPhysicalPresence : Thing_Describable
{

	Game.Direction dirFacing;
	public Game.Direction DirectionFacing { get { return this.dirFacing; } }

	public ThingWithPhysicalPresence(Thing.CATEGORY type) : base(type)
	{
	}

	public List<DEL_CAN_FACE> OnCanFace = new List<DEL_CAN_FACE>();
	public List<DEL_FACE> OnFace = new List<DEL_FACE>();

	public virtual bool Face(World world, Game.Direction direction)
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

	public virtual bool canFace(World world, Game.Direction direction)
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

