using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PickUpItemEvent : RandomEncounterEvent
{
    [Header("Pick Up Item Event Field")]
    public Slot rewardItem;

    public override bool GetOptionEnable(int optionIndex)
    {
        if (optionIndex == 0)
        {
            if (StorageManager.Inst.inventory.GetFirstEmptySlot() != -1)
                return true;
            else
                return false;
        }
        return true;
    }

    public override void Option0()
    {
        PickUpItem();
        ExplorationUIManager.Inst.NoticeResultText(optionResultTexts[0]);
        FinishEvent(resultEvent[0]);
    }

    private void PickUpItem()
    {
        if(StorageManager.Inst.inventory.GetFirstEmptySlot() != -1)
        {
            StorageManager.Inst.AddItemsToInven(rewardItem.slotItem, rewardItem.slotItemNumber);
        }
    }

    public override void Option1() 
    {
        ExplorationUIManager.Inst.NoticeResultText(optionResultTexts[1]);
        FinishEvent(resultEvent[0]);
    }

    public override void Option2() { }

    public override void Option3() { }

    protected override bool GetIsEnabled()
    {
        return true;
    }

    protected override int GetOptionCaseNumber(int optionNumber)
    {
        return 0;
    }
}
