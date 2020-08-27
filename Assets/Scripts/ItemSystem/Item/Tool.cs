using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Tool : Item
{
    [Header("Item Stat")]
    public int atk;
    public int def;
    public int dex;
    public int mana;
    public int endurance;

    public int Def { get { return def; } }
}
