using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Hunger_Vegi : Hunger_General
{
	public Hunger_Vegi()
	{
		this.requiredKeyword = Game.Keyword.FOOD_VEGI;
		this.stressKeyword = Game.Keyword.HUNGER;
		this.name = "Hunger";
		this.explanation = "A general need for food. It can be satisfied by consuming vegitable, fruit, or plant matters.";
	}
}
