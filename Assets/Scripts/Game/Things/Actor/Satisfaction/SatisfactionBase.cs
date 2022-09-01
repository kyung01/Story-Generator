using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryGenerator.World.Things.Actors.Satisfaction
{
	public class SatisfactionBase
	{
		public enum TYPE
		{
			ENERGY,
			FUN,
			REST,
			SLEEP,
			SOCIAL,
			HAPPY
		}

		TYPE T;

		public SatisfactionBase(TYPE type)
		{
			this.T = type;
		}

		public virtual void ReceiveKeyword(Game.Keyword keyword)
		{

		}

		public void Update(World world, Actor actor, float timeElapsed)
		{

		}

		internal void Init(Actor actor)
		{

		}
	}

}

