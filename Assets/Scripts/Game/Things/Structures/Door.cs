using StoryGenerator.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Door : Structure
{
	public enum State 
	{
		OPEN,
		SHOULD_OPEN,
		CLOSED,
		SHOULD_CLOSE
	
	
	}
	State state = State.CLOSED;

	float openNess = 0;
	float doorOpenSpeed = 3f;
	float doorCloseSpeed = 5;
	float doorOpenDuration = 1.0f;

	float doorOpenTimeElapsed = 0;
	
	public bool IsOpen { get { return openNess == 1; } }

	public float OpenLevel { get { return openNess; } }

	public Door()
	{
		this.type = TYPE.DOOR;
	}
	public void Open()
	{
		this.state = State.SHOULD_OPEN;
		this.doorOpenTimeElapsed = 0;
	}

	public override void Update(World world, float timeElapsed)
	{
		base.Update(world, timeElapsed);
		switch (this.state)
		{
			default:
			case State.CLOSED:
				break;
			case State.SHOULD_OPEN:
				openNess += doorOpenSpeed * timeElapsed;
				openNess = Math.Min(1, openNess);
				if (openNess == 1)
				{
					this.doorOpenTimeElapsed = 0;
					this.state = State.OPEN;
				}
				break;
			case State.OPEN:
				doorOpenTimeElapsed += timeElapsed;
				if(doorOpenTimeElapsed > doorOpenDuration)
				{
					doorOpenTimeElapsed = 0;
					this.state = State.SHOULD_CLOSE;
				}
				break;
			case State.SHOULD_CLOSE:
				var things = world.GetThingsAt(this.X_INT, this.Y_INT);
				if (hprCheckIfSomethingStuck(things))
				{
					Open();
					return;
				}
				else
				{
					openNess -= doorCloseSpeed * timeElapsed;
					openNess = Math.Max(0, openNess);
				}
				if(openNess == 0)
				{
					this.state = State.CLOSED;
					return;
				}
				break;
			
		}

		
	}

	private bool hprCheckIfSomethingStuck(List<Thing> things)
	{
		foreach (Thing t in things)
		{
			//Check for uninstalled structure being stuck
			if(t is Structure)
			{
				var s = (Structure)t;
				if (!s.IsInstalled)
				{
					return true;
				}
				//installed structure 

			}
			if (t.type == TYPE.FLOOR || t.type == TYPE.DOOR || t.type == TYPE.CEILING)
			{
				//these dont get stuck
				continue;
			}
			return true;
		}
		return false;
	}
}