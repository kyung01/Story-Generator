using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Hunger_Meat : Need
{
	public Hunger_Meat()
	{
		this.requiredKeyword = Game.Keyword.FOOD_MEAT;
		this.stressKeyword = Game.Keyword.HUNGER;
		this.name = "Hunger";
		this.explanation = "A general need for food. It can be satisfied only by consuming a meat.";
	}
}
