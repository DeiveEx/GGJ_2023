using System.Collections.Generic;

namespace Ignix.CraftSystem
{
    /// <summary>
    /// A craft station is an object that can create other objects based on recipes
    /// </summary>
    public interface ICraftingStation
    {
        /// <summary>
        /// The recipes this station can craft
        /// </summary>
        IEnumerable<ICraftRecipe> Recipes { get; }
        
        /// <summary>
        /// Is this craft station currently able to craft recipes?
        /// </summary>
        bool Enabled { get; }

        /// <summary>
        /// Can the given recipe be crafted by this station?
        /// </summary>
        /// <param name="recipe">The recipe to craft</param>
        /// <returns>True if the recipe can be crafted</returns>
        bool CanCraftRecipe(ICraftRecipe recipe);
        
        /// <summary>
        /// Crafts the recipe and returns the resulting objects
        /// </summary>
        /// <param name="recipe">The recipe to craft</param>
        /// <returns>A collection of objects as a result of this craft</returns>
        IEnumerable<object> GetCraftResult(ICraftRecipe recipe);
    }

    /// <summary>
    /// <inheritdoc cref="ICraftingStation"/>
    /// </summary>
    /// <typeparam name="TRecipe">The type of the recipes</typeparam>
    public interface ICraftingStation<out TRecipe> : ICraftingStation where TRecipe : ICraftRecipe
    {
        /// <summary>
        /// <inheritdoc cref="ICraftingStation.Recipes"/>
        /// </summary>
        new IEnumerable<TRecipe> Recipes { get; }
    }

    /// <summary>
    /// <inheritdoc cref="ICraftingStation"/>
    /// </summary>
    /// <typeparam name="TRecipe">The type of the recipes</typeparam>
    /// <typeparam name="TResult"></typeparam>
    public interface ICraftingStation<out TRecipe, out TResult> : ICraftingStation<TRecipe> where TRecipe : ICraftRecipe where TResult : ICraftResult
    {
        new IEnumerable<TResult> GetCraftResult(ICraftRecipe recipe);
    }
}
