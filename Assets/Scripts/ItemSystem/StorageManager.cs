using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageManager : SingletonBehaviour<StorageManager>
{
    public Item none;
    public Chest[] chests;
    public Sprite emptyImage;
    private HomeUIManager _homeUIManager = HomeUIManager.Inst;

    public Inventory inventory;
    public Chest chest;

    // UI에서 참조하기 위한 index
    public int[] _indexTableBodypart = new int[30];
    public int GetIndexBodyPart(int uiIndex)
    {
        return _indexTableBodypart[uiIndex];
    }
    public int[] _indexTableConsumable = new int[30];
    public int GetIndexConsumable(int uiIndex)
    {
        return _indexTableConsumable[uiIndex];
    }
    public int[] _indexTableTool = new int[30];
    public int GetIndexTool(int uiIndex)
    {
        return _indexTableTool[uiIndex];
    }
    public int[] _indexTableIngredient = new int[30];
    public int GetIndexIngredient(int uiIndex)
    {
        return _indexTableIngredient[uiIndex];
    }

    private void UpdateChestIndexes()
    {
        int indexBodyPart=0, indexConsumable=0, indexTool=0, indexIngredient = 0;
        for (int i = 0; i < chest.slotItem.Length; i++)
        {
            if(chest.slotItem[i] != null)
                switch (chest.slotItem[i].type)
                {
                    case Type.BodyPart:
                        _indexTableBodypart[indexBodyPart++] = i;
                        break;
                    case Type.Consumable:
                        _indexTableConsumable[indexConsumable++] = i;
                        break;
                    case Type.Ingredient:
                        _indexTableIngredient[indexIngredient++] = i;
                        break;
                    case Type.Tool:
                        _indexTableTool[indexTool++] = i;
                        break;
                    default:
                        Debug.Log("wrong!");
                        return;
                }
        }
    }
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
        for (int i = 0; i < dest.slotItem.Length; i++)
        {
            if (dest.slotItem[i] == null)
                continue;
            if (dest.slotItem[i].id == itemId)
            {
                dest.slotItemNumber[i]--;
                if (dest.slotItemNumber[i] == 0)
                    dest.slotItem[i] = null;
                return;
            }
        }
        return;
    }
    public void MoveItemToChest(int slotNumber)
    {
        Item _item = DeleteItem(slotNumber, inventory);
        if (_item == null)
            return;
        AddItem(_item, chests[(int)_item.type + 1]);
        if (AddItem(_item, chests[0]))
        {
            UpdateToolStat();
            return;
        }
        return ;
    }

    // chest의 slotNumber번째 아이템을 인벤 마지막 자리로 이동한다.
    public void MoveItemToInven(int slotNumber)
    {
        // to implement
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
    //public void UpdateChest(int constraint)
    //{
    //    if (chest.slotItem[constraint] != null)
    //        _homeUIManager.imageChest[constraint].image.sprite = chest.slotItem[constraint].itemImage;
    //    else
    //        _homeUIManager.imageChest[constraint].image.sprite = emptyImage;
    //}
    //public void InitialUpdateChest()
    //{
    //    chest = chests[0];
    //    for (int i = 0; i < chest.slotItem.Length; i++)
    //    {
    //        if (chest.slotItem[i] != null)
    //            _homeUIManager.imageChest[i].image.sprite = chest.slotItem[i].itemImage;
    //        else
    //            _homeUIManager.imageChest[i].image.sprite = emptyImage;
    //    }
    //}
    //public void SortItem(int num)
    //{
    //    homeUIManager = GameObject.Find("UI").GetComponent<HomeUIManager>();
    //    chest = chests[num];
    //    for (int i = 0; i < chest.slotItem.Length; i++)
    //    {
    //        if (chest.slotItem[i] != null)
    //            homeUIManager.imageChest[i].image.sprite = chest.slotItem[i].itemImage;
    //        else
    //            homeUIManager.imageChest[i].image.sprite = emptyImage;
    //    }
    //}

    // 인벤토리에 장착된 도구의 스텟 효과를 반환한다. 매개변수는 소문자를 사용한다.
    public int GetInventoryItemStat(string stat)
    {
        int itemStatToReturn = 0;
        for (int i = 0; i < inventory.slotItem.Length; i++)
        {
            if (inventory.slotItem[i] == null)
                continue;
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

    public Status toolStat;
    // 인벤토리에 변화가 있을 때마다 호출해서 장비의 스텟효과를 갱신한다. 
    private void UpdateToolStat()
    {
        toolStat.atk = GetInventoryItemStat("atk");
        toolStat.def = GetInventoryItemStat("def");
        toolStat.dex = GetInventoryItemStat("dex");
        toolStat.mana = GetInventoryItemStat("mana");
        toolStat.endurance = GetInventoryItemStat("endurance");
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
        UpdateChestIndexes();
    }
}
