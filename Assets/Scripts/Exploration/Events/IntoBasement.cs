using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class IntoBasement : RandomEncounterEvent
{
    [SerializeField]
    private Time _time;
    protected override bool GetIsEnabled()
    {
        if (_time.runtimeTime != 0)
            return false;
        if (!Player.Inst.NightVision)
            return false;
        return base.GetIsEnabled();
    }
}
