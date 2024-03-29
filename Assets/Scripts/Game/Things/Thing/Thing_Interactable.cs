﻿
using GameEnums;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Interactor {
	public Thing_Interactable thingInteractable;
	public InteractorType type;


	public Interactor(Thing_Interactable thing, InteractorType type)
	{
		this.thingInteractable = thing;
		this.type = type;

	}
	public static implicit operator Thing(Interactor interactor) => interactor.thingInteractable;
}


public class Thing_Interactable : Thing
{
	public Thing_Interactable(ThingCategory category) : base(category)
	{

	}

	//Thing carrying me
	//Thing_Interactable	interactor = null;

	Interactor interactor = null;

	public bool IsInteractor(Thing thing)
	{
		return interactor != null && interactor.thingInteractable == thing;
	}
	//InteractorType		interactorType = InteractorType.NONE;
	// Interactor Interactor { get { return interactor; } }
	
	public virtual float GetInteractRange()
	{
		return 0.5f;
	}

	public virtual bool RequestUnInteract(Thing_Interactable thing)
	{
		//Thing asked me to begin uninteracting with them
		return true;
	}
	
	public virtual bool ResolveInteractor()
	{
		if (this.interactor == null) return true;
		UnityEngine.Debug.LogError(this + "ResolveInteractor Called");
		if (this.interactor.thingInteractable.RequestUnInteract(this))
		{
			UnityEngine.Debug.LogError(this + "ResolveInteractor S");
			this.interactor = null;
			return true;

		}
		UnityEngine.Debug.LogError(this + "ResolveInteractor F");
		return false;

	}
	public void SetInteractor(Thing_Interactable thing, InteractorType interactorType)
	{
		this.interactor = new Interactor(thing, interactorType);
	}

	public bool IsBeingInteracted
	{
		get { return interactor != null; }
	}
	public bool IsBeingCarried
	{
		get { return interactor != null && interactor.type == InteractorType.CARRIER; }
	}

	internal static bool Is(Thing thing)
	{
		throw new System.NotImplementedException();
	}



	/*
	private void hdrUpdateCarryingThingsPositions(Thing thing, float xBefore, float yBefore, float xNew, float yNew)
	{
		for (int i = 0; i < thingsIAmCarrying.Count; i++)
		{
			thingsIAmCarrying[i].XY = this.XY;
		}
	}
	 * */



	/*
	public void InitCarryingFunctionality()
	{
		this.OnPositionChanged.Add(hdrUpdateCarryingThingsPositions);
	}

	private void dropCarryingThing(Thing box)
	{
		thingsIAmCarrying.Remove(box);
		box.freeFromCarrier();
	}
	*/


	/*
	public void Drop()
	{
		for (int i = thingsIAmCarrying.Count - 1; i >= 0; i--)
		{
			dropCarryingThing(thingsIAmCarrying[i]);
		}
	}

	public int CountAllCarryingThings()
	{
		int num = 0;
		num += thingsIAmCarrying.Count;
		foreach (Thing t in thingsIAmCarrying)
		{
			num += t.CountAllCarryingThings();
		}
		return num;
	}

	public bool AreYouCarrying(Thing thing)
	{
		foreach (Thing t in thingsIAmCarrying)
		{
			if (t == thing) return true;
		}
		return false;
	}
	 * */
	/*
	public bool IsCarrying
	{
		get
		{
			return thingsIAmCarrying.Count != 0;

		}
	}
	 * */

	/*
	public virtual void Carry(Thing thingToCarry)
	{
		thingsIAmCarrying.Add(thingToCarry);
		thingToCarry.thingCarryingThis = this;
		thingToCarry.XY = this.XY;


	}
	*/


}