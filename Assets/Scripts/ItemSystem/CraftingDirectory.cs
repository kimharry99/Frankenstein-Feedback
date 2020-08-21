using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingDirectory
{
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
