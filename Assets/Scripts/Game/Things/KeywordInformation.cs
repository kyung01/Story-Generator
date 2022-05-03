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
	public void Combine(KeywordInformation other)
	{
		this.amount += other.amount;
	}

}