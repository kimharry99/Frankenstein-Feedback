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
}
