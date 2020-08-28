using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DurabilityDamageEvent : RandomEncounterEvent
{
    [Header("DurabilityDamageEvent Field")]
    [SerializeField]
    private int _damage;
    [SerializeField]
    private StatConstraint[] statConstraints;

    public override bool GetOptionEnable(int optionIndex)
    {
        return true;
    }

    public override void Option0()
    {
        switch(GetOptionCaseNumber(0))
        {
            case 0:
                Debug.Log("회피 실패");
                GetDamage(_damage);
                optionResultTexts[0] = option0CaseResult[0];
                break;
            case 1:
                Debug.Log("회피 성공");
                optionResultTexts[0] = option0CaseResult[1];
                break;
        }
        ExplorationUIManager.Inst.NoticeResultText(optionResultTexts[0]);
        FinishEvent(resultEvent[0]);
    }

    /// <param name="optionNumber">항상 0</param>
    /// <returns>0 : 회피 실패, 1 : 회피 성공</returns>
    protected override int GetOptionCaseNumber(int optionNumber)
    {
        if(optionNumber == 0)
        {
            if (statConstraints.Length != 0)
            {
                switch (statConstraints[0].statName)
                {
                    case Status.StatName.Dex:
                        if (Player.Inst.Dex >= 20)
                            return 1;
                        else
                            return 0;
                    default:
                        Debug.LogError("Wrong Stat Name");
                        break;
                }
            }
            else
            {
                return 0;
            }
        }
        return 0;
    }

    private void GetDamage(int damage)
    {
        Player.Inst.Durability -= damage;
        GeneralUIManager.Inst.UpdateTextDurability();
    }

    public override void Option1()
    {
    }

    public override void Option2()
    {
    }

    public override void Option3()
    {
    }

    protected override bool GetIsEnabled()
    {
        return true;
    }
}
