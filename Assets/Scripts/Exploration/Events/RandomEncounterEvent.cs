using System;
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

    [System.Serializable]
    private struct Case
    {
        public ExplorationEvent resultEvent;
        public int durabilityDamage;
        public Slot rewardItem;
        public Slot consumedItem;
        public string resultString;
    }

    [System.Serializable]
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
    public bool isFleeting = false;
    [SerializeField]
    private Option[] options;
    /// <summary>
    /// 일회용일 경우 true
    /// </summary>
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

    #region Option Methods

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
        ExplorationUIManager.Inst.NoticeResultText(currentCase.resultString);
        if(currentCase.resultEvent == null)
        {
            Debug.Log("result is not assigned");
            return;
        }
        FinishEvent(currentCase.resultEvent);
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
#endregion
    private void GetDamage(int damage)
    {
        Player.Inst.Durability -= damage;
        GeneralUIManager.Inst.UpdateTextDurability();
    }

    #region Constraint Methods

    /// <summary>
    /// optionNumber 옵션의 특수 결과 조건의 번호를 반환한다.
    /// </summary>
    /// <param name="optionNumber"></param>
    /// <returns></returns>
    protected int GetOptionCaseNumber(int optionNumber)
    {
        if (!CaseLengthMatched())
        {
            Debug.LogError("case lenfth mismatched");
            return 0;
        }
        if (!ConstraintLengthMatched())
        {
            Debug.LogError("constraint number mismatched");
            return 0;
        }
        Option option = options[optionNumber];
        int constraintBase = 0;
        int caseBase = 0;
        for (int i = optionNumber; i > 0; i--)
        {
            constraintBase += option.constraintPerCase[optionNumber - 1];
        }
        Debug.Log("constraing Base : " + constraintBase + " caseBase : " + caseBase);
        switch (option.cases.Length)
        {
            case 0:
            case 1:
                return 0;
            case 2:
                if (CalConstraints(0, option.constraintPerCase[0], option.statConstraints))
                    return 0;
                else
                    return 1;
            case 3:
                //Debug.Log("CalConstraints("+ (constraintBase + _constraintPerCase[caseBase]) + ", " + _constraintPerCase[caseBase + 1]+", " +statConstraints+")");
                if (CalConstraints(0, option.constraintPerCase[0], option.statConstraints))
                    return 0;
                else if (CalConstraints(option.constraintPerCase[0], option.constraintPerCase[1], option.statConstraints))
                    return 1;
                else
                    return 2;
        }
        return 0;
    }

    /// <summary>
    /// statConstraints의 [i, i + n)구간의 constraint를 계산하여 결과를 return한다.
    /// </summary>
    private bool CalConstraints(int i, int n, StatConstraint[] statConstraints)
    {
        if (i + n - 1 >= statConstraints.Length)
        {
            Debug.Log("out of index i+n : " + (i + n) + " length : " + statConstraints.Length);
            return true;
        }
        for (int t = i; t < i + n; t++)
        {
            if (!CalConstraint(statConstraints[t]))
                return false;
        }
        return true;
    }
    private bool CaseLengthMatched()
    {
        for(int i=0;i<options.Length;i++)
        {
            Option option = options[i];
            if (option.cases.Length <= 1)
                continue;
            if (options[i].constraintPerCase.Length != options[i].cases.Length)
                return false;
        }
        return true;
    }

    private bool ConstraintLengthMatched()
    {
        for(int i=0;i<options.Length;i++)
        {
            Option option = options[i];
            if (option.cases.Length <= 1)
                continue;
            int a = 0;
            for(int j=0;j< option.cases.Length;j++)
            {
                a += option.constraintPerCase[j];
            }
            if (a != option.statConstraints.Length)
                return false;
        }
        return true;
    }
#endregion
}
