using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Text;
using System.Threading.Tasks;
using StoryGenerator.World;

public class Worker {
	public Thing thing;
	public Work workIAmDoing;
	public bool IsFree { 
		get {
			return workIAmDoing == null || workIAmDoing.IsFinished;
				}
	}
}

public class WorkManagaer
{
	List<Worker> workers = new List<Worker>();
	List<Work> works = new List<Work>();

	public WorkManagaer()
	{
	}
	void addWork(Work work)
	{
		works.Add(work);
	}
	public bool Howl(Thing ThingToHowl, Vector2 position)
	{
		/*
		for(int i = 0; i< works.Count; i++)
		{
			var w = works[i];
			if(w is Howl)
			{
				var h = (Howl)w;
				bool isTheSamePosition = 
					Mathf.RoundToInt(h.positionToHowlTo.x) == Mathf.RoundToInt(position.x) &&
					Mathf.RoundToInt(h.positionToHowlTo.y) == Mathf.RoundToInt(position.y);
				if (isTheSamePosition)
				{
					return false;
				}
			}
		}
		*/
		var work = new Howl(null, ThingToHowl, position);
		addWork(work);
		return true;
	}

	internal void Update(World world, float timeElapsed)
	{
		for(int i = 0; i< works.Count; i++)
		{
			var work = works[i];
			if (!work.IsWorkerAssigned)
			{
				assignWorker(work);
			}
			if (!work.IsWorkerAssigned)
			{
				continue;
			}
			if (work.assignedWorker.TAM.IsIdl)
			{
				works[i].Update(world, timeElapsed);

			}
		}
	}

	private void assignWorker(Work work)
	{
		for (int i = 0; i < workers.Count; i++)
		{
			var worker = workers[i];
			if (worker.IsFree)
			{
				work.assignedWorker = worker.thing;
				worker.workIAmDoing = work;
			}
		}
	}
}