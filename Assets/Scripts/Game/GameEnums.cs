using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class Game
{
	//First level of category, defines what the thing is
	//Category is required to understand what a thing is, and to sort things
	public enum CATEGORY
	{
		UNDEFINED = 0,


		//Frame
		WALL,
		DOOR,
		FLOOR,
		ROOF,

		//Actors
		PLANT,
		PERSON,
		RABBIT,
		BEAR,
		HUMAN,

		//Furniture categories
		BED,
		CHAIR,
		TABLE,

		ROCK,
		REED,
		GRASS,
		BUSH
	}

	

	public enum Direction
	{
		UP = 0,
		UP_RIGHT,
		RIGHT,
		RIGHT_DOWN,
		DOWN,
		DOWN_LEFT,
		LEFT,
		LEFT_UP
	}

	
	public enum Keyword
	{
		//Types of Thing
		CHAIR,
		BED,
		TABLE,

		FOOD,
		SLEEP,
		STRESS,

		VITAMIN_D,
		VITAMIN_B12,
		VITAMIN_B6,
		VITAMIN_A,
		VITAMIN_C,
		VITAMIN_E,
		VITAMIN_B1_Thiamin,
		VITAMIN_B2_Riboflavin,
		VITAMIN_B3_Niacin,
		VITAMIN_K,
		Calcium,
		Magnesium,
		Potassium,
		Sodium,
		Folate,
		HUNGER,
		FOOD_MEAT,
		FOOD_VEGI,
		STILL,
		MOVED,
		NUTRITION,
		NEGATIVE_HEALTH_CHANGE,
		PAIN,
		GOOD_FEELING, //A general good feeling, incrases thing's mood 
		UNDEFINED,
	}

	//Types of action
	//Indiciated inside body parts to be able to distinguish which body part can do which task
	public enum TaskType { 
		BITE,
		HOWL,
		HOLD,
		PUNCH,
		MOVE
	}

	public static bool IsKeywordCompatible(Keyword original, Keyword testKeyword)
	{
		if (original == Keyword.FOOD && testKeyword == Keyword.FOOD_VEGI)
		{
			return true;
		}
		if (original == Keyword.FOOD && testKeyword == Keyword.FOOD_MEAT)
		{
			return true;
		}
		return original == testKeyword;
	}
	public static bool IsKeywordCompatible(List<Keyword> originals, Keyword testKeyword)
	{
		foreach(var k in originals)
		{
			if (IsKeywordCompatible(k, testKeyword))
			{
				return true;
			}
		}
		return false;
	}

}