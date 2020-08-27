using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Consumable : Item
{
    public abstract void UseItem();

    public abstract bool IsConsumeEnable();
}
