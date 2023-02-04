using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public enum ItemType
{
    None = 0,
    Ingredient = 1,
    Potion = 2,
    Seed = 3,
}

[Serializable]
public class PropertySpec
{
    public IngredientPropertySO property;
    public int amount;
}

[Serializable]
public class CraftIngredient
{
    [SerializeField] private string _ingredientName;
    [SerializeField] private List<PropertySpec> _properties = new();
    
    private ItemType _itemType;

    public string IngredientName => _ingredientName;
    public IEnumerable<PropertySpec> Properties => _properties;
    public ItemType ItemType => _itemType;

    public CraftIngredient(string propertyName, IEnumerable<PropertySpec> properties, ItemType itemType)
    {
        _ingredientName = propertyName;
        _properties = properties.ToList();
        _itemType = itemType;
    }

    public override string ToString()
    {
        StringBuilder sb = new($"Item name: {_ingredientName}");

        foreach (var property in _properties)
        {
            sb.Append($"\n- {property.property.PropertyName}: {property.amount}");
        }
        
        return sb.ToString();
    }
}
