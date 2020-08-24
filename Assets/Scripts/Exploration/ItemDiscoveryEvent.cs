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
    public Item foundedItem;

    public override void Option0()
    {
        GetItem();
        ExplorationtManager.Inst.SelectNextEvent();
    }

    public override void Option1()
    {
        ExplorationtManager.Inst.SelectNextEvent();
    }

    public override void Option2()
    {
        Debug.Log(optionTexts[2] + "이 선택됨");
    }

    public override void Option3()
    {
        Debug.Log(optionTexts[3] + "이 선택됨");
    }

    private void GetItem()
    {
        StorageManager.Inst.AddItemToInven(foundedItem);
    }
}
