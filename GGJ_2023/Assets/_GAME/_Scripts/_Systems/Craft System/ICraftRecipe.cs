using System.Collections.Generic;

namespace Ignix.CraftSystem
{
    /// <summary>
    /// A Recipe shows which ingredients are needed for you to craft the resulting item
    /// </summary>
    public interface ICraftRecipe
    {
        /// <summary>
        /// The necessary ingredients to craft this recipe
        /// </summary>
        IEnumerable<object> Ingredients { get; }
        
        /// <summary>
        /// The Output of the recipe
        /// </summary>
        IEnumerable<ICraftResult> Result { get; }
    }

    /// <summary>
    /// <inheritdoc cref="ICraftRecipe"/>
    /// <para>This is the Generic version of the interface</para>
    /// </summary>
    /// <typeparam name="TIngredient">The type of the ingredients</typeparam>
    public interface ICraftRecipe<out TIngredient> : ICraftRecipe
    {
        /// <summary>
        /// <inheritdoc cref="ICraftRecipe.Ingredients"/>
        /// </summary>
        new IEnumerable<TIngredient> Ingredients { get; }
    }
    
    /// <summary>
    /// <inheritdoc cref="ICraftRecipe{T}"/>
    /// </summary>
    /// <typeparam name="TIngredient">The type of the ingredients</typeparam>
    /// <typeparam name="TResult">The type of the result</typeparam>
    public interface ICraftRecipe<out TIngredient, out TResult> : ICraftRecipe<TIngredient> where TResult : ICraftResult
    {
        /// <summary>
        /// <inheritdoc cref="ICraftRecipe.Result"/>
        /// </summary>
        new IEnumerable<TResult> Result { get; }
    }
}
