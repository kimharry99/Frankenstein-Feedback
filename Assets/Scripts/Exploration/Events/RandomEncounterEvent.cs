using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RandomEncounterEvent : ExplorationEvent
{
    [System.Serializable]
    protected struct StatConstraint
    {
        [System.Serializable]
        public enum MoreOrLess
        {
            More,
            MoreEqual,
            Equal,
            LessEqual,
            Less,
        }
        [Tooltip("플레이어의 스텟이 요구스텟 보다 높아야 하면 more")]
        public MoreOrLess moreOrLess;
        public Status.StatName statName;
        public int statConstraint;
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
        Skip,
    }

    [Header("Random Encounter Event Field")]
    public EventType type;
    /// <summary>
    /// 일회용일 경우 true
    /// </summary>
    public bool isFleeting = false;
    public ExplorationEvent[] resultEvent;
    [Tooltip("option0 번의 특수 결과 내용 결과가 갈리지 않으면 size는 0")]
    public List<string> option0CaseResult;
    private bool _isEncountered;
    public new bool IsEnabled 
    {
        get
        {
            // 일회용 이벤트인 경우
            if (isFleeting && _isEncountered)
                return false;
            return GetIsEnabled();
        }
    }
    protected new void FinishEvent(ExplorationEvent nextEvent = null, bool isReturnHome = false)
    {
        if(nextEvent == null)
        {
            Debug.LogError("nextEvent is not assigned : " + eventName);
            return;
        }
        ExplorationManager.Inst.FinishEvent(phase, nextEvent, isReturnHome);
    }
    protected abstract bool GetIsEnabled();

    /// <summary>
    /// optionNumber 옵션의 특수 결과 조건의 번호를 반환한다.
    /// </summary>
    /// <param name="optionNumber"></param>
    /// <returns></returns>
    protected abstract int GetOptionCaseNumber(int optionNumber);
}
