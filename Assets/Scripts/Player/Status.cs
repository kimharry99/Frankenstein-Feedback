using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : ScriptableObject
{
    public int atk;
    public int def;
    public int dex;
    public int mana;
    public int endurance;

    //public int durability;
    public void ResetStatus()
    {
        atk = 0;
        def = 0;
        dex = 0;
        mana = 0;
        endurance = 0;
    }
}
