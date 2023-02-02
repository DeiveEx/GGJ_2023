using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CauldronCraftStation : MonoBehaviour
{
    private List<CraftIngredient> _currentIngredients = new();
    private Dictionary<IngredientProperty, int> _currentProperties = new();

    public IEnumerable<CraftIngredient> CurrentIngredients => _currentIngredients;

    public IReadOnlyDictionary<IngredientProperty, int> CurrentProperties => _currentProperties;

    public void AddIngredient(CraftIngredient ingredient)
    {
        _currentIngredients.Add(ingredient);

        foreach (var ingredientProperty in ingredient.Properties)
        {
            if(!_currentProperties.ContainsKey(ingredientProperty.property))
                _currentProperties.Add(ingredientProperty.property, 0);

            _currentProperties[ingredientProperty.property] += ingredientProperty.amount;
        }
    }

    public CraftIngredient EvaluateRecipe()
    {
        CraftIngredient potion = new(
            "Result Potion", //TODO how to name potions?
            _currentProperties.Select(x => new PropertySpec()
            {
                property = x.Key,
                amount = x.Value
            })
        );

        _currentIngredients.Clear();
        _currentProperties.Clear();
        
        return potion;
    }
}
