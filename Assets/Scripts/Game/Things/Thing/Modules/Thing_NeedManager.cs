using StoryGenerator.World;
using StoryGenerator.World.Things.Actors;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public partial class Thing
{
	ThingNeedManager thingNeedManager;
	public ThingNeedManager moduleNeeds { get { return this.thingNeedManager; } }

	public void InitThingNeedManager()
	{
		thingNeedManager = new ThingNeedManager();
		thingNeedManager.Init(this);

	}
}
public class ThingNeedManager :ThingModule{
	public List<NeedBase> needs = new List<NeedBase>();

	public void AddNeed(NeedBase n)
	{
		//n.Init(this);
		needs.Add(n);

	}
	public override void Init( Thing thing)
	{
		base.Init(thing);
		for (int i = 0; i < needs.Count; i++)
		{
			needs[i].Init(thing);
		}

	}
	public override void hdrUpdate(World world, Thing thing, float timeElapsed)
	{
		base.hdrUpdate(world, thing, timeElapsed);
		ActorBase actor = (ActorBase)thing;

		for (int i = 0; i < needs.Count; i++)
		{
			needs[i].updateStatic(world, actor, timeElapsed);
		}

		if (actor.TAM.IsIdl)
		{
			for (int i = 0; i < needs.Count; i++)
			{
				if (needs[i].UpdateResolveNeed(world, actor, timeElapsed))
				{
					Debug.Log("Resolving a need " + needs[i].name);
					var resolvingNeed = needs[i];
					needs.RemoveAt(i);
					needs.Add(resolvingNeed);
					break;
				}
			}
		}

	}
}
