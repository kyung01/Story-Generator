
using GameEnums;

public class KeywordInformation
{
	public Keyword keyword;
	public State state;
	public float amount;
	public enum State { AVAILABLE, LOCKED }

	public KeywordInformation(Keyword keyword, State state, float amount)
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