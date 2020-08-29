using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MachinePart : Consumable
{
    [Header("Machine Part Fields")]
    public int atkValue;
    public int defValue;
    public int dexValue;
    public int manaValue;

    public override bool IsConsumeEnable()
    {
        EquippedBodyPart equippedBodyPart = Player.Inst.equippedBodyPart;
        if(atkValue != 0)
        {
            if(!IsMachine(equippedBodyPart, BodyPartType.LeftArm) && !IsMachine(equippedBodyPart, BodyPartType.RightArm))
            {
                return false;
            }
        }
        if (defValue != 0)
        {
            if (!IsMachine(equippedBodyPart, BodyPartType.Body))
                return false;
        }
        if(dexValue != 0)
        {
            if(!IsMachine(equippedBodyPart, BodyPartType.LeftLeg) && !IsMachine(equippedBodyPart, BodyPartType.RightLeg))
            {
                return false;
            }
        }
        if(manaValue != 0)
        {
            if (!IsMachine(equippedBodyPart, BodyPartType.Head))
                return false;
        }
        return true;
    }

    private bool IsMachine(EquippedBodyPart equippedBodyPart, BodyPartType bodyPartType)
    {
        if (equippedBodyPart.bodyParts[(int)bodyPartType] == null)
            return false;
        else
        {
            return equippedBodyPart.bodyParts[(int)bodyPartType].race != Race.Machine;
        }
    }

    public override void UseItem()
    {
        Debug.Log("기계부품 사용");
        EquippedBodyPart equippedBodyPart = Player.Inst.equippedBodyPart;
        if (IsConsumeEnable())
        {
            if (atkValue != 0)
            {
                UpgradeAtk(equippedBodyPart);
            }
            if(defValue != 0)
            {
                UpgradeDef(equippedBodyPart);
            }
            if(dexValue != 0)
            {
                UpgradeDex(equippedBodyPart);
            }
            if(manaValue != 0)
            {
                UpgradeMana(equippedBodyPart);
            }
        }
    }

    private void UpgradeAtk(EquippedBodyPart equippedBodyPart)
    {
        if (IsMachine(equippedBodyPart, BodyPartType.LeftArm) && IsMachine(equippedBodyPart, BodyPartType.RightArm))
        {
            if (equippedBodyPart.LeftArm.grade >= equippedBodyPart.RightArm.grade)
            {
                Player.Inst.UpgradeMachinePart((int)BodyPartType.LeftArm);
            }
            else
            {
                Player.Inst.UpgradeMachinePart((int)BodyPartType.RightArm);
            }
        }
        else if (IsMachine(equippedBodyPart, BodyPartType.LeftArm))
        {
            Player.Inst.UpgradeMachinePart((int)BodyPartType.LeftArm);
        }
        else
        {
            Player.Inst.UpgradeMachinePart((int)BodyPartType.RightArm);
        }
    }

    private void UpgradeDef(EquippedBodyPart equippedBodyPart)
    {
        Player.Inst.UpgradeMachinePart((int)BodyPartType.Body);
    }

    private void UpgradeDex(EquippedBodyPart equippedBodyPart)
    {
        if (IsMachine(equippedBodyPart, BodyPartType.LeftLeg) && IsMachine(equippedBodyPart, BodyPartType.RightLeg))
        {
            if (equippedBodyPart.LeftLeg.grade >= equippedBodyPart.RightLeg.grade)
            {
                Player.Inst.UpgradeMachinePart((int)BodyPartType.LeftLeg);
            }
            else
            {
                Player.Inst.UpgradeMachinePart((int)BodyPartType.RightLeg);
            }
        }
        else if (equippedBodyPart.LeftLeg.race == Race.Machine)
        {
            Player.Inst.UpgradeMachinePart((int)BodyPartType.LeftLeg);
        }
        else
        {
            Player.Inst.UpgradeMachinePart((int)BodyPartType.RightLeg);
        }
    }

    private void UpgradeMana(EquippedBodyPart equippedBodyPart)
    {
        Player.Inst.UpgradeMachinePart((int)BodyPartType.Head);
    }
}
