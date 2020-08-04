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
    public new string name;
    public string description;
    public Sprite itemImage;

    public Type type;
    public Grade grade;
    public int energyPotential;
}
