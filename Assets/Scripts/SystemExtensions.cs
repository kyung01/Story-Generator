using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class SystemExtensions
{
	static public bool ContainsAnyKey<TKey, TValue>(this Dictionary<TKey, TValue> me, List<TKey> keys)
	{
		foreach (var key in keys)
		{
			if (me.ContainsKey(key)) return true;
		}
		return false;
	}
	static public bool ContainsAnyKey<TKey, TValue>(this Dictionary<TKey, TValue> me, List<TKey> keys, ref TKey keyContained)
	{
		foreach (var key in keys)
		{
			if (me.ContainsKey(key))
			{
				keyContained = key;
				return true;
			}

		}
		return false;
	}
	static public bool ContainsAnyKey<TKey, TValue>(this Dictionary<TKey, TValue> me, List<TKey> keys, ref List<TKey> keysContained)
	{
		bool isFound = false;
		foreach (var key in keys)
		{
			if (me.ContainsKey(key))
			{
				isFound = true;
				keysContained.Add(key);
				return true;
			}
		}
		return isFound;
	}
}