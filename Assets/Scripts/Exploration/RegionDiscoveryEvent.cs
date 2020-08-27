using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class RegionDiscoveryEvent : ExplorationEvent
{
    public Region foundRegion;

    public override bool GetOptionEnable(int optionIndex)
    {
        return true;
    }

    public override void Option0()
    {
        UnlockRegion(foundRegion);
        FinishEvent();
    }
    
    private void UnlockRegion(Region foundRegion)
    {
        ExplorationManager.Inst.UnlockRegion(foundRegion);
    }

    public override void Option1()
    {
        FinishEvent();
    }

    public override void Option2()
    {

    }

    public override void Option3()
    {

    }
}
