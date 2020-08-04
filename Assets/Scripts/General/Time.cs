using System;
using UnityEngine;

[CreateAssetMenu]
public class Time : ScriptableObject, ISerializationCallbackReceiver
{
    public int initialTime;
    [NonSerialized]
    public int runtimeTime;
    public int initialDay;
    [NonSerialized]
    public int runtimeDay;
    public bool isNight;
    
    public void SetTime(int value)
    {
        runtimeTime = value;
        if(runtimeTime >= 24)
        {
            runtimeTime -= 24;
            runtimeDay++;
        }
        if (runtimeTime != 0 && runtimeTime < 8) 
            isNight = true;
        else
            isNight = false;
    }
    public void OnAfterDeserialize()
    {
        runtimeTime = initialTime;
        runtimeDay = initialDay;
    }

    public void OnBeforeSerialize() { }
}
