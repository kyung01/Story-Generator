using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ThingSheet
{
	public static Thing GetGrass()
	{
		Thing plant = new Thing(Thing.TYPE.GRASS);
		plant.InitContainer();
		plant.moduleContainer.Add(Game.Keyword.FOOD_VEGI, 100);
		return plant;
	}
	public static Thing GetBush()
	{
		Thing plant = new Thing(Thing.TYPE.BUSH);
		//plant.resources.Add(Game.Keyword.FOOD_VEGI, 100);
		return plant;
	}
	public static Thing GetRock()
	{
		Thing thing = new Thing(Thing.TYPE.ROCK);
		thing.T = Thing.TYPE.ROCK;
		return thing;
	}
	public static Thing GetReed()
	{
		Thing thing = new Thing(Thing.TYPE.REED);
		return thing;
	}

	public static Structure GetRoof()
	{
		Structure roof = new Structure();
		roof.SetType(Thing.TYPE.ROOF);
		return roof;
	}
	public static Structure GetWall()
	{
		Structure roof = new Structure();
		roof.SetType(Thing.TYPE.WALL);
		return roof;
	}
	public static Thing Rabbit()
	{
		Thing thing = new StoryGenerator.World.Things.Actors.Animal( Thing.TYPE.RABBIT );

		thing.InitBodyManager();
		thing.InitThingNeedManager();

		//thing.moduleNeeds.AddNeed(new Hunger_Vegi());
		thing.moduleNeeds.AddNeed(Hunger_General.InitSimpleHunger(Game.Keyword.FOOD_VEGI, Hunger_General.HungerResolutionMethodType.PASSIVE));
		thing.moduleNeeds.AddNeed(new Wander());

		var meatBody = new MeatBody();
		var stomach = new Stomach();
		stomach.addNutrtionBody(meatBody);

		thing.moduleBody.AddBody(new Eye().SetName("Left Eye"));
		thing.moduleBody.AddBody(new Eye().SetName("Right Eye"));
		thing.moduleBody.AddBody(new MotionDemander());
		thing.moduleBody.AddBody(new PainCreator());
		thing.moduleBody.AddBody(new PainReactionModelBody());
		thing.moduleBody.AddBody(new Mouth());
		//thing.moduleBody.AddBody(meatBody);
		//thing.moduleBody.AddBody(stomach);

		thing.SetType(Thing.TYPE.RABBIT);
		return thing;
	}
	public static Thing Human()
	{
		var rabbit = Rabbit();
		rabbit.SetType(Thing.TYPE.HUMAN);
		return rabbit;
	}
	public static Thing GetBear()
	{
		Thing thing = new Thing( Thing.TYPE.BEAR);

		thing.InitBodyManager();
		thing.InitThingNeedManager();

		//thing.moduleNeeds.AddNeed(new Hunger_Meat());
		thing.moduleNeeds.AddNeed(Hunger_General.InitSimpleHunger(
			Game.Keyword.FOOD_MEAT, Hunger_General.HungerResolutionMethodType.PASSIVE, Hunger_General.HungerResolutionMethodType.HUNT));
		thing.moduleNeeds.AddNeed(new Wander());		

		var meatBody = new MeatBody();
		var stomach = new Stomach();
		stomach.addNutrtionBody(meatBody);

		thing.moduleBody.AddBody(new Eye().SetName("Left Eye"));
		thing.moduleBody.AddBody(new Eye().SetName("Right Eye"));
		thing.moduleBody.AddBody(new MotionDemander());
		thing.moduleBody.AddBody(new PainCreator());
		thing.moduleBody.AddBody(new Mouth());
		thing.moduleBody.AddBody(meatBody);
		thing.moduleBody.AddBody(stomach);

		thing.SetType(Thing.TYPE.BEAR);
		return thing;
	}
}
