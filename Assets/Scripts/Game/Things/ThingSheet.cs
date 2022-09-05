﻿using GameEnums;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ThingSheet
{
	public static Thing GetGrass()
	{
		Thing plant = new Item(CATEGORY.PLANT);
		//plant.InitContainer();
		//plant.moduleContainer.Add(Keyword.FOOD_VEGI, 100);
		return plant;
	}
	public static Thing GetBush()
	{
		Thing plant = new Item(CATEGORY.PLANT);
		//plant.resources.Add(Keyword.FOOD_VEGI, 100);
		return plant;
	}
	public static Thing GetRock()
	{
		Thing thing = new Item(CATEGORY.ROCK);
		//thing.Category = CATEGORY.ROCK;
		return thing;
	}
	public static Thing GetReed()
	{
		Thing thing = new Item(CATEGORY.REED);
		return thing;
	}

	internal static Structure GetBed()
	{
		Structure thing = new Structure(CATEGORY.BED, CAIModelSheet.Bed_Single);

		return thing;
	}

	public static Frame GetRoof()
	{
		Frame roof = new Frame(CATEGORY.ROOF);
		//roof.SetCategory(CATEGORY.ROOF);
		return roof;
	}
	public static Frame GetWall()
	{
		Frame roof = new Frame(CATEGORY.WALL);
		//roof.SetCategory(CATEGORY.WALL);
		return roof;
	}
	public static Thing Rabbit(CATEGORY category = CATEGORY.RABBIT)
	{
		Thing thing = new StoryGenerator.World.Things.Actors.Animal(category);

		thing.InitBodyManager();
		thing.InitThingNeedManager();

		//thing.moduleNeeds.AddNeed(new Hunger_Vegi());
		thing.moduleNeeds.AddNeed(Hunger_General.InitSimpleHunger(Keyword.FOOD_VEGI, Hunger_General.HungerResolutionMethodType.PASSIVE));
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

		//thing.SetCategory(CATEGORY.RABBIT);
		return thing;
	}
	public static Thing Human()
	{
		var rabbit = Rabbit(CATEGORY.HUMAN);
		//rabbit.SetCategory(CATEGORY.HUMAN);
		return rabbit;
	}
	public static Thing GetBear()
	{
		Thing thing = new Thing(CATEGORY.BEAR);

		thing.InitBodyManager();
		thing.InitThingNeedManager();

		//thing.moduleNeeds.AddNeed(new Hunger_Meat());
		thing.moduleNeeds.AddNeed(Hunger_General.InitSimpleHunger(
			Keyword.FOOD_MEAT, Hunger_General.HungerResolutionMethodType.PASSIVE, Hunger_General.HungerResolutionMethodType.HUNT));
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

		//thing.SetCategory(CATEGORY.BEAR);
		return thing;
	}
}
