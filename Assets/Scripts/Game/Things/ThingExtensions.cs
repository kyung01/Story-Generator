using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class ThingExtensions
{
	static public float GetEatingSpeed(this Thing thing)
	{
		return 50;
	}
	static public float GetEatingDistance(this Thing thing)
	{
		return 1;
	}

	static public Dictionary<Game.TaskType, List<BodyTaskable>> GetBodiesForTask(this Thing thing)
	{
		bool IsThisThing_ThingBodyWithTask = thing is ThingWithBody;

		var dicTaskAvailableBodies = new Dictionary<Game.TaskType, List<BodyTaskable>>();
		if (!IsThisThing_ThingBodyWithTask)
		{
			return dicTaskAvailableBodies;
		}
		var twb = (ThingWithBody)thing;
		sortBodiesForTask(ref dicTaskAvailableBodies, twb.body);


		return dicTaskAvailableBodies;

	}

	static void sortBodiesForTask(ref Dictionary<Game.TaskType, List<BodyTaskable>> dicTaskAvailableBodies, Body body)
	{
		bool isBodyTaskable = body is BodyTaskable;
		if (isBodyTaskable)
		{
			//sort 
			var bt = (BodyTaskable)body;
			sort(ref dicTaskAvailableBodies, bt);
		}
		
		for(int i = 0; i < body.otherBodyParts.Count; i++)
		{
			var otherBody = body.otherBodyParts[i];
			sortBodiesForTask(ref dicTaskAvailableBodies, otherBody);
		}
	}

	private static void sort(ref Dictionary<Game.TaskType, List<BodyTaskable>> dicTaskAvailableBodies, BodyTaskable bt)
	{
		foreach(var task in bt.tasks)
		{
			if (!dicTaskAvailableBodies.ContainsKey(task)) dicTaskAvailableBodies[task] = new List<BodyTaskable>();
			dicTaskAvailableBodies[task].Add(bt);
		}
	}
}