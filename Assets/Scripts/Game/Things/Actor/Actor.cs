using StoryGenerator.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Actor : Thing
{
	public List<Game.Keyword> foodList = new List<Game.Keyword>();


	public virtual void DoEat(World world)
	{

	}

	public virtual void DoSleep(World world)
	{

	}

	public virtual void DoRest(World world)
	{

	}

	public virtual void DoFun(World world)
	{

	}
}
