using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CraftingTable : MonoBehaviour
{
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

    private UsingItem[] _usingItems = new UsingItem[6];
    /// <summary>
    /// 제작 레시피에 대한 객체
    /// </summary>
    private CraftingDirectory _craftingDirectory = new CraftingDirectory();
    /// <summary>
    /// 제작결과 아이템
    /// </summary>
    private Item _resultItem;

    // for debugging
    private List<int> debuggingIngredientItemIds = new List<int>();
    public Item[] item;

    private void Start()
    {
        // 아이템 레시피는 Dictionary로 이루어어져 있으며, Key로 아이템 id가 연결되어있는 string을 사용한다.
        // id는 오름차순정렬
        // 디버깅 할 때 itemRecpie에 원하는 조합법을 추가하면 된다.
        _craftingDirectory.itemRecipe.Add("40014201", item[0]);
    }

    #region usingItems Property

    /// <summary>
    /// Using Item과 Chest내의 아이템의 index를 구성한다.
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
        _usingItems[usingIndex].indexUsingChest = chestIndex;
        _usingItems[usingIndex].usingItem = StorageManager.Inst.chest.slotItem[chestIndex];
    }

    private void InitializeUsingItemSet(int usingIndex)
    {
        _usingItems[usingIndex].indexUsingChest = -1;
        _usingItems[usingIndex].usingItem = null;
        SetUsingItemCount(usingIndex, 0);
    }

    public void SetUsingItemCount(int usingIndex, int itemCount = 0, char setMode = 's')
    {
        _usingItems[usingIndex].itemUsingCount = itemCount;
    }

    public void AddUsingItemCount(int usingIndex, int value)
    {
        _usingItems[usingIndex].itemUsingCount += value;
    }

    #endregion

    // 진웅 TODO : 아이템 창고로 추가, using item삭제 기능 추가
    /// <summary>
    /// 아이템 제작을 실행한다.
    /// </summary>
    public void CraftItem()
    {
        debuggingIngredientItemIds.Clear();
        string ingredientItemIds = sortUsingItemsById(_usingItems);
        _resultItem = _craftingDirectory.FindItem(ingredientItemIds);
    }

    /// <summary>
    /// usingItems의 아이템의 아이디를 읽어서 정렬된 상태로 ingredientItemIds에 저장한다.
    /// </summary>
    /// <param name="usingItems"></param>
    /// <param name="ingredientItemIds"></param>
    private string sortUsingItemsById(UsingItem[] usingItems)
    {
        List<int> itemIdList = new List<int>();
        for(int i=0;usingItems[i].indexUsingChest >= 0; i++)
        {
            itemIdList.Add(usingItems[i].usingItem.id);
            debuggingIngredientItemIds.Add(usingItems[i].usingItem.id);
        }
        itemIdList.Sort();
        debuggingIngredientItemIds.Sort();
        string rString = "";
        foreach(int itemIngredient in itemIdList)
        {
            rString += itemIngredient.ToString();
        }
        return rString;
    }
}
