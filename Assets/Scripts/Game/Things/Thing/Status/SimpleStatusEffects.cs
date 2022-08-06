using StoryGenerator.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryGenerator.Status
{

	public class Famished : StatusBase
	{

		public Famished() : base(
			Type.FAMISHED,
			"Famished",
			"Feeling extreme hunger.\n" +
			"It will seek to consume-digest any detectable nutritional things.",
			10)
		{

		}
	}

	public class NewBeginning : StatusBase
	{

		public NewBeginning() : base(
			Type.NEW_BEGINNING,
			"New Beginning",
			"Recently started a new beginning. Feeling extremely hopeful.\n" +
			"It will feel constant stream of joy.",
			10)
		{

		}
		public override void Update(World.World world, Thing thing, float timeElapsedTick)
		{
			base.Update(world, thing, timeElapsedTick);
		}
	}

}