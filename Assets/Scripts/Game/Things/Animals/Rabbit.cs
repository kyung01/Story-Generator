using StoryGenerator.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Rabbit : ThingWithNeeds_ShouldBeJust_ThingWithBody
{
	public Rabbit()
	{
		addNeed(new Hunger_Vegi());
		addNeed(new Wander());


		bodyOld.addBody(new MotionDemander());
		bodyOld.addBody(new PainCreator());
		bodyOld.addBody(new PainReactionModelBody());

		var meatBody = new MeatBody();
		var stomach = new Stomach();
		stomach.addNutrtionBody(meatBody);
		bodyOld.addBody(meatBody);
		bodyOld.addBody(stomach);

		this.type = TYPE.RABBIT;
	}
}
