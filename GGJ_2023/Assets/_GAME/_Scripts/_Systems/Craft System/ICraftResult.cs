namespace Ignix.CraftSystem
{
    /// <summary>
    /// When you craft a recipe you're basically creating a new object, so that's exactly what we ask this interface to do
    /// </summary>
    public interface ICraftResult
    {
        /// <summary>
        /// Returns a instance of the result of a recipe
        /// </summary>
        /// <returns></returns>
        object GetResult();
    }
    
    /// <summary>
    /// <inheritdoc cref="ICraftResult"/>
    /// </summary>
    public interface ICraftResult<out T>
    {
        /// <summary>
        /// <inheritdoc cref="ICraftResult.GetResult"/>
        /// </summary>
        T GetResult();
    }
}
