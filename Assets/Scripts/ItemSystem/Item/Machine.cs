using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Machine : BodyPart
{
    [Header("Machine Max Stats")]
    public int maxAtk;
    public int maxDef;
    public int maxDex;
    public int maxMana;
    public int maxEndurance;

    public new int Def { get { return Mathf.Min(maxDef, def + Player.Inst.GetUpgradeLevel(BodyPartType.Body) * 5); } }

}
