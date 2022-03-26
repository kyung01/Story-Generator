using System.Collections;
using UnityEngine;

public class Bear : ThingAlive
{
	public Bear()
	{
		addNeed(new Hunger_Meat());
		addNeed(new Wander());

		body.addBody(new MotionDemander());
		body.addBody(new PainCreator());
		body.addBody(new Mouth());

		var meatBody = new MeatBody();
		var stomach = new Stomach();
		stomach.addNutrtionBody(meatBody);
		body.addBody(meatBody);
		body.addBody(stomach);

		this.type = TYPE.BEAR;
	}
}