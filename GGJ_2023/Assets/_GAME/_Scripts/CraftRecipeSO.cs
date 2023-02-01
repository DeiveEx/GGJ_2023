using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class CraftRecipe
{
    public string recipeName;
    public List<IngredientPropertyValue> requirements = new();
    public List<CraftIngredientSO> results = new();
    
}

[CreateAssetMenu(fileName = "Recipe", menuName = "Custom/New Recipe")]
public class CraftRecipeSO : ScriptableObject
{
    [SerializeField] private CraftRecipe _recipeInfo;

    public bool MatchRecipe(IEnumerable<CraftIngredient> ingredients)
    {
        var values = Helper.GetPropertiesFromIngredients(ingredients);

        foreach (var requirement in _recipeInfo.requirements)
        {
            if (!values.TryGetValue(requirement.property, out int amount) ||
                amount < requirement.amount)
                return false;
        }

        return true;
    }
    
    public IEnumerable<CraftIngredient> GetResults()
    {
        return _recipeInfo.results.Select(x => x.IngredientInfo);
    }
}
