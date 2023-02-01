using System;
using System.Collections.Generic;
using UnityEngine;

public enum IngredientProperty
{
    none = 0,
    property_1 = 1,
    property_2 = 2,
    property_3 = 3,
}

[Serializable]
public class IngredientPropertyValue
{
    public IngredientProperty property;
    public int amount;
}

[Serializable]
public class CraftIngredient
{
    public string ingredientName;
    public List<IngredientPropertyValue> properties = new();
}

[CreateAssetMenu(fileName = "Ingredient", menuName = "Custom/New Ingredient")]
public class CraftIngredientSO : ScriptableObject
{
    [SerializeField] private CraftIngredient _ingredientInfo;

    public CraftIngredient IngredientInfo => _ingredientInfo;
}
