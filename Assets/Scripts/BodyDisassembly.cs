using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyDisassembly : MonoBehaviour
{
    public void DisassembleItem(/* some parameters */)
    {
        int count = 0;

        for (int i = 0; i < 30; i++)
        {
            if (HomeUIManager.Inst.imageCheck[i].activeSelf == true)
            {
                count++;
                int slotNumber = HomeUIManager.Inst.indexHoldingChest[i];
                // TODO : dest를 StorageManager로 수정
                StorageManager.Inst.DeleteFromChest(slotNumber);
            }
        }

        if (count == 0)
        {
            HomeUIManager.Inst.panelNotice.SetActive(true);
            HomeUIManager.Inst.textNotice.text = "분해할 사체를 선택하세요.";
            return;
        }

        GeneralUIManager.Inst.energy.value += HomeUIManager.Inst.disassembleEnergy;
        GeneralUIManager.Inst.UpdateEnergy();

        HomeUIManager.Inst.UpdateDisassemble();

        GameManager.Inst.OnTurnOver(1);
    }
}
