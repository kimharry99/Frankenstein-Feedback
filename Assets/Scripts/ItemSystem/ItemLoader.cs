using System.Collections.Generic;
using UnityEngine;

public class ItemLoader : SingletonBehaviour<ItemLoader>
{
    private Item[] allItems;
    private Dictionary<int, Item> dicIdItem = new Dictionary<int, Item>();

    protected override void Awake()
    {
        base.Awake(); 
        allItems = Resources.LoadAll<Item>("Items");
        for (int i = 0; i < allItems.Length; i++)
        {
            if(!dicIdItem.ContainsKey(allItems[i].id))
                dicIdItem.Add(allItems[i].id, allItems[i]);
            else
            {
                Debug.LogError("중복된 key : " + allItems[i].id + ", " + allItems[i].itemName);
            }
            //Debug.Log("add " + allItems[i].itemName);
        }
        //Debug.Log("3021 : " + GetItemById(3021).itemName);
        //Debug.Log("1124 : "+GetItemById(1124).itemName);
    }

    public Item GetItemById(int id)
    {
        Item loadedItem = null;
        if (dicIdItem.ContainsKey(id))
            loadedItem = dicIdItem[id];
        else
            Debug.LogError("key is not contained, key : " + id);
        //loadedItem = Resources.Load<Item>("Items/" + originItem.type.ToString() + "/" + originItem.itemName);
        return loadedItem;
    }
}
