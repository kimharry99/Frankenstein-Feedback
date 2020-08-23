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
        string fileName = "123";
        _recipies = LoadJsonFile<Serialization<Recipie>>(loadPath, fileName).ToList();
        //AddRecipieToDictionary(_recipies, dicItemRecipie);
    }
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

    private T LoadJsonFile<T>(string loadPath, string fileName)
    {
        FileStream fileStream = new FileStream(string.Format("{0}/{1}.json", loadPath, fileName), FileMode.Open);
        byte[] data = new byte[fileStream.Length];
        fileStream.Read(data, 0, data.Length);
        fileStream.Close();
        string jsonData = Encoding.UTF8.GetString(data);
        return JsonUtility.FromJson<T>(jsonData);
    }
}
