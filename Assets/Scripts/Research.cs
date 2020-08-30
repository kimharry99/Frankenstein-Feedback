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
}
