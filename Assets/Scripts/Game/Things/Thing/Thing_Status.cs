using StoryGenerator.World;
using GameEnums;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public partial class Thing
{
	internal List<StatusBase> statuses = new List<StatusBase>();

	public void InitStatus()
	{
		this.OnUpdate.Add(hdrStatusUpdate);
	}

	void hdrStatusUpdate(World world, Thing thing, float timeElapsed)
	{
		for(int i = thing.statuses.Count-1; i >= 0; i--)
		{
			thing.statuses[i].Update(world, thing, timeElapsed);
		}
	}

	public void AddStatus(StatusBase status)
	{
		status.Init(this);
		this.statuses.Add(status);
	}
}