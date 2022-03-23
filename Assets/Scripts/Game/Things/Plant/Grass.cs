using StoryGenerator.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Grass : Plant
{
	public Grass()
	{
		this.type = TYPE.GRASS;
		this.resources.Add(Game.Keyword.FOOD_VEGI, 100);

	}
	public override void Init(World world)
	{
		base.Init(world);
	}
}