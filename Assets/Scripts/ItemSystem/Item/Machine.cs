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

    public int MaxAtk { get { return maxAtk + GameManager.Inst.research.GetBonusLimit(); } }
    public int MaxDef { get { return maxDef + GameManager.Inst.research.GetBonusLimit(); } }
    public int MaxDex { get { return maxDex + GameManager.Inst.research.GetBonusLimit(); } }
    public int MaxMana { get { return maxMana + GameManager.Inst.research.GetBonusLimit(); } }

    public new int Atk 
    { 
        get 
        {
            if (bodyPartType == BodyPartType.LeftArm)
                return Mathf.Min(MaxAtk, atk + Player.Inst.GetUpgradeLevel(BodyPartType.LeftArm) * 5);
            else if (bodyPartType == BodyPartType.RightArm)
                return Mathf.Min(MaxAtk, atk + Player.Inst.GetUpgradeLevel(BodyPartType.RightArm) * 5);
            else
                return atk;
        } 
    }
    public new int Def 
    { 
        get 
        {
            if (bodyPartType == BodyPartType.Body)
                return Mathf.Min(MaxDef, def + Player.Inst.GetUpgradeLevel(BodyPartType.Body) * 5);
            else
                return def;
        }
    }
    public new int Dex
    {
        get
        {
            if (bodyPartType == BodyPartType.LeftLeg)
                return Mathf.Min(MaxDex, dex + Player.Inst.GetUpgradeLevel(BodyPartType.LeftArm) * 10);
            else if (bodyPartType == BodyPartType.RightLeg)
                return Mathf.Min(MaxDex, dex + Player.Inst.GetUpgradeLevel(BodyPartType.RightArm) * 10);
            else
                return dex;
        }
    }
    public new int Mana 
    { 
        get 
        {
            if (bodyPartType == BodyPartType.Head)
                return Mathf.Min(MaxMana, mana + Player.Inst.GetUpgradeLevel(BodyPartType.Head) * 5);
            else
                return mana;
        } 
    }

}
