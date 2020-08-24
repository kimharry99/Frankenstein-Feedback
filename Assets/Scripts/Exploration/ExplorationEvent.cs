using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public abstract class ExplorationEvent : ScriptableObject
{
    public enum EventPhase
    {
        None = -1,
        RandomEncounter,
        SearchingItem,
    }
    public enum EventType
    { 
        None = -1,
        RegionDiscovery,
        ItemDiscovery,
        /// <summary>
        /// 조우
        /// </summary>
        Encounter,
        DurabilityDamage,
    }


    public EventPhase phase;
    public EventType type;
    public int id;
    public string eventName;
    public string titleText;
    public string content;

    public abstract void Foo();
}
