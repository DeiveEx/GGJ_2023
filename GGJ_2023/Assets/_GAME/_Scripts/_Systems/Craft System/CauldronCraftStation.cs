using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CauldronCraftStation : MonoBehaviour
{
    private List<CraftIngredient> _currentIngredients = new();
    private Dictionary<IngredientPropertySO, PropertySpec> _currentProperties = new();

    public IEnumerable<CraftIngredient> CurrentIngredients => _currentIngredients;

    public IReadOnlyDictionary<IngredientPropertySO, PropertySpec> CurrentProperties => _currentProperties;

    public event EventHandler onCauldronUpdated; 

    public void AddIngredient(CraftIngredient ingredient)
    {
        _currentIngredients.Add(ingredient);
        
        //Add the properties from the ingredient
        foreach (var ingredientProperty in ingredient.Properties)
        {
            if (!_currentProperties.ContainsKey(ingredientProperty.property))
            {
                _currentProperties.Add(ingredientProperty.property, new PropertySpec()
                {
                    property = ingredientProperty.property,
                    amount = 0
                });
            }

            _currentProperties[ingredientProperty.property].amount += ingredientProperty.amount;
        }
        
        //Calculate any property that cancel each other
        foreach (var currentProperty in _currentProperties.Values)
        {
            foreach (var cancelTarget in currentProperty.property.CancelList)
            {
                if (_currentProperties.TryGetValue(cancelTarget, out var targetProperty))
                {
                    int amountToCancel = Mathf.Min(currentProperty.amount, targetProperty.amount);
                    
                    if(amountToCancel <= 0)
                        continue;
                    
                    currentProperty.amount -= amountToCancel;
                    targetProperty.amount -= amountToCancel;
                }
            }
        }
        
        //Remove any zeroed properties
        _currentProperties = _currentProperties
            .Where(x => x.Value.amount > 0)
            .ToDictionary(x => x.Key, y => y.Value);
        
        onCauldronUpdated?.Invoke(this, EventArgs.Empty);
    }

    public CraftIngredient EvaluateRecipe()
    {
        if (_currentProperties.Count == 0)
            return null;
        
        CraftIngredient potion = new(
            "Potion", //TODO how to name potions?
            _currentProperties.Select(x => new PropertySpec()
            {
                property = x.Key,
                amount = x.Value.amount
            }),
            ItemType.Potion
        );

        _currentIngredients.Clear();
        _currentProperties.Clear();
        
        return potion;
    }
}
