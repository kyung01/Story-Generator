using StoryGenerator.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Rabbit : ThingAlive
{
	public Rabbit()
	{
		addNeed(new Hunger_Vegi());
		addNeed(new Wander());
		body.addBody(new MotionDemander());

		var meatBody = new MeatBody();
		var stomach = new Stomach();
		stomach.addNutrtionBody(new MeatBody());
		body.addBody(meatBody);
		body.addBody(stomach);

		this.type = TYPE.RABBIT;
	}
	public override void Init(World world)
	{
		base.Init(world);
	}
	public override void Update(World world, float timeElapsed)
	{
		base.Update(world, timeElapsed);

		//UnityEngine. Debug.Log("RabitUpdate" + " , " + world);
	}
}
