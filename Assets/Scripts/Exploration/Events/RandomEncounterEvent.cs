using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RandomEncounterEvent : ExplorationEvent
{
    protected new abstract void FinishEvent(bool isReturnHome = false);
}
