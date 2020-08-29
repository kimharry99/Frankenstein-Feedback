using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class FinishExplorationEvent : ExplorationEvent
{
    [Tooltip("null : 연구소로 복귀")]
    public Region[] linkedRegions;
    /// <summary>
    /// 다른 지역을 탐사한다.
    /// </summary>
    public override void Option0()
    {
        //if (ExplorationManager.Inst.GetIsOverwork())
        //    WarnBodyCollapse(1);
        //else
        ExplorationUIManager.Inst.NoticeResultText(optionResultTexts[0]);
        ExploreAnother(0);
    }

    /// <summary>
    /// 해당 지역을 한 번 더 탐사한다.
    /// </summary>
    public override void Option1()
    {
        //if (ExplorationManager.Inst.GetIsOverwork())
        //    WarnBodyCollapse(0);
        //else
        ExplorationUIManager.Inst.NoticeResultText(optionResultTexts[1]);
        ExploreAnother(1);
    }

    public void ExploreAnother(int regionIndex = 0)
    {
        if(linkedRegions[regionIndex] == null)
        {
            ReturnHome();
            return;
        }
        GameManager.Inst.OnTurnOver(1);
        ExplorationManager.Inst.MoveToAnotherRegion(linkedRegions[regionIndex]);
        FinishEvent();
    }

    public override void Option2()
    {
        ExploreAnother(2);
    }

    private void ReturnHome()
    {
        FinishEvent(null, true);
        GameManager.Inst.ReturnHome();
        GameManager.Inst.OnTurnOver(1);
    }

    public override void Option3()
    {
    }

    public override bool GetOptionEnable(int optionIndex)
    {
        return true;
    }

    private void WarnBodyCollapse(int option)
    {
        //   ExplorationUIManager.Inst.ActiveCollapseWarningPanel(this, option);
    }
    public void ExploreAgain()
    {
        GameManager.Inst.OnTurnOver(1);
        FinishEvent();
    }
}
