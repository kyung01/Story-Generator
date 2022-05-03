using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class KeywordInformation
{
	public Game.Keyword keyword;
	public State state;
	public float amount;
	public enum State { AVAILABLE, LOCKED }

	public KeywordInformation(Game.Keyword keyword, State state, float amount)
	{
		this.keyword = keyword;
		this.state = state;
		this.amount = amount;
	}
	public void Combine(KeywordInformation other)
	{
		this.amount += other.amount;
	}

}