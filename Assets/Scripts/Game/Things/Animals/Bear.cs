using System.Collections;
using UnityEngine;

public class Bear : ThingWithNeeds_ShouldBeJust_ThingWithBody
{
	public Bear()
	{
		addNeed(new Hunger_Meat());
		addNeed(new Wander());

		bodyOld.addBody(new MotionDemander());
		bodyOld.addBody(new PainCreator());
		bodyOld.addBody(new Mouth());

		var meatBody = new MeatBody();
		var stomach = new Stomach();
		stomach.addNutrtionBody(meatBody);
		bodyOld.addBody(meatBody);
		bodyOld.addBody(stomach);

		this.type = TYPE.BEAR;
	}
}