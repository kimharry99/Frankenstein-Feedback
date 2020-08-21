using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
struct UsingItem
{
    public Item usingItem;
    /// <summary>
    /// 작업에 사용된 아이템과 창고내의 아이템 번호를 연결하는 index
    /// </summary>
    public int indexUsingChest;
    public int itemUsingCount;
}



public class CraftingTable : MonoBehaviour
{
    private UsingItem[] usingItems = new UsingItem[6];

    /// <summary>
    /// 
    /// </summary>
    /// <param name="chestSlot"></param>
    public void SetIndexUsingChest(int usingIndex, int chestIndex)
    {
        if(chestIndex > 0)
        {
            IndexingUsingAndChest(usingIndex, chestIndex);
        }
        else
        {
            InitializeUsingItemSet(usingIndex);
        }
    }

    private void IndexingUsingAndChest(int usingIndex, int chestIndex)
    {
        usingItems[usingIndex].indexUsingChest = chestIndex;
        usingItems[usingIndex].usingItem = StorageManager.Inst.chest.slotItem[chestIndex];
    }

    private void InitializeUsingItemSet(int usingIndex)
    {
        usingItems[usingIndex].indexUsingChest = -1;
        usingItems[usingIndex].usingItem = null;
    }

    public void SetUsingItemCount(int usingIndex, int itemCount = 0, char setMode = 's')
    {
        usingItems[usingIndex].itemUsingCount = itemCount;
    }

    public void AddUsingItemCount(int usingIndex, int value)
    {
        usingItems[usingIndex].itemUsingCount += value;
    }

}
