using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StorageManager : SingletonBehaviour<StorageManager>
{
    public Item none;
    public Inventory inventory;
    public Chest[] chests;
    public Chest chest;
    public Sprite emptyImage;
    public HomeUIManager homeUIManager;
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
    public void DeleteItemById(int itemId, Storage dest)
    {
        for(int i = 0; i < dest.slotItem.Length; i++)
        {
            if (dest.slotItem[i] == null)
                continue;
            if(dest.slotItem[i].id == itemId)
            {
                dest.slotItemNumber[i]--;
                if (dest.slotItemNumber[i] == 0)
                    dest.slotItem[i] = null;
                return;
            }
        }
        return;
    }
    public void UpdateChest(int constraint)
    {
        homeUIManager = GameObject.Find("UI").GetComponent<HomeUIManager>();
        if (chest.slotItem[constraint] != null)
            homeUIManager.imageChest[constraint].image.sprite = chest.slotItem[constraint].itemImage;
        else
            homeUIManager.imageChest[constraint].image.sprite = emptyImage;
    }
    public void MoveItemToChest(int slotNumber)
    {
        Item _item = DeleteItem(slotNumber, inventory);
        if (_item == null)
            return;
        AddItem(_item, chests[(int)_item.type+1]);
        if (AddItem(_item, chests[0]))
            return;
        return ;
    }

    public void MoveItemToInven(int slotNumber)
    {
        Item _item = DeleteItem(slotNumber, chest);
        if (_item == null)
            return;
        if (chest == chests[0])
            DeleteItemById(_item.id, chests[(int)_item.type + 1]);
        else
            DeleteItemById(_item.id, chests[0]);
        if (AddItem(_item, inventory))
            return;
        return;
    }
    public void InitialUpdateChest()
    {
        chest = chests[0];
        homeUIManager = GameObject.Find("UI").GetComponent<HomeUIManager>();
        for (int i = 0; i < chest.slotItem.Length; i++)
        {
            if (chest.slotItem[i] != null)
                homeUIManager.imageChest[i].image.sprite = chest.slotItem[i].itemImage;
            else
                homeUIManager.imageChest[i].image.sprite = emptyImage;
        }
    }
    public void SortItem(int num)
    {
        homeUIManager = GameObject.Find("UI").GetComponent<HomeUIManager>();
        chest = chests[num];
        for (int i = 0; i < chest.slotItem.Length; i++)
        {
            if (chest.slotItem[i] != null)
               homeUIManager.imageChest[i].image.sprite  = chest.slotItem[i].itemImage;
            else
                homeUIManager.imageChest[i].image.sprite = emptyImage;
        }
    }
}
