using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 아이템 발견 이벤트 Option은 항상 다음 조건을 만족한다. 
/// <para/>Option0 : 아이템을 얻는다. Option1 : 아이템을 얻지 않는다.
/// </summary>
[CreateAssetMenu]
public class ItemDiscoveryEvent : ExplorationEvent
{
    [Header("ItemDIscoveryEvent Field")]
    public Item foundItem;

    public override bool IsEnabled { get; set; }

    public override bool GetOptionEnable(int optionIndex)
    {
        if(optionIndex == 0)
        {
            if(foundItem != null)
                return StorageManager.Inst.inventory.GetFirstEmptySlot() != -1;
            return true;
        }
        else
        {
            return true;
        }
    }

    public override void Option0()
    {
        ExplorationUIManager.Inst.NoticeResultText(optionResultTexts[0]);
        if (GetItem())
            FinishEvent();
    }

    public override void Option1()
    {
        ExplorationUIManager.Inst.NoticeResultText(optionResultTexts[1]);
        FinishEvent();
    }

    public override void Option2()
    {
        ExplorationUIManager.Inst.NoticeResultText(optionResultTexts[2]);
        Debug.Log(optionTexts[2] + "이 선택됨");
        FinishEvent();
    }

    public override void Option3()
    {
        Debug.Log(optionTexts[3] + "이 선택됨");
        FinishEvent();
    }

    protected override void FinishEvent(ExplorationEvent nextEvent = null, bool isReturnHome = false)
    {
        ExplorationManager.Inst.FinishEvent(phase, nextEvent, isReturnHome);
    }

    protected override bool GetIsEnabled()
    {
        return true;
    }

    private bool GetItem()
    {
        if(foundItem != null)
        {
            return StorageManager.Inst.AddItemToInven(foundItem);
        }
        // 아이템 발견을 실패한 경우
        return true;
    }

    [ContextMenu("Make Event \'을\'")]
    private void MakeEvent1()
    {
        if(foundItem != null)
        {
            eventName = foundItem.itemName + "획득";
            phase = EventPhase.SearchingItem;
            titleText = foundItem.itemName + "을 발견했다.";
            optionTexts = new List<string>();
            optionTexts.Add("가져간다");
            optionTexts.Add("버린다");
            optionResultTexts = new List<string>();
            optionResultTexts.Add(foundItem.itemName + "을 획득했다.");
            optionResultTexts.Add("");
        }
    }

    [ContextMenu("Make Event \'를\'")]
    private void MakeEvent2()
    {
        if (foundItem != null)
        {
            eventName = foundItem.itemName + "획득";
            phase = EventPhase.SearchingItem;
            titleText = foundItem.itemName + "를 발견했다.";
            optionTexts = new List<string>();
            optionTexts.Add("가져간다");
            optionTexts.Add("버린다");
            optionResultTexts = new List<string>();
            optionResultTexts.Add(foundItem.itemName + "를 획득했다.");
            optionResultTexts.Add("");
        }
    }
}
