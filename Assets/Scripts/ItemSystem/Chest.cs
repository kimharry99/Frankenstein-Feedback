using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Storage
{
    public const int CAPACITY = 30;
    public override int Capacity { get { return CAPACITY; } }
}
