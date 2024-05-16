using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// THIS CLASS STORES ANY INFORMATION ABOUT THE AI, SO THAT THE AI CAN RETRIEVE ANY INFO HE WANTS AND BEHAVE ACCORDINGLY
public class Blackboard
{
    Dictionary<string, object> blackboardData = new Dictionary<string, object>();

    public delegate void OnBlackboardValueChange(string key, object val);

    public event OnBlackboardValueChange onBlackboardValueChange;

    public void SetOrAddData(string key, object val)
    {
        if(blackboardData.ContainsKey(key)) // IF KEY EXISTS, UPDATE ITS VALUE; OTHERWISE, ADD A NEW KEY-VALUE PAIR
        {
            blackboardData[key] = val;
        }
        else
        {
            blackboardData.Add(key, val);
        }
        onBlackboardValueChange?.Invoke(key, val); // TRIGGER EVENT TO NOTIFY LISTENERS ABOUT THE CHANGE
    }

    public bool GetBlackboardData<T>(string key, out T val) // METHOD TO RETRIEVE DATA FROM THE BLACKBOARD
    {
        val = default(T);
        if(blackboardData.ContainsKey(key)) // IF KEY EXISTS, RETRIEVE ITS VALUE AND RETURN TRUE; OTHERWISE, RETURN FALSE
        {
            val = (T)blackboardData[key];
            return true;
        }
        return false;
    }

    public void RemoveBlackboardData(string key)
    {
        blackboardData.Remove(key);
        onBlackboardValueChange?.Invoke(key, null);
    }

    public bool HasKey(string key) // METHOD TO CHECK IF A KEY EXISTS IN THE BLACKBOARD
    {
        return blackboardData.ContainsKey(key);
    }
}
