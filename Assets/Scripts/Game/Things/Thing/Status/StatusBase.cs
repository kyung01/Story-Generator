using StoryGenerator.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class StatusBase
{
	public enum Type { UNKNOWN, FAMISHED,
		NEW_BEGINNING
	}

	internal Type T;
	internal string name;
	internal string explanation;
	internal float duration;
	internal float timeElapsed;

	public StatusBase(Type T, string name, string explanation, float duration)
	{
		this.T = T;
		this.name = name;
		this.explanation = explanation;
		this.duration = duration;
		timeElapsed = 0;
	}

	public virtual void Init(Thing thing)
	{

	}
	public virtual void Update(World world, Thing thing, float timeElapsedTick)
	{
		this.timeElapsed += timeElapsedTick;

		if (timeElapsed >= duration)
		{
			thing.statuses.Remove(this);
		}

	}

	
}