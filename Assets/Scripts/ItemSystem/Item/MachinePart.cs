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
            if(equippedBodyPart.LeftArm.race != Race.Machine && equippedBodyPart.RightArm.race != Race.Machine)
            {
                return false;
            }
        }
        if (defValue != 0)
        {
            if (equippedBodyPart.Body.race != Race.Machine)
                return false;
        }
        if(dexValue != 0)
        {
            if(equippedBodyPart.LeftLeg.race != Race.Machine && equippedBodyPart.RightLeg.race != Race.Machine)
            {
                return false;
            }
        }
        if(manaValue != 0)
        {
            if (equippedBodyPart.Head.race != Race.Machine)
                return false;
        }
        return true;
    }

    public override void UseItem()
    {
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
        if (equippedBodyPart.LeftArm.race == Race.Machine && equippedBodyPart.RightArm.race == Race.Machine)
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
        else if (equippedBodyPart.LeftArm.race == Race.Machine)
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
        if (equippedBodyPart.LeftLeg.race == Race.Machine && equippedBodyPart.RightLeg.race == Race.Machine)
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
