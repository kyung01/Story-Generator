using GameEnums;
using StoryGenerator.World.Things.Actors;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ThingSheet
{
	public static Thing GetGrass()
	{
		Thing plant = new Item(ThingCategory.GRASS);
		//plant.InitContainer();
		//plant.moduleContainer.Add(Keyword.FOOD_VEGI, 100);
		return plant;
	}
	public static Thing GetBush()
	{
		Thing plant = new Item(ThingCategory.BUSH);
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

	public static Structure GetBed()
	{
		Structure thing = new Bed_Single();

		return thing;
	}

	public static Structure GetStorage()
	{
		Structure s = new Storage(CAIModelSheet.Storage);
		return s;
	}
	static Item GetEgg()
	{
		return null;
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
		Animal thing = new Animal(category);

		//thing.InitBodyManager();
		//thing.InitThingNeedManager();

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
		Person human  = new StoryGenerator.World.Things.Actors.Person( ThingCategory.HUMAN);

		//human.AddNeed(new NeedSleepHere());
		human.moduleBody.AddBody(new SleepNerve());
		//rabbit.SetCategory(CATEGORY.HUMAN);
		return human;
	}
	public static Thing GetBear()
	{
		Animal thing = new Animal(ThingCategory.BEAR);

		//thing.InitBodyManager();
		//thing.InitThingNeedManager();

		//thing.moduleNeeds.AddNeed(new Hunger_Meat());
		thing.AddNeed(Hunger_General.InitSimpleHunger(
			Keyword.FOOD_MEAT, Hunger_General.HungerResolutionMethodType.PASSIVE, Hunger_General.HungerResolutionMethodType.HUNT));
		thing.AddNeed(new Wander());		

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
