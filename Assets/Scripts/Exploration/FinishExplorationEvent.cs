using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class FinishExplorationEvent : ExplorationEvent
{
    /// <summary>
    /// 다른 지역을 탐사한다.
    /// </summary>
    public override void Option0()
    {
        Debug.Log(ExplorationManager.Inst.GetIsOverwork());
        //WarnBodyCollapse(0);
        ExploreAnother();
    }

    public void ExploreAnother()
    {
        GameManager.Inst.OnTurnOver(1);
        ExplorationManager.Inst.MoveToAnotherRegion();
        FinishEvent();
    }

    /// <summary>
    /// 해당 지역을 한 번 더 탐사한다.
    /// </summary>
    public override void Option1()
    {
        Debug.Log(ExplorationManager.Inst.GetIsOverwork());
        ExploreAgain();
    }
    
    public void ExploreAgain()
    {
        GameManager.Inst.OnTurnOver(1);
        FinishEvent();
    }

    /// <summary>
    /// 연구소로 복귀한다.
    /// </summary>
    public override void Option2()
    {
        ReturnHome();
    }

    private void ReturnHome()
    {
        FinishEvent(true);
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
        ExplorationUIManager.Inst.ActiveCollapseWarningPanel(this, option);
    }
}
