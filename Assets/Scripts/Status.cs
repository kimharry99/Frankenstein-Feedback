using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Status : ScriptableObject
{
    public int atk;
    public int def;
    public int dex;
    public int mana;
    public int endurance;

    public int[] raceAffinity = new int[5];
    //public int durability;
    public void ResetStatus()
    {
        this.atk = 0;
        this.def = 0;
        this.dex = 0;
        this.mana = 0;
        this.endurance = 0;
        for(int i=0;i<raceAffinity.Length;i++)
        {
            raceAffinity[i] = 0;
        }
    }
}
