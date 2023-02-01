using System.Collections.Generic;
using System.Linq;
using Ignix.CraftSystem;

public class SimpleCraftingStation : ICraftingStation<SimpleCraftRecipe>
{
    private List<SimpleCraftRecipe> _recipes = new();
    private List<SimpleCraftItem> _currentIngredients = new();

    public IEnumerable<SimpleCraftRecipe> Recipes => _recipes;
    IEnumerable<ICraftRecipe> ICraftingStation.Recipes => Recipes; //In order to use the generic version, we need to implement both interfaces, and for that we can use the Explicit implementation

    public bool Enabled => true;

    public IEnumerable<SimpleCraftItem> Ingredients => _currentIngredients;

    public bool CanAcceptIngredient(SimpleCraftItem ingredient)
    {   
        //If any recipe in this station needs this ingredient, then we can accept the ingredient
        return _recipes.Any(x => x.Ingredients.Any(y => ((SimpleCraftItem)y).Item == ingredient.Item));
    }

    public bool CanCraftRecipe(ICraftRecipe recipe)
    {
        //Check if this is a valid recipe for this station
        if (recipe is not SimpleCraftRecipe simpleRecipe || !_recipes.Contains(recipe))
            return false;
        
        foreach (SimpleCraftItem recipeIngredient in simpleRecipe.Ingredients)
        {   
            var heldIngredient = _currentIngredients.FirstOrDefault(x => x.Item == recipeIngredient.Item);

            if (heldIngredient == null)
                return false;

            if (heldIngredient.Amount < recipeIngredient.Amount)
                return false;
        }

        return true;
    }

    public IEnumerable<object> GetCraftResult(ICraftRecipe recipe)
    {
        //"Consumes" the ingredients
        foreach (SimpleCraftItem ingredient in recipe.Ingredients)
        {
            RemoveIngredient(new SimpleCraftItem(ingredient.Item, ingredient.Amount));
        }
        
        //Returns the results
        List<object> result = new();

        foreach (var resultItem in recipe.Result)
        {   
            result.Add(resultItem.GetResult());
        }

        return result;
    }

    public void AddRecipe(SimpleCraftRecipe recipe)
    {
        _recipes.Add(recipe);
    }

    public void AddIngredient(SimpleCraftItem ingredient)
    {
        var existing = _currentIngredients.FirstOrDefault(x => x.Item == ingredient.Item);

        if (existing != null)
        {
            existing.Amount += ingredient.Amount;
            return;
        }
        
        _currentIngredients.Add(ingredient);
    }

    public void RemoveIngredient(SimpleCraftItem ingredient)
    {
        var existing = _currentIngredients.FirstOrDefault(x => x.Item == ingredient.Item);

        if (existing != null)
        {
            existing.Amount -= ingredient.Amount;

            if (existing.Amount > 0)
                return;
            else
                existing.Amount = 0;
        }
        
        _currentIngredients.Remove(existing);
    }
}
