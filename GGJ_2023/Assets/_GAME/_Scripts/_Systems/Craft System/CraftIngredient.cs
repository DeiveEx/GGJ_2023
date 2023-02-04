using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

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
    
    private bool _isPotion;

    public string IngredientName => _ingredientName;
    public IEnumerable<PropertySpec> Properties => _properties;
    public bool IsPotion => _isPotion;

    public CraftIngredient(string propertyName, IEnumerable<PropertySpec> properties, bool isPotion = false)
    {
        _ingredientName = propertyName;
        _properties = properties.ToList();
        _isPotion = isPotion;
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
