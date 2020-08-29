using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BodyCollapseEvent : RandomEncounterEvent
{
    [Header("Body Collapse Event Field")]
    public int durabilityConstraint;
    public BodyPartType partType;

    public override bool IsEnabled 
    { 
        get => base.IsEnabled; 
        set => base.IsEnabled = value; 
    }

    protected override bool GetIsEnabled()
    {
        Debug.Log(Player.Inst.equippedBodyPart.bodyParts[(int)partType] == null);
        if (Player.Inst.equippedBodyPart.bodyParts[(int)partType] == null)
            return false;
        if (Player.Inst.durability.value > durabilityConstraint)
            return false;
        return base.GetIsEnabled();
    }

    protected override void FinishEvent(ExplorationEvent nextEvent = null, bool isReturnHome = false)
    {
        base.FinishEvent(nextEvent, isReturnHome);
    }

    public override void Option0()
    {
        base.Option0();
    }

    public override void Option1()
    {
        if (partType != BodyPartType.Head || partType != BodyPartType.None)
            Player.Inst.equippedBodyPart.bodyParts[(int)partType] = null;
        base.Option1();
    }
}
