﻿using GameEnums;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Text;
using System.Threading.Tasks;
using StoryGenerator.World;
using StoryGenerator.World.Things.Actors;

public class Worker {
	public ActorBase assginedWorker;
	public Work workIAmDoing;
	public bool IsFree { 
		get {
			return workIAmDoing == null || workIAmDoing.IsFinished;
				}
	}
	public Worker(ActorBase thing)
	{
		this.assginedWorker = thing;
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
	public void AddWorker(Worker worker)
	{
		workers.Add(worker);
	}

	public bool Howl(Thing_Interactable ThingToHowl)
	{
		Debug.Log(this + " HOWL : " + ThingToHowl);
		var work = new Haul(null, ThingToHowl);
		addWork(work);
		return true;
	}
	

	internal void Update( World world, float timeElapsed)
	{
		for(int i = 0; i< works.Count; i++)
		{
			var work = works[i];
			//Debug.Log("Updating Work " + (1 + i) + " / " + works.Count + " Worker status " + work.IsWorkerAssigned);
			if (!work.IsWorkerAssigned)
			{
				assignWorker(work);
			}
			if (!work.IsWorkerAssigned)
			{
				//Debug.Log("Updating Work continue " + (1 + i) + " / " + works.Count);
				continue;
			}
			work.Update(world, timeElapsed);
		}
		for (int i = works.Count - 1; i >= 0; i--)
		{
			if (works[i].IsFinished)
			{
				works.RemoveAt(i);
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
				//Debug.Log("Assinging Worker " + (1+i)+ " / " + workers.Count);
				work.assignedWorker = worker.assginedWorker;
				worker.workIAmDoing = work;
				return;
			}
		}
	}
}