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
        GameManager.Inst.OnTurnOver(1);
        ExploreAnother();
        FinishEvent();
    }

    private void ExploreAnother()
    {

    }

    /// <summary>
    /// 해당 지역을 한 번 더 탐사한다.
    /// </summary>
    public override void Option1()
    {
        GameManager.Inst.OnTurnOver(1);
        ExploreAgain();
        FinishEvent();
    }
    
    private void ExploreAgain()
    {

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


}
