﻿using StoryGenerator.World;
using GameEnums;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoryGenerator.World.Things.Actors;

public static class ThingExtensions
{
	public static void Raise(this List<Thing.DEL_UPDATE> list, World world, Thing thing, float timeElapsed)
	{
		for (int i = 0; i < list.Count; i++)
		{
			list[i](world, thing, timeElapsed);
		}
	}
	static public bool Contains(this List<KeywordInformation> list, Keyword keyword)
	{
		foreach (var info in list)
		{
			if (info.keyword == keyword) return true;

		}
		return false;
	}
	static public bool Contains(this List<KeywordInformation> list, Keyword keyword, KeywordInformation.State state)
	{
		foreach (var info in list)
		{
			if (info.keyword == keyword && info.state == state) return true;

		}
		return false;
	}

	static public float Get(this List<KeywordInformation> list, Keyword keyword)
	{
		float amount = 0;
		foreach (var info in list)
		{
			if (info.keyword == keyword) amount += info.amount;

		}
		return amount;
	}
	static public float GetEatingSpeed(this Thing thing)
	{
		return 50;
	}
	static public float GetEatingDistance(this Thing thing)
	{
		return 1;
	}

	static public Dictionary<TaskType, List<BodyTaskable>> GetBodiesForTask(this Thing thing)
	{
		bool IsThisThing_ThingBodyWithTask =( thing is ActorBase);

		var dicTaskAvailableBodies = new Dictionary<TaskType, List<BodyTaskable>>();
		if (!IsThisThing_ThingBodyWithTask)
		{
			//UnityEngine.Debug.Log( " GetBodiesForTask :: Thing doesn't have a body");
			//Cnannot proceed because thing is not a body with a task
			return dicTaskAvailableBodies;
		}
		//var twb = (ThingWithBody)thing;
		sortBodiesForTask(ref dicTaskAvailableBodies, (thing as ActorBase) .moduleBody.MainBody);
		//UnityEngine.Debug.Log(" GetBodiesForTask :: " + dicTaskAvailableBodies.Count);


		return dicTaskAvailableBodies;

	}

	static void sortBodiesForTask(ref Dictionary<TaskType, List<BodyTaskable>> dicTaskAvailableBodies, BodyBase body)
	{
		bool isBodyTaskable = body is BodyTaskable;
		if (isBodyTaskable)
		{
			//sort 
			var bt = (BodyTaskable)body;
			//UnityEngine.Debug.Log("Body is taskable " + bt);
			sort(ref dicTaskAvailableBodies, bt);
		}
		else
		{

			//UnityEngine.Debug.Log("Body is not taskable " + body);
		}
		
		for(int i = 0; i < body.otherBodyParts.Count; i++)
		{
			var otherBody = body.otherBodyParts[i];
			sortBodiesForTask(ref dicTaskAvailableBodies, otherBody);
		}
	}

	private static void sort(ref Dictionary<TaskType, List<BodyTaskable>> dicTaskAvailableBodies, BodyTaskable bt)
	{
		//UnityEngine.Debug.Log("Sort " + bt.tasks.Count);
		foreach(var task in bt.tasks)
		{
			if (!dicTaskAvailableBodies.ContainsKey(task)) dicTaskAvailableBodies[task] = new List<BodyTaskable>();
			dicTaskAvailableBodies[task].Add(bt);
		}
	}
}