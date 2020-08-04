using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Type
{
    Corpse,
    Consumable,
    Tool,
    Ingredient
}
public enum Grade
{
    A,
    B,
    C,
    D
}
[CreateAssetMenu]
public class Item : ScriptableObject
{ 
    public int id;
    public string name;
    public string description;
    public Texture2D itemImage;

    public Type type;
    public Grade grade;
    public int energyPotential;
}
