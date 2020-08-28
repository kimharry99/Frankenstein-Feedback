using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[System.Serializable]
public abstract class ExplorationEvent : ScriptableObject, ISerializationCallbackReceiver
{
    public enum EventPhase
    {
        None = -1,
        SearchingItem,
        RandomEncounter,
        FinishingExploration,
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
    public float encounterProbabilty;
    /// <summary>
    /// 일회용일 경우 true
    /// </summary>
    public bool isFleeting = false;

    [Header("Event Content")]
    public string titleText;
    public string content;
    [SerializeField]
    private bool basicEnable;
    [NonSerialized]
    public bool isEnabled;
    public string linkedEventName;

    public int OptionNumber { get { return optionTexts.Count; } }
    [Header("Option Field")]
    public List<string> optionTexts;
    public List<string> optionResultTexts;


    /// <summary>
    /// 이벤트가 종료되었을 때 해야하는 작업을 명시한다.
    /// SelectNextEvent가 마지막에 와야 한다.
    /// </summary>
    protected void FinishEvent(bool isReturnHome = false)
    {
        if (isFleeting)
            isEnabled = false;
        ExplorationManager.Inst.FinishEvent(phase ,isReturnHome); 
    }

    /// <summary>
    /// 연계되는 퀘스트를 활성화한다.
    /// </summary>
    protected void UnlockLinkedEvent()
    {
        ExplorationManager.Inst.UnlockLinkedEvent();
    }

    public abstract bool GetOptionEnable(int optionIndex);
    public abstract void Option0();
    public abstract void Option1();
    public abstract void Option2();
    public abstract void Option3();

    public void OnBeforeSerialize()
    {
    }

    public void OnAfterDeserialize()
    {
        Debug.Log("event deserialize");
        isEnabled = basicEnable;
    }
}
