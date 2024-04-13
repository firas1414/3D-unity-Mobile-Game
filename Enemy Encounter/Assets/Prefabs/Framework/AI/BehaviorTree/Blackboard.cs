using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackboard
{
	public delegate void OnBlackboardValueChanged(string key, object val);
	public event OnBlackboardValueChanged onBlackboardValueChanged;
	Dictionary<string, object> blackboardData = new Dictionary<string, object>();
	public void SetOrAddData(string key, object val) // Update the AI behavior data so the AI system can react based on the updated information.
	{
		if(blackboardData.ContainsKey(key))
		{
			blackboardData[key] = val;
		}
		else
		{
			blackboardData.Add(key, val);
		}
		onBlackboardValueChanged?.Invoke(key, val); // Transmit the changed data to other components
	}
	public bool GetBlackboardData<T>(string key, out T val) //****************REVIEWWWWWWW
	{
		val = default(T);
		if(blackboardData.ContainsKey(key))
		{
			val = (T)blackboardData[key];
			return true;
		}
		return false;
	}
	public void RemoveBlackboardData(string key)
	{
		blackboardData.Remove(key);
		onBlackboardValueChanged?.Invoke(key, null);
	}
	public bool HasKey(string key)
	{
		return blackboardData.ContainsKey(key);
	}
}
