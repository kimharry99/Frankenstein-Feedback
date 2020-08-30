using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Slot
{
    public Item slotItem;
    public int slotItemNumber;
}

public class Storage : ScriptableObject
{
    public Item[] slotItem;
    public int[] slotItemNumber;

    public virtual int GetFirstEmptySlot()
    {
        for (int i = 0; i < slotItem.Length; i++)
        {
            if (slotItem[i] == null)
            {
                return i;
            }
        }
        return -1;
    }
    public virtual int GetLastEmptySlot()
    {
        for (int i = slotItem.Length - 1; i >= 0; i--)
        {
            if (slotItem[i] == null)
            {
                return i;
            }
        }
        return -1;
    }

    //public virtual bool AddItem(Item item)
    //{
    //    for (int i = 0; i < slotItem.Length; i++)
    //    {
    //        if (slotItem[i] != null)
    //        {
    //            if (slotItem[i].id == item.id)
    //            {
    //                slotItemNumber[i]++;
    //                return true;
    //            }
    //        }
    //    }
    //    int emptySlot = GetFirstEmptySlot();
    //    if (emptySlot != -1)
    //    {
    //        slotItem[emptySlot] = item;
    //        slotItemNumber[emptySlot]++;
    //        return true;
    //    }
    //    return false;
    //}
    //public virtual Item DeleteItem(int slotNumber)
    //{
    //    Item titem = slotItem[slotNumber];
    //    if (titem != null)
    //    {
    //        slotItemNumber[slotNumber]--;
    //        if (slotItemNumber[slotNumber] > 0)
    //        {
    //            return titem;
    //        }
    //        else if (slotItemNumber[slotNumber] == 0)
    //        { 
    //            slotItem[slotNumber] = null;
    //            return titem;
    //        }
    //        else
    //        {
    //            return null;
    //        }
    //    }
    //    else
    //    {
    //        return null;
    //    }
    //}
    //public virtual  void MoveItem(int slotNumber, Storage dest)
    //{
    //    Item _item = DeleteItem(slotNumber);
    //    if (_item == null)
    //        return;
    //    if (dest.AddItem(_item))
    //        return;
    //    return;
    //}
}
