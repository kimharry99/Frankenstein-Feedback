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

    public new int Atk 
    { 
        get 
        {
            if (bodyPartType == BodyPartType.LeftArm)
                return Mathf.Min(maxAtk, atk + Player.Inst.GetUpgradeLevel(BodyPartType.LeftArm) * 5);
            else if (bodyPartType == BodyPartType.RightArm)
                return Mathf.Min(maxAtk, atk + Player.Inst.GetUpgradeLevel(BodyPartType.RightArm) * 5);
            else
                return atk;
        } 
    }
    public new int Def 
    { 
        get 
        {
            if (bodyPartType == BodyPartType.Body)
                return Mathf.Min(maxDef, def + Player.Inst.GetUpgradeLevel(BodyPartType.Body) * 5);
            else
                return def;
        }
    }
    public new int Dex
    {
        get
        {
            if (bodyPartType == BodyPartType.LeftLeg)
                return Mathf.Min(maxDex, dex + Player.Inst.GetUpgradeLevel(BodyPartType.LeftArm) * 10);
            else if (bodyPartType == BodyPartType.RightLeg)
                return Mathf.Min(maxDex, dex + Player.Inst.GetUpgradeLevel(BodyPartType.RightArm) * 10);
            else
                return dex;
        }
    }
    public new int Mana 
    { 
        get 
        {
            if (bodyPartType == BodyPartType.Head)
                return Mathf.Min(maxMana, mana + Player.Inst.GetUpgradeLevel(BodyPartType.Head) * 5);
            else
                return mana;
        } 
    }

}
