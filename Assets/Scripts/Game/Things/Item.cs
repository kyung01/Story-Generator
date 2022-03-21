using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Item : Thing
{
	//Items contain x amount of keyword
	Dictionary<Game.Keyword, float> container = new Dictionary<Game.Keyword, float>();
	public override Dictionary<Game.Keyword, float> GetKeywords()
	{
		return container;
	}

}
