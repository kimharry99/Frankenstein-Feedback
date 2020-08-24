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

    [Header("Event Info")]
    public int id;
    public string eventName;
    public EventPhase phase;
    public EventType type;

    [Header("Event Content")]
    public string titleText;
    public string content;

    public int OptionNumber { get { return optionTexts.Count; } }
    [Header("Option Field")]
    public List<string> optionTexts = new List<string>();

    public abstract void Option0();
    public abstract void Option1();
    public abstract void Option2();
    public abstract void Option3();
}
