using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class CraftingDirectory : MonoBehaviour
{
    [Serializable]
    public struct Recipie
    {
        public string itemName;
        public string ingredientItemIds;
        public int resultItemId;
    }

    [Serializable]
    private class Serialization<T>
    {
        [SerializeField]
        List<T> target;
        public List<T> ToList() { return target; }

        public Serialization(List<T> target)
        {
            this.target = target;
        }
    }

    /// <summary>
    /// key값 : 재료 아이템의 Sorted list, value값 : 결과 아이템
    /// </summary>
    public Dictionary<string, Item> dicItemRecipie = new Dictionary<string, Item>();
    public List<Recipie> _recipies;

    public CraftingDirectory()
    {
        //for debugging
        //_recipies = new List<Recipie>();
        //Recipie recipie0;
        //recipie0.itemName = "권총";
        //recipie0.ingredientItemIds = "400240024003";
        //recipie0.resultItemId = 3021;
        //_recipies.Add(recipie0);
        //_recipies.Add(recipie0);
        //Debug.Log(JsonUtility.ToJson(new Serialization<Recipie>(_recipies), true));

        string loadPath = "Assets/Data";
        string fileName = "CraftingRecipies";
        _recipies = LoadJsonFile<Serialization<Recipie>>(loadPath, fileName).ToList();
        AddRecipieToDictionary(_recipies, dicItemRecipie);
    }

    /// <summary>
    /// ingreditnItemIDs으로 제작할 수 있는 아이템을 반환한다.
    /// </summary>
    /// <param name="ingredientItemIDs"></param>
    /// <returns></returns>
    public Item FindItem(string ingredientItemIDs)
    {
        if(dicItemRecipie.ContainsKey(ingredientItemIDs))
        {
            Debug.Log(dicItemRecipie.ContainsKey(ingredientItemIDs));
            return dicItemRecipie[ingredientItemIDs];
        }
        Debug.Log(dicItemRecipie.ContainsKey(ingredientItemIDs));
        return null;
    }

    private T LoadJsonFile<T>(string loadPath, string fileName)
    {
        FileStream fileStream = new FileStream(string.Format("{0}/{1}.json", loadPath, fileName), FileMode.Open);
        byte[] data = new byte[fileStream.Length];
        fileStream.Read(data, 0, data.Length);
        fileStream.Close();
        string jsonData = Encoding.UTF8.GetString(data);
        return JsonUtility.FromJson<T>(jsonData);
    }

    private void AddRecipieToDictionary(List<Recipie> recipies, Dictionary<string, Item> _itemRecipie)
    {
        Debug.Log(recipies.Count);
        foreach(Recipie r in recipies)
        {
            int itemId = r.resultItemId;
            Item item = FindItemByID(itemId);
            Debug.Log(r.ingredientItemIds);
            if (!_itemRecipie.ContainsKey(r.ingredientItemIds))
                _itemRecipie.Add(r.ingredientItemIds, item);
            else
                Debug.LogError("key is already exist, key : " + r.ingredientItemIds);
        }
    }

    private Item FindItemByID(int itemId)
    {
        ItemLoader itemLoader = ItemLoader.Inst;
        return itemLoader.GetItemById(itemId);
    }
}
