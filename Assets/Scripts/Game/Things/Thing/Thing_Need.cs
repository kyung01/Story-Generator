using StoryGenerator.World;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public partial class Thing
{
	ThingNeedManager thingNeedManager;
	public ThingNeedManager TNM { get { return this.thingNeedManager; } }

	public void InitTNM()
	{
		thingNeedManager = new ThingNeedManager();
		this.OnUpdate.Add(thingNeedManager.Update);

	}
}
public class ThingNeedManager {
	public List<Need> needs = new List<Need>();

	public void AddNeed(Need n)
	{
		//n.Init(this);
		needs.Add(n);

	}
	internal void Init( Thing thing)
	{
		for (int i = 0; i < needs.Count; i++)
		{
			needs[i].Init(thing);
		}

	}

	internal void Update(World world, Thing thing, float timeElapsed)
	{
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
