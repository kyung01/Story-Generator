using StoryGenerator.World;
using GameEnums;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Player's team
/// Computer's team
/// 
/// </summary>
/// 
public partial class Team
{
	
	//Things I have
	List<ThingClassification> things = new List<ThingClassification>();

	WorkManagaer workManager = new WorkManagaer();
	public WorkManagaer WorkManager { get { return this.workManager; } }
	public Team()
	{
	}

	public void AddThing(Thing thing, Team.ThingRole role)
	{
		things.Add(new ThingClassification(thing, role));
	}

	internal void Update( World world, float timeElapsed)
	{
		workManager.Update( world, timeElapsed);
	}

}
