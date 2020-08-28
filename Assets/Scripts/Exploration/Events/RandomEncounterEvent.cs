using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class RandomEncounterEvent : ExplorationEvent
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

    [Serializable]
    private struct Case
    {
        public ExplorationEvent resultEvent;
        public int durabilityDamage;
        public Slot rewardItem;
        public Slot consumedItem;
        public string resultString;
    }

    [Serializable]
    private struct Option
    {
        public Case[] cases;
        public StatConstraint[] statConstraints;
        public int[] constraintPerCase;
    }

    protected bool CalConstraint(StatConstraint statConstraint)
    {
        switch(statConstraint.moreOrLess)
        {
            case StatConstraint.MoreOrLess.More:
                if (Player.Inst.GetStatus(statConstraint.statName) > statConstraint.statConstraint)
                    return true;
                else
                    return false;
            case StatConstraint.MoreOrLess.MoreEqual:
                if (Player.Inst.GetStatus(statConstraint.statName) >= statConstraint.statConstraint)
                    return true;
                else
                    return false;
            case StatConstraint.MoreOrLess.Equal:
                if (Player.Inst.GetStatus(statConstraint.statName) == statConstraint.statConstraint)
                    return true;
                else
                    return false;
            case StatConstraint.MoreOrLess.LessEqual:
                if (Player.Inst.GetStatus(statConstraint.statName) <= statConstraint.statConstraint)
                    return true;
                else
                    return false;
            case StatConstraint.MoreOrLess.Less:
                if (Player.Inst.GetStatus(statConstraint.statName) < statConstraint.statConstraint)
                    return true;
                else
                    return false;
            default:
                return true;
        }
    }

    public enum EventType
    {
        None = -1,
        RegionDiscovery,
        PickUpItem,
        /// <summary>
        /// 조우
        /// </summary>
        Encounter,
        DurabilityDamage,
        Skip,
    }

    [NonSerialized]
    public EventType type;
    [Header("Random Encounter Event Field")]
    [SerializeField]
    private Option[] options;
    /// <summary>
    /// 일회용일 경우 true
    /// </summary>
    public bool isFleeting = false;
    [NonSerialized]
    public ExplorationEvent[] resultEvent;
    [NonSerialized]
    public List<string> option0CaseResult;
    private bool _isUnlocked = true;
    private bool _isEncountered;
    public new bool IsEnabled 
    {
        get
        {
            if (!_isUnlocked)
                return false;
            // 일회용 이벤트인 경우
            if (isFleeting && _isEncountered)
                return false;
            return GetIsEnabled();
        }
        set
        {
            _isUnlocked = value;
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
    protected bool GetIsEnabled()
    {
        return true;
    }

    public override void Option0()
    {
        int durabilityDamage;
        Case currentCase;
        currentCase = options[0].cases[GetOptionCaseNumber(0)];
        durabilityDamage = currentCase.durabilityDamage;
        if (durabilityDamage != 0)
        {
            GetDamage(options[0].cases[0].durabilityDamage);
        }
        ExplorationUIManager.Inst.NoticeResultText(optionResultTexts[0]);
        FinishEvent(currentCase.resultEvent);
    }

    private void GetDamage(int damage)
    {
        Player.Inst.Durability -= damage;
        GeneralUIManager.Inst.UpdateTextDurability();
    }

    public override void Option1()
    {
        throw new NotImplementedException();
    }

    public override void Option2()
    {
        throw new NotImplementedException();
    }

    public override void Option3()
    {
        throw new NotImplementedException();
    }
    public override bool GetOptionEnable(int optionIndex)
    {
        return true;
    }

    /// <summary>
    /// optionNumber 옵션의 특수 결과 조건의 번호를 반환한다.
    /// </summary>
    /// <param name="optionNumber"></param>
    /// <returns></returns>
    protected int GetOptionCaseNumber(int optionNumber)
    {
        return 0;
    }
}
