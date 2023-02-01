using System.Collections.Generic;

public static class Helper
{
    public static Dictionary<IngredientProperty, int> GetPropertiesFromValues(IEnumerable<IngredientPropertyValue> values)
    {
        Dictionary<IngredientProperty, int> properties = new();
        
        foreach (var ingredientProperty in values)
        {
            if (properties.ContainsKey(ingredientProperty.property))
                properties[ingredientProperty.property] += ingredientProperty.amount;
            else
                properties.Add(ingredientProperty.property, ingredientProperty.amount);
        }

        return properties;
    }
    
    public static Dictionary<IngredientProperty, int> GetPropertiesFromIngredients(IEnumerable<CraftIngredient> ingredients)
    {
        Dictionary<IngredientProperty, int> properties = new();

        foreach (var ingredient in ingredients)
        {
            foreach (var ingredientProperty in ingredient.properties)
            {
                if (properties.ContainsKey(ingredientProperty.property))
                    properties[ingredientProperty.property] += ingredientProperty.amount;
                else
                    properties.Add(ingredientProperty.property, ingredientProperty.amount);
            }
        }

        return properties;
    }
}
