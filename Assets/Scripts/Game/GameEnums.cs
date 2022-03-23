using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class Game
{
	public enum Keyword
	{
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
		NUTRITION
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
		return original == testKeyword;
	}

}