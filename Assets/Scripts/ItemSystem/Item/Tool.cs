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

    [SerializeField]
    private bool _nightVision;
    public bool nightVision { get { return _nightVision; } }
    [SerializeField]
    private bool _magic;
    public bool magic { get { return _magic; } }

    public int Atk { get { return atk; } }
    public int Def { get { return def; } }
    public int Dex { get { return dex; } }
    public int Mana { get { return mana; } }
    public int Endurance { get { return endurance; } }
}
