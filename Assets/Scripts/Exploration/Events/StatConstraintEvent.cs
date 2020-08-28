using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class StatConstraintEvent : RandomEncounterEvent
{ 
    [SerializeField]
    private StatConstraint[] statConstraints;

    [Tooltip("[0] = 1 option 0은 case 1개를 사용")]
    [SerializeField]
    private int[] _casePerOption;
    [Tooltip("[0] = 1 option 0은 constraint 1개를 사용")]
    [SerializeField]
    private int[] _constraintPerOption;
    [Tooltip("[0] = 1 case 0은 constraint 1개를 사용")]
    [SerializeField]
    private int[] _constraintPerCase;

    public override bool GetOptionEnable(int optionIndex)
    {
        return true;
    }

    public override void Option0()
    {
        int caseNumber = GetOptionCaseNumber(0);
        optionResultTexts[0] = option0CaseResult[caseNumber];
        ExplorationUIManager.Inst.NoticeResultText(optionResultTexts[0]);
        FinishEvent(resultEvent[caseNumber]);
    }

    public override void Option1()
    {
        Debug.Log(GetOptionCaseNumber(1));
    }

    public override void Option2()
    {
        throw new System.NotImplementedException();
    }

    public override void Option3()
    {
        throw new System.NotImplementedException();
    }

    protected override bool GetIsEnabled()
    {
        throw new System.NotImplementedException();
    }

    protected override int GetOptionCaseNumber(int optionNumber)
    {
        if(!OptionLengthMatched())
        {
            Debug.LogError("option length mismatched");
            return 0;
        }
        if(!CaseLengthMatched())
        {
            Debug.LogError("case lenfth mismatched");
            return 0;
        }
        if(!ConstraintLengthMatched())
        {
            Debug.LogError("constraint number mismatched");
            return 0;
        }
        int constraintBase = 0;
        int caseBase = 0;
        for(int i=optionNumber; i > 0 ;i--)
        {
            constraintBase += _constraintPerCase[optionNumber - 1];
            caseBase += _casePerOption[optionNumber - 1];
        }
        Debug.Log("constraing Base : " + constraintBase + " caseBase : " + caseBase);
        switch(_casePerOption[optionNumber])
        {
            case 0:
            case 1:
                return 0;
            case 2:
                if (CalConstraints(constraintBase, _constraintPerCase[caseBase], statConstraints))
                    return 0;
                else
                    return 1;
            case 3:
                //Debug.Log("CalConstraints("+ (constraintBase + _constraintPerCase[caseBase]) + ", " + _constraintPerCase[caseBase + 1]+", " +statConstraints+")");
                if (CalConstraints(constraintBase, _constraintPerCase[caseBase], statConstraints))
                    return 0;
                else if (CalConstraints(constraintBase + _constraintPerCase[caseBase], _constraintPerCase[caseBase + 1], statConstraints))
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
        if (i + n - 1>= statConstraints.Length)
        {
            Debug.Log("out of index i+n : " + (i+n) + " length : " + statConstraints.Length);
            return true;
        }
        for(int t = i; t < i+n ; t++ )
        {
            if (!CalConstraint(statConstraints[t]))
                return false;
        }
        return true;
    }

    private bool OptionLengthMatched()
    {
        if (optionResultTexts.Count == optionTexts.Count)
            if (optionTexts.Count == _constraintPerOption.Length)
                if (_constraintPerOption.Length == _casePerOption.Length)
                    return true;
        return false;
    }

    private bool CaseLengthMatched()
    {
        int a = 0;
        for(int i=0;i<_casePerOption.Length;i++)
        {
            a += _casePerOption[i];
        }
        if(a == _constraintPerCase.Length && a == resultEvent.Length)
            return true;
        return false;
    }

    private bool ConstraintLengthMatched()
    {
        int a = 0;
        for(int i=0;i<_constraintPerCase.Length;i++)
        {
            a += _constraintPerCase[i];
        }
        int b = 0;
        for(int i=0;i<_constraintPerOption.Length;i++)
        {
            b += _constraintPerOption[i];
        }
        if (statConstraints.Length == a && a == b)
            return true;
        return false;
    }
}
