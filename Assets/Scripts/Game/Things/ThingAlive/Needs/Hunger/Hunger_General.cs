using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Hunger_General : Need
{
	public Hunger_General()
	{
		this.requiredKeyword = Game.Keyword.FOOD;
		this.stressKeyword = Game.Keyword.HUNGER;
		this.name = "Hunger";
		this.explanation = "A general need for food. It can be satisfied by any consumable that can be calssified as a food.";
	}
}
