using GameEnums;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Sleep : NeedBase
{
	public Sleep()
	{
		this.name = "Sleep";
		this.explanation = "Actor needs to sleep periodically.";
		this.fullfillment = 100;
		this.requiredKeywords.Add(Keyword.MOVED);
		this.stressKeywords.Add(Keyword.STILL);
	}
}