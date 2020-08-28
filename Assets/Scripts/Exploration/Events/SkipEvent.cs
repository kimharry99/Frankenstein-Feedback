using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SkipEvent : RandomEncounterEvent
{
    public override bool GetOptionEnable(int optionIndex)
    {
        return true;
    }

    public override void Option2() { }

    public override void Option3() { }
}
