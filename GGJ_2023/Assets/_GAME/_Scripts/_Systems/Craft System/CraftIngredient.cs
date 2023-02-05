using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public enum IngredientType
{
    None = 0,
    Ingredient = 1,
    Potion = 2,
}

[Serializable]
public class PropertySpec
{
    public IngredientPropertySO property;
    public int amount;
}

[Serializable]
public class CraftIngredient : Item
{
    [SerializeField] private List<PropertySpec> _properties = new();
    
    private IngredientType _ingredientType;

    public IEnumerable<PropertySpec> Properties => _properties;
    public IngredientType IngredientType => _ingredientType;

    public CraftIngredient(string ingredientName, IEnumerable<PropertySpec> properties, IngredientType ingredientType): base(ingredientName)
    {
        _properties = properties.ToList();
        _ingredientType = ingredientType;
    }

    public override string ToString()
    {
        StringBuilder sb = new($"Item name: {_itemName}");

        foreach (var property in _properties)
        {
            sb.Append($"\n- {property.property.PropertyName}: {property.amount}");
        }
        
        return sb.ToString();
    }
}
