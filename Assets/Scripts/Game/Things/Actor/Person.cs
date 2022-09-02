using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryGenerator.World.Things.Actors{

	public class Person : Actor
	{
		
		public Person( TYPE type ):base(type)
		{
			addSatisfaction(new Satisfaction.Happy());
			addSatisfaction(new Satisfaction.Energy());
			addSatisfaction(new Satisfaction.Social());
			addSatisfaction(new Satisfaction.Fun());
			addSatisfaction(new Satisfaction.Rest());
			addSatisfaction(new Satisfaction.Sleep());
		}
	}

}
