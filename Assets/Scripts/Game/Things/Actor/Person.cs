using GameEnums;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryGenerator.World.Things.Actors{

	public class Person : ActorBase
	{
		
		public Person( ThingCategory type ):base(type)
		{
			moduleNeeds.AddNeed(new NeedSleepHere(this));

			addNeed(new Need.Happy());
			addNeed(new Need.Energy());
			addNeed(new Need.Social());
			addNeed(new Need.Fun());
			addNeed(new Need.Rest());
			//addSatisfaction(new Satisfaction.Sleep());
		}
	}

}
