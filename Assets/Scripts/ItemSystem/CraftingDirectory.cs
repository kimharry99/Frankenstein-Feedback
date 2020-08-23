using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class CraftingDirectory
{
    [Serializable]
    public struct Recipie
    {
        public string itemName;
        public string ingredientItemIds;
        public int resultItemId;
    }
    /// <summary>
    /// key값 : 재료 아이템의 Sorted list, value값 : 결과 아이템
    /// </summary>
    public Dictionary<string, Item> itemRecipe = new Dictionary<string, Item>();

    public Item FindItem(string ingredientItemIDs)
    {
        if(itemRecipe.ContainsKey(ingredientItemIDs))
        {
            Debug.Log(itemRecipe.ContainsKey(ingredientItemIDs));
            return itemRecipe[ingredientItemIDs];
        }
        Debug.Log(itemRecipe.ContainsKey(ingredientItemIDs));
        return null;
    }
}
