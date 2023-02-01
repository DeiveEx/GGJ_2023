using System.Collections.Generic;
using Ignix.CraftSystem;

public class SimpleCraftRecipe : ICraftRecipe
{
    private List<SimpleCraftItem> _ingredients = new();
    private List<SimpleCraftItem> _result = new();

    public IEnumerable<object> Ingredients => _ingredients;

    public IEnumerable<ICraftResult> Result => _result;

    public void AddIngredient(SimpleCraftItem ingredient)
    {
        _ingredients.Add(ingredient);
    }
    
    public void AddResult(SimpleCraftItem result)
    {
        _result.Add(result);
    }
}
