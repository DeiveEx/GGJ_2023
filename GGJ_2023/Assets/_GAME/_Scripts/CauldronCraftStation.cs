using System.Collections.Generic;
using UnityEngine;

public class CauldronCraftStation : MonoBehaviour
{
    [SerializeField] private List<CraftRecipeSO> _recipes = new();

    private List<CraftIngredient> _currentIngredients = new();

    public IEnumerable<CraftIngredient> CurrentIngredients => _currentIngredients;

    public void AddIngredient(CraftIngredient ingredient)
    {
        _currentIngredients.Add(ingredient);
    }

    public IEnumerable<CraftIngredient> EvaluateRecipe()
    {
        //See if the current combination matches any recipe
        foreach (var specialRecipe in _recipes)
        {
            if (specialRecipe.MatchRecipe(_currentIngredients))
                return specialRecipe.GetResults();
        }
        
        //Failure...
        return null;
    }

    public void ClearIngredients()
    {
        _currentIngredients.Clear();
    }
}
