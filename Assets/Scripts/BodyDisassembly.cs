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
                BodyPart dissassembled = (BodyPart)StorageManager.Inst.DeleteFromChest(slotNumber);
                GetBonusItem(dissassembled);
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

    private void GetBonusItem(BodyPart item)
    {
        BonusItemTable bonusItemTable = new BonusItemTable();
        int probabilityIndex = GetRandomIndex();
        Slot[] rewardItems = bonusItemTable.GetTableData(item.race, item.grade, probabilityIndex);
        StorageManager.Inst.AddItemsToChest(rewardItems[0].slotItem, rewardItems[0].slotItemNumber);
        StorageManager.Inst.AddItemsToChest(rewardItems[1].slotItem, rewardItems[1].slotItemNumber);
    }

    // 승윤 TODO : 
    /// <summary>
    /// 다음의 확률대로 값을 반환한다. 50% : 0, 30% : 1, 15% : 2, 5% : 3
    /// </summary>
    /// <returns></returns>
    private int GetRandomIndex()
    {
        return 0;
    }

}
