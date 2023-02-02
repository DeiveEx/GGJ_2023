using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public enum IngredientProperty
{
    none = 0,
    property1 = 1,
    property2 = 2,
    property3 = 3,
    property4 = 4,
}

[Serializable]
public class PropertySpec
{
    public IngredientProperty property;
    public int amount;
}

[Serializable]
public class CraftIngredient
{
    [SerializeField] private string _ingredientName;
    [SerializeField] private List<PropertySpec> _properties = new();

    public string IngredientName => _ingredientName;
    public IEnumerable<PropertySpec> Properties => _properties;

    public CraftIngredient(string propertyName, IEnumerable<PropertySpec> properties)
    {
        _ingredientName = propertyName;
        _properties = properties.ToList();
    }

    public override string ToString()
    {
        StringBuilder sb = new($"Ingredient Name: {_ingredientName}");

        foreach (var property in _properties)
        {
            sb.Append($"\n- {property.property}: {property.amount}");
        }
        
        return sb.ToString();
    }
}
