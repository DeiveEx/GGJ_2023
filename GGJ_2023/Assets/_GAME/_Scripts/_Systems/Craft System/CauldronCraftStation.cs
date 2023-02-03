using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CauldronCraftStation : MonoBehaviour
{
    private List<CraftIngredient> _currentIngredients = new();
    private Dictionary<IngredientPropertySO, PropertySpec> _currentProperties = new();

    public IEnumerable<CraftIngredient> CurrentIngredients => _currentIngredients;

    public IReadOnlyDictionary<IngredientPropertySO, PropertySpec> CurrentProperties => _currentProperties;

    public void AddIngredient(CraftIngredient ingredient)
    {
        _currentIngredients.Add(ingredient);

        //We create a duplicate list of properties
        var propertyListClone = ingredient.Properties.Select(x => new PropertySpec()
        {
            property = x.property,
            amount = x.amount
        }).ToList();
        
        // //Then we cancel any existing properties, removing any property that was used to cancel another property
        // for (int i = 0; i < propertyListClone.Count; i++)
        // {
        //     var ingredientProperty = propertyListClone[i];
        //     
        //     foreach (var cancelTarget in ingredientProperty.property.CancelList)
        //     {
        //         if (_currentProperties.TryGetValue(cancelTarget, out var targetProperty))
        //         {
        //             int amountToCancel = Mathf.Min(ingredientProperty.amount, targetProperty.amount);
        //             targetProperty.amount -= amountToCancel;
        //             ingredientProperty.amount -= amountToCancel;
        //             
        //             Debug.Log($"[{ingredient.IngredientName}] has property [{ingredientProperty.property.PropertyName}], which cancelled [{amountToCancel}] units of [{targetProperty.property.PropertyName}]");
        //
        //             if (targetProperty.amount <= 0)
        //             {
        //                 _currentProperties.Remove(cancelTarget);
        //             }
        //
        //             if (ingredientProperty.amount == 0)
        //             {
        //                 propertyListClone.RemoveAt(i);
        //                 i--;
        //             }
        //         }
        //     }
        // }
        
        //Finally, with whatever is remaining in the list, we add to the current list of properties
        foreach (var ingredientProperty in propertyListClone)
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
    }

    public CraftIngredient EvaluateRecipe()
    {
        CraftIngredient potion = new(
            "Result Potion", //TODO how to name potions?
            _currentProperties.Select(x => new PropertySpec()
            {
                property = x.Key,
                amount = x.Value.amount
            })
        );

        _currentIngredients.Clear();
        _currentProperties.Clear();
        
        return potion;
    }
}
