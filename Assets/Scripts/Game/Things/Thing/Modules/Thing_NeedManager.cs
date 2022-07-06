using StoryGenerator.World;
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
	public List<Need> needs = new List<Need>();

	public void AddNeed(Need n)
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

		for (int i = 0; i < needs.Count; i++)
		{
			needs[i].updateStatic(world, thing, timeElapsed);
		}

		if (thing.TAM.IsIdl)
		{
			for (int i = 0; i < needs.Count; i++)
			{
				if (needs[i].ResolveNeed(world, thing, timeElapsed))
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
