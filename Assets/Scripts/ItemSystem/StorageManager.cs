using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageManager : SingletonBehaviour<StorageManager>
{
    public Item none;
    //public Chest[] chests;
    public Sprite emptyImage;

    public Inventory inventory;
    public Chest chest;

    protected override void Awake()
    {
        base.Awake();
        UpdateChestIndexes();
    }
    #region Chest Indexes
    // UI에서 참조하기 위한 index
    public int[] _indexTableBodypart = new int[Chest.CAPACITY];
    private int[] _indexTableConsumable = new int[Chest.CAPACITY];
    private int[] _indexTableTool = new int[Chest.CAPACITY];
    private int[] _indexTableIngredient = new int[Chest.CAPACITY];
    public int GetIndexTable(Type itemType, int uiIndex)
    {
        switch (itemType)
        {
            case Type.All:
                return uiIndex;
            case Type.BodyPart:
                return _indexTableBodypart[uiIndex];
            case Type.Consumable:
                return _indexTableConsumable[uiIndex];
            case Type.Ingredient:
                return _indexTableIngredient[uiIndex];
            case Type.Tool:
                return _indexTableTool[uiIndex];
            default:
                Debug.Log("wrong item Type");
                return -1;
        }
    }
    private void UpdateChestIndexes()
    {
        int indexBodyPart = 0, indexConsumable = 0, indexTool = 0, indexIngredient = 0;
        for (int indexChest = 0; indexChest < chest.slotItem.Length; indexChest++)
        {
            if (chest.slotItem[indexChest] != null)
            {
                switch (chest.slotItem[indexChest].type)
                {
                    case Type.BodyPart:
                        _indexTableBodypart[indexBodyPart++] = indexChest;
                        break;
                    case Type.Consumable:
                        _indexTableConsumable[indexConsumable++] = indexChest;
                        break;
                    case Type.Ingredient:
                        _indexTableIngredient[indexIngredient++] = indexChest;
                        break;
                    case Type.Tool:
                        _indexTableTool[indexTool++] = indexChest;
                        break;
                    default:
                        Debug.Log("wrong!");
                        return;
                }
            }
        }
        while(indexBodyPart<_indexTableBodypart.Length)
        {
            _indexTableBodypart[indexBodyPart++] = -1;
        }
        while (indexConsumable < _indexTableConsumable.Length)
        {
            _indexTableConsumable[indexConsumable++] = -1;
        }
        while (indexIngredient < _indexTableIngredient.Length)
        {
            _indexTableIngredient[indexIngredient++] = -1;
        }
        while (indexTool < _indexTableTool.Length)
        {
            _indexTableTool[indexTool++] = -1;
        }
    }
    #endregion

    #region Delete Methods
    /// <summary>
    /// Storage dest의 slotNumber번째 아이템을 하나 삭제한다.
    /// </summary>
    /// <param name="slotNumber"></param>
    /// <param name="dest"></param>
    /// <returns></returns>
    private Item DeleteItem(int slotNumber, Storage dest)
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

    public Item DeleteFromChest(int slotNumber)
    {
        Item item = DeleteItem(slotNumber, chest);
        SortChestItem();
        UpdateChestIndexes();
        if (HomeUIManager.Inst.panelChest)
        {
            HomeUIManager.Inst.UpdateChest();
        }
        return item;
    }
    public Item DeleteFromInven(int slotNumber)
    {
        Item item = DeleteItem(slotNumber, inventory);
        UpdateToolStat();
        GeneralUIManager.Inst.UpdateInventory();
        return item;
    }
    #endregion

    #region Add Methods
    /// <summary>
    /// dest Stroage의 item을 추가한다. item이 dest에 존재하지 않으면, 마지막 slot에 추가한다.
    /// </summary>
    private bool AddItem(Item item, Storage dest)
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

    public bool AddItemToChest(Item item)
    {
        bool isSuccess = AddItem(item, chest);
        UpdateChestIndexes();
        if (HomeUIManager.Inst.panelChest)
        {
            HomeUIManager.Inst.UpdateChest();
        }
        return isSuccess;
    }

    public bool AddItemToInven(Item item)
    {
        bool isSuccess = AddItem(item, inventory);
        UpdateToolStat();
        GeneralUIManager.Inst.UpdateInventory();
        return isSuccess;
    }

    #endregion

    #region Move Methods
    /// <summary>
    /// 인벤토리의 slotNumber번째 아이템을 창고로 이동한다.
    /// </summary>
    /// <param name="slotNumber"></param>
    public void MoveItemToChest(int slotNumber)
    {
        Item _item = DeleteFromInven(slotNumber);
        if (_item == null)
            return;
        AddItemToChest(_item);
    }

    /// <summary>
    /// chest의 slotNumber번째 아이템을 인벤으로 이동한다.
    /// </summary>
    public void MoveItemToInven(int slotNumber)
    {
        Item _item = DeleteFromChest(slotNumber);
        if (_item == null)
            return;
        AddItemToInven(_item);
    }

    #endregion

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

    #region Inventory Status Methods
    public Status toolStat;
    /// <summary>
    /// 인벤토리에 장착된 도구의 스텟 효과를 반환한다. 매개변수는 소문자를 사용한다.
    /// </summary>
    /// <param name="stat"></param>
    /// <returns></returns>
    private int GetInventoryItemStat(string stat)
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

    /// <summary>
    /// 인벤토리에 변화가 있을 때마다 호출해서 장비의 스텟효과를 갱신한다. 
    /// </summary>
    private void UpdateToolStat()
    {
        toolStat.atk = GetInventoryItemStat("atk");
        toolStat.def = GetInventoryItemStat("def");
        toolStat.dex = GetInventoryItemStat("dex");
        toolStat.mana = GetInventoryItemStat("mana");
        toolStat.endurance = GetInventoryItemStat("endurance");
    }
    #endregion
    
    // 승윤 TODO
    /// <summary>
    /// Storage dest에서 item을 찾아서 그 아이템의 slotNumber를 반환한다.
    /// </summary>
    private int FindSlotNumberById(Item item, Storage dest)
    {
        return 0;
    }

    // 승윤 TODO : 메소드 구현
    /// <summary>
    /// chest의 아이템을 빈 칸이 없도록 정렬한다.
    /// </summary>
    private void SortChestItem()
    {

    }

    // for debugging
    public void Foo()
    {
        UpdateChestIndexes();
    }
}
