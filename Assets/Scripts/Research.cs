using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Research : SingletonBehaviour<Research>
{
    [SerializeField] // for debugging
    private int[] researchLevel = new int[4];

    public int GoblinLevel { get { return researchLevel[0]; } set { if (value < 10) researchLevel[0] = value; } }
    public int GoblinEnergyCost { get { return (GoblinLevel + 1) * 10000;} }
    public int ElfLevel { get { return researchLevel[1]; } set { if (value < 10) researchLevel[1] = value; } }
    public int ElfEnergyCost { get { return (ElfLevel + 4) * 10000; } }
    public int OakLevel { get { return researchLevel[2]; } set { if (value < 10) researchLevel[2] = value; } }
    public int OakEnergyCost { get { return (OakLevel + 5) * 10000; } }
    public int MachineLevel { get { return researchLevel[3]; } set { if (value < 10) researchLevel[3] = value; } } 
    public int MachineEnergyCost { get { return 50000; } }

    public void ResearchRace(int raceIndex)
    {
        switch(raceIndex)
        {
            case 0:
                GoblinLevel++;
                break;
            case 1:
                ElfLevel++;
                break;
            case 2:
                OakLevel++;
                break;
            case 3:
                MachineLevel++;
                break;
            default:
                Debug.LogError("wrong raceIndex" + raceIndex);
                break;
        }
    }
    
    public int ResearchCost(int raceIndex)
    {
        switch (raceIndex)
        {
            case 0:
                return GoblinEnergyCost;
            case 1:
                return ElfEnergyCost;
            case 2:
                return OakEnergyCost;
            case 3:
                return MachineEnergyCost;
            default:
                Debug.LogError("wrong raceIndex" + raceIndex);
                return -1;
        }
    }

    public float GetRegenBonus()
    {
        float bonus = 0;
        float bonusCoef = GetBonusCoef(Race.Goblin);
        bonus += bonusCoef * Player.Inst.equippedBodyPart.GetCntOfBodyPart(Race.Goblin);
        bonusCoef = GetBonusCoef(Race.Elf);
        bonus += bonusCoef * Player.Inst.equippedBodyPart.GetCntOfBodyPart(Race.Elf);
        bonusCoef = GetBonusCoef(Race.Oak);
        bonus += bonusCoef * Player.Inst.equippedBodyPart.GetCntOfBodyPart(Race.Oak);
        bonusCoef = GetBonusCoef(Race.Machine);
        bonus += bonusCoef * Player.Inst.equippedBodyPart.GetCntOfBodyPart(Race.Machine);

        return bonus;
    }

    private float GetBonusCoef(Race race)
    {
        int rLevel = 0;
        float bonusCoef = 0.0f;
        switch (race)
        {
            case Race.Goblin:
                rLevel = GoblinLevel;
                break;
            case Race.Elf:
                rLevel = ElfLevel;
                break;
            case Race.Oak:
                rLevel = OakLevel;
                break;
            case Race.Machine:
                rLevel = MachineLevel;
                break;
        }
        switch (rLevel)
        {
            case 1:
            case 2:
                bonusCoef = 0.1f;
                break;
            case 3:
            case 4:
                bonusCoef = 0.2f;
                break;
            case 5:
            case 6:
            case 7:
                bonusCoef = 0.3f;
                break;
            case 8:
            case 9:
            case 10:
                bonusCoef = 0.4f;
                break;
            default:
                break;
        }
        return bonusCoef;
    }

    public int GetStatBonus(Status.StatName statName)
    {
        switch(statName)
        {
            case Status.StatName.Atk:
                return GetAtkBonus();
            case Status.StatName.Def:
                return GetDefBonus();
            case Status.StatName.Dex:
                return GetDexBonus();
            case Status.StatName.Mana:
                return GetManaBonus();
            case Status.StatName.Endurance:
                return 0;
            default:
                return 0;
        }
    }

    private int GetAtkBonus()
    {
        int atkBonus = 0;
        switch (OakLevel)
        {
            case 1:
                break;
            case 2:
            case 3:
            case 4:
            case 5:
            case 6:
                atkBonus += 5 * Player.Inst.equippedBodyPart.GetCntOfBodyPart(Race.Oak);
                break;
            case 7:
            case 8:
                atkBonus += 10 * Player.Inst.equippedBodyPart.GetCntOfBodyPart(Race.Oak);
                break;
            case 9:
            case 10:
                atkBonus += 10 * Player.Inst.equippedBodyPart.GetCntOfBodyPart(Race.Oak);
                atkBonus += 20;
                break;
        }
        if (ElfLevel >= 10)
        {
            atkBonus += (int)(Player.Inst.Mana * 0.3f);
        }
        else if (ElfLevel >= 4)
        {
            atkBonus += (int)(Player.Inst.Mana * 0.1f);
        }
        return atkBonus;
    }

    private int GetDefBonus()
    {
        int defBonus = 0;
        switch (OakLevel)
        {
            case 1:
                break;
            case 2:
            case 3:
                defBonus += 5 * Player.Inst.equippedBodyPart.GetCntOfBodyPart(Race.Oak);
                break;
            case 4:
            case 5:
            case 6:
                defBonus += 5 * Player.Inst.equippedBodyPart.GetCntOfBodyPart(Race.Oak);
                defBonus += 10;
                break;
            case 7:
            case 8:
            case 9:
            case 10:
                defBonus += 10 * Player.Inst.equippedBodyPart.GetCntOfBodyPart(Race.Oak);
                break;
        }
        if (ElfLevel >= 10)
        {
            defBonus += (int)(Player.Inst.Mana * 0.3f);
        }
        else if (ElfLevel >= 4)
        {
            defBonus += (int)(Player.Inst.Mana * 0.1f);
        }
        return defBonus;
    }
    
    private int GetDexBonus()
    {
        int dexBonus = 0;
        switch (GoblinLevel)
        {
            case 1:
                break;
            case 2:
            case 3:
                dexBonus += 2 * Player.Inst.equippedBodyPart.GetCntOfBodyPart(Race.Goblin);
                break;
            case 4:
            case 5:
                dexBonus += 4 * Player.Inst.equippedBodyPart.GetCntOfBodyPart(Race.Goblin);
                break;
            case 6:
                dexBonus += 4 * Player.Inst.equippedBodyPart.GetCntOfBodyPart(Race.Goblin);
                dexBonus += 5;
                break;
            case 7:
            case 8:
                dexBonus += 6 * Player.Inst.equippedBodyPart.GetCntOfBodyPart(Race.Goblin);
                dexBonus += 5;
                break;
            case 9:
                dexBonus += 6 * Player.Inst.equippedBodyPart.GetCntOfBodyPart(Race.Goblin);
                dexBonus += 15;
                break;
            case 10:
                dexBonus += 20 * Player.Inst.equippedBodyPart.GetCntOfBodyPart(Race.Goblin);
                dexBonus += 15;
                break;
        }
        return dexBonus;
    }

    private int GetManaBonus()
    {
        int manaBonus = 0;
        switch (GoblinLevel)
        {
            case 1:
                break;
            case 2:
            case 3:
            case 4:
            case 5:
                manaBonus += 3 * Player.Inst.equippedBodyPart.GetCntOfBodyPart(Race.Elf);
                break;
            case 6:
                manaBonus += 3 * Player.Inst.equippedBodyPart.GetCntOfBodyPart(Race.Elf);
                manaBonus += 10;
                break;
            case 7:
            case 8:
                manaBonus += 10 * Player.Inst.equippedBodyPart.GetCntOfBodyPart(Race.Elf);
                manaBonus += 10;
                break;
            case 9:
            case 10:
                manaBonus += 10 * Player.Inst.equippedBodyPart.GetCntOfBodyPart(Race.Elf);
                manaBonus += 30;
                break;
        }
        return manaBonus;
    }

    public int GetBonusLimit()
    {
        switch(MachineLevel)
        {
            case 1:
                return 0;
            case 2:
            case 3:
                return 5;
            case 4:
            case 5:
                return 15;
            case 6:
                return 30;
            case 7:
            case 8:
                return 50;
            case 9:
                return 75;
            case 10:
                return 100;
            default:
                return 0;
        }
    }

    public int GetBonusAffinity(Race race, BodyPartType bodyPartType)
    {
        if (race == Race.Oak)
        {
            if (bodyPartType == BodyPartType.Head || bodyPartType == BodyPartType.Body)
            {
                if (OakLevel == 10)
                {
                    return 3;
                }
            }
            else
            {
                if (OakLevel >= 6)
                    return 1;
            }
        }
        return 0;
    }
}
