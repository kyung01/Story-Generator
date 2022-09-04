using StoryGenerator.World;
using UnityEngine;
using GameEnums;
using System.Collections.Generic;

/// <summary>
/// Thing 
/// </summary>

public class ThingScore {
	public Thing thing;
	public float score;

	public ThingScore(Thing thing, float score)
	{
		this.thing = thing;
		this.score = score;
	}
}
/// <summary>
/// A thing with a home prefers to eat inside their home
/// Otherwise the thing will attempt to find edible things based on its abilities
/// </summary>
public class Brain_GetEdible { 
	public virtual List<Thing> GetEdible(World world, Thing thing)
	{
		return new List<Thing>();
	}
}

/// <summary>
/// An animal will eat whatever it can detect around it based on its physical ability
/// An animal with its owner(a pet) can prefer to eat inside their home I guess? (however this is a behaviour already discussed)
/// Why do I need to separate animal from a human being? because human beings are more complicated
/// How is human being more complicated than an animal? They have dignity and dignity controlls their behaviour 
/// </summary>
public class Animal_GetEdible : Brain_GetEdible {
	public override List<Thing> GetEdible(World world, Thing thing)
	{
		return base.GetEdible(world, thing);
	}
}

/// <summary>
/// A person with a home will eat food inside their house
/// A person without a home (homeless) will eat whatever they can find on street: however it won't eat things like grass
/// A person with a home but at work can eat at work or go outside to buy food and eat there
/// </summary>
public class Person_GetEdible : Brain_GetEdible
{
	public override List<Thing> GetEdible(World world, Thing thing)
	{


		return base.GetEdible(world, thing);
	}

}


public partial class Thing {

	/// <summary>
	/// Thing computes what it can eat
	/// Not all the time will thing properly eat among avilable things to eat
	/// For an example, when a thing is in starvation mode, a thing will eat whatever it can see 
	/// 

	/// 
	/// A person with a home will eat food inside their house
	/// A person without a home (homeless) will eat whatever they can find on street: however it won't eat things like grass
	/// A person with a home but at work can eat at work or go outside to buy food and eat there
	/// 
	/// Let's imagine something else as well...
	/// What if there exists a thing that eats person's thoughts(consciousness) or sights?
	/// It should find available people then perform its necessary actions to eat their consciousness or sights or blood
	/// 
	/// Therefore this function is quite complicated
	/// 
	/// What if a person with a home is outside and is VERY hungry
	/// they should eat whatever they can find around themselves
	/// 
	/// There is a general behaviour such as Animal/Plant
	/// But each different animal or plant can exert a different behaviour 
	/// An eat algorithm for a cat is different from that of human's
	///			
	/// </summary>
	/// 

	internal Brain_GetEdible brainGetEdible;

	public virtual List<Thing> ThinkGetEdible(World world)
	{
		if (brainGetEdible == null) return new List<Thing>();
		return brainGetEdible.GetEdible(world, this);
	}

	public virtual List<ThingScore> ThinkGetEdibleScores(World world, List<Thing> things)
	{
		return new List<ThingScore>();
	}

	/// <summary>
	/// Thing computes resting spot that it can use to rest on
	/// </summary>
	public virtual List<Thing> ThinkGetRestingSpot(World world)
	{
		return new List<Thing>();
	}

}
