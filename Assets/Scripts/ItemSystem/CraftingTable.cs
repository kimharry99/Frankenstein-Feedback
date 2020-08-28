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
    private CraftingDirectory _craftingDirectory;
    /// <summary>
    /// 제작결과 아이템
    /// </summary>
    private Item _resultItem;

    // for debugging
    private List<int> debuggingIngredientItemIds = new List<int>();
    public Item[] item;

    private void Start()
    {
        _craftingDirectory = gameObject.AddComponent<CraftingDirectory>();
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

        if (ingredientItemIds == "")
        {
            HomeUIManager.Inst.panelNotice.SetActive(true);
            HomeUIManager.Inst.textNotice.text = "제작에 사용할 아이템을 선택하세요.";
            return;
        }

        _resultItem = _craftingDirectory.FindItem(ingredientItemIds);

        if (_resultItem)
        {
            for (int i = 0; i < 6; i++)
            {
                while (_usingItems[i].itemUsingCount > 0)
                {
                    _usingItems[i].itemUsingCount--;
                    StorageManager.Inst.DeleteFromChest(_usingItems[i].indexUsingChest);
                }
            }

            StorageManager.Inst.AddItemToChest(_resultItem);

            HomeUIManager.Inst.panelNotice.SetActive(true);
            HomeUIManager.Inst.textNotice.text = HomeUIManager.Inst.craftEnergy.ToString() + " 에너지를 소모하여\n"
                + _resultItem.itemName + " 아이템을 제작하였습니다.";

            GameManager.Inst.OnTurnOver(1);
            HomeUIManager.Inst.UpdateCrafting();
        }

        else
        {
            HomeUIManager.Inst.panelNotice.SetActive(true);
            HomeUIManager.Inst.textNotice.text = "해당하는 레시피가 없습니다.";
        }
    }

    /// <summary>
    /// usingItems의 아이템의 아이디를 읽어서 정렬된 상태로 ingredientItemIds에 저장한다.
    /// </summary>
    /// <param name="usingItems"></param>
    /// <param name="ingredientItemIds"></param>
    private string sortUsingItemsById(UsingItem[] usingItems)
    {
        List<int> itemIdList = new List<int>();
        for(int i = 0; i < 6; i++)
        {
            if (usingItems[i].indexUsingChest < 0)
                continue;

            for (int j = 0; j < usingItems[i].itemUsingCount; j++)
            {
                itemIdList.Add(usingItems[i].usingItem.id);
                debuggingIngredientItemIds.Add(usingItems[i].usingItem.id);
            }
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
