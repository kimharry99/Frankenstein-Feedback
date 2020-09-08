using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

public class BodyDisassembly : MonoBehaviour
{
    public const int ITEM_CAPACITY = 6;
    /// <summary>
    /// 0: 가죽, 1: 고철, 2: 금가루, 3: 자연의 정수, 4: 질긴 가죽, 5: 기계 부품
    /// </summary>
    private int[] _bonusItemNum = new int[6];

    public void DisassembleItem(/* some parameters */)
    {
        int count = 0;
        StorageManager sm = StorageManager.Inst;
        ResetBonusItemNum();

        for (int i = 29; i >= 0; i--)
        {
            if (HomeUIManager.Inst.imageCheck[i].activeSelf == true)
            {
                count++;
                //int slotNumber = HomeUIManager.Inst.indexHoldingChest[i];
                int slotNumber = sm.GetIndexFromTable(Type.BodyPart, i);
                // TODO : dest를 StorageManager로 수정
                BodyPart dissassembled = (BodyPart)sm.DeleteFromChest(slotNumber);
                if(dissassembled == null)
                {
                    Debug.LogError("dissassembled is null");
                }
                else
                    GetBonusItem(dissassembled);
            }
        }

        if (count == 0)
        {
            HomeUIManager.Inst.panelNotice.SetActive(true);
            HomeUIManager.Inst.textNotice.text = "분해할 사체를 선택하세요.";
            return;
        }

        NoticeDisassemble();
        ResetBonusItemNum();

        GeneralUIManager.Inst.energy.value += HomeUIManager.Inst.disassembleEnergy;
        GeneralUIManager.Inst.UpdateEnergy();

        HomeUIManager.Inst.UpdateDisassemble();

        GameManager.Inst.OnTurnOver(1);
    }

    private void ResetBonusItemNum()
    {
        for (int i = 0; i < 6; i++)
        {
            _bonusItemNum[i] = 0;
        }
    }

    public void GetBonusItem(BodyPart item)
    {
        if (item == null)
        {
            Debug.LogError("item is null");
        }
        BonusItemTable bonusItemTable = new BonusItemTable();
        int probabilityIndex = GetRandomIndex();
        Debug.Log("probabilityIndex:" + probabilityIndex);
        Slot[] rewardItems = bonusItemTable.GetTableData(item.race, item.grade, probabilityIndex);
        StorageManager.Inst.AddItemsToChest(rewardItems[0].slotItem, rewardItems[0].slotItemNumber);
        StorageManager.Inst.AddItemsToChest(rewardItems[1].slotItem, rewardItems[1].slotItemNumber);

        int index = -1;
        if (rewardItems[0].slotItem != null)
        {
            index = (rewardItems[0].slotItem.id % 10 == 1) ? (rewardItems[0].slotItem.id / 100 % 10) : 1;
            _bonusItemNum[index] += rewardItems[0].slotItemNumber;
        }
        if (rewardItems[1].slotItem != null)
        {
            index = (rewardItems[1].slotItem.id % 10 == 1) ? (rewardItems[1].slotItem.id / 100 % 10) : 1;
            _bonusItemNum[index] += rewardItems[1].slotItemNumber;
        }
    }

    // 승윤 TODO : 
    /// <summary>
    /// 다음의 확률대로 값을 반환한다. 50% : 0, 30% : 1, 15% : 2, 5% : 3
    /// Random.Range 프로퍼티를 사용
    /// </summary>
    /// <returns></returns>
    private int GetRandomIndex()
    {
        int ran = Random.Range(0, 20);
        if (ran < 10)
        {
            return 0;
        }
        else if (ran < 16)
        {
            return 1;
        }
        else if (ran < 19)
        {
            return 2;
        }
        else
        {
            return 3;
        }
    }

    private void NoticeDisassemble()
    {
        HomeUIManager.Inst.panelNotice.SetActive(true);
        HomeUIManager.Inst.textNotice.text = "";
        if (_bonusItemNum[0] > 0)
            HomeUIManager.Inst.textNotice.text += "가죽 " + _bonusItemNum[0] + "개, ";
        if (_bonusItemNum[1] > 0)
            HomeUIManager.Inst.textNotice.text += "고철 " + _bonusItemNum[1] + "개, ";
        if (_bonusItemNum[2] > 0)
            HomeUIManager.Inst.textNotice.text += "금가루 " + _bonusItemNum[2] + "개, ";
        if (_bonusItemNum[3] > 0)
            HomeUIManager.Inst.textNotice.text += "자연의 정수 " + _bonusItemNum[3] + "개, ";
        if (_bonusItemNum[4] > 0)
            HomeUIManager.Inst.textNotice.text += "질긴 가죽 " + _bonusItemNum[4] + "개, ";
        if (_bonusItemNum[5] > 0)
            HomeUIManager.Inst.textNotice.text += "기계 부품 " + _bonusItemNum[5] + "개, ";
        HomeUIManager.Inst.textNotice.text += HomeUIManager.Inst.disassembleEnergy.ToString() + " 에너지를 획득했습니다.";
    }
}
