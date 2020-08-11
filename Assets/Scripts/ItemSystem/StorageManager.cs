using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageManager : SingletonBehaviour<StorageManager>
{
    public Item none;

    public Inventory inventory;
    public Chest chest;

    public bool AddItem(Item item, Storage dest)
    {
        for (int i = 0; i < dest.slotItem.Length; i++)
        {
            if (dest.slotItem[i] != null)
            {
                if (dest.slotItem[i].id == item.id)
                {
                    dest.slotItemNumber[i]++;
                    return true;
                }
            }
        }
        int emptySlot = dest.GetFirstEmptySlot();
        if (emptySlot != -1)
        {
            dest.slotItem[emptySlot] = item;
            dest.slotItemNumber[emptySlot]++;
            return true;
        }
        return false;
    }
    public Item DeleteItem(int slotNumber, Storage dest)
    {
        if (slotNumber < 0 || slotNumber >= dest.slotItem.Length)
            return null;
        Item titem = dest.slotItem[slotNumber];
        if (titem != null)
        {
            dest.slotItemNumber[slotNumber]--;
            if (dest.slotItemNumber[slotNumber] > 0)
            {
                return titem;
            }
            else if (dest.slotItemNumber[slotNumber] == 0)
            {
                dest.slotItem[slotNumber] = null;
                return titem;
            }
            else
            {
                return null;
            }
        }
        else
        {
            return null;
        }
    }
    public void MoveItemToChest(int slotNumber)
    {
        Item _item = DeleteItem(slotNumber, inventory);
        if (_item == null)
            return;
        if (AddItem(_item, chest))
            return;
        return ;
    }

    public void MoveItemToInven(int slotNumber)
    {
        // to implement
    }
    
    // 인벤토리에 장착된 도구의 스텟 효과를 반환한다. 매개변수는 소문자를 사용한다.
    public int GetInventoryItemStat(string stat)
    {
        int itemStatToReturn = 0;
        for (int i = 0; i < inventory.slotItem.Length; i++)
        {
            if (inventory.slotItem[i].type == Type.Tool)
            {
                Tool tool = (Tool)inventory.slotItem[i];
                switch (stat)
                {
                    case "atk":
                        itemStatToReturn += tool.atk;
                        break;
                    case "def":
                        itemStatToReturn += tool.def;
                        break;
                    case "dex":
                        itemStatToReturn += tool.dex;
                        break;
                    case "mana":
                        itemStatToReturn += tool.mana;
                        break;
                    case "endurance":
                        itemStatToReturn += tool.endurance;
                        break;
                    default:
                        Debug.Log(stat + " : wrong stat name");
                        return -1;
                }
            }
        }
        return itemStatToReturn;
    }


    public void AddItemToChest(Item item)
    {
        AddItem(item, chest);
    }
    public void AddItemToInven(Item item)
    {
        AddItem(item, inventory);
    }

    // TODO : 아래 두 함수 구현
    //public Item DeleteFromChest(Item item);
    //public Item DeleteFromInven(Item item);
    // for debugging
    public void Foo()
    {
        Debug.Log("inven str : " + GetInventoryItemStat("str"));
        Debug.Log("inven def : " + GetInventoryItemStat("def"));
        Debug.Log("inven dex : " + GetInventoryItemStat("dex"));
    }
}
