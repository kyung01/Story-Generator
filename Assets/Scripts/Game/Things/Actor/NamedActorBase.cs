using GameEnums;
using System.Collections;
using UnityEngine;

namespace StoryGenerator.World.Things.Actors
{
	public partial class NamedActorBase : ActorBase
	{
		public string name;


		public NamedActorBase(ThingCategory category) : base(category)
		{
		}
	}
}