using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class KeywordValuePair
{
	public GameEnums.Keyword keyword;
	public float value;
	public KeywordValuePair(GameEnums.Keyword keyword, float value)
	{
		this.keyword = keyword;
		this.value = value;
	}
}