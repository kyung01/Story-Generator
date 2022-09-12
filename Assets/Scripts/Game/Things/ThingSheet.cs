using GameEnums;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ThingSheet
{
	public static Thing GetGrass()
	{
		Thing plant = new Item(ThingCategory.PLANT);
		//plant.InitContainer();
		//plant.moduleContainer.Add(Keyword.FOOD_VEGI, 100);
		return plant;
	}
	public static Thing GetBush()
	{
		Thing plant = new Item(ThingCategory.PLANT);
		//plant.resources.Add(Keyword.FOOD_VEGI, 100);
		return plant;
	}
	public static Thing GetRock()
	{
		Thing thing = new Item(ThingCategory.ROCK);
		//thing.Category = CATEGORY.ROCK;
		return thing;
	}
	public static Thing GetReed()
	{
		Thing thing = new Item(ThingCategory.REED);
		return thing;
	}

	internal static Structure GetBed()
	{
		Structure thing = new Bed_Single();

		return thing;
	}

	public static Frame GetRoof()
	{
		Frame roof = new Frame(ThingCategory.ROOF);
		//roof.SetCategory(CATEGORY.ROOF);
		return roof;
	}
	public static Frame GetWall()
	{
		Frame roof = new Frame(ThingCategory.WALL);
		//roof.SetCategory(CATEGORY.WALL);
		return roof;
	}
	public static Thing Rabbit(ThingCategory category = ThingCategory.RABBIT)
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
		var rabbit = Rabbit(ThingCategory.HUMAN);
		rabbit.moduleNeeds.AddNeed(new NeedSleepHere());
		rabbit.moduleBody.AddBody(new SleepNerve());
		//rabbit.SetCategory(CATEGORY.HUMAN);
		return rabbit;
	}
	public static Thing GetBear()
	{
		Thing thing = new Thing(ThingCategory.BEAR);

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
