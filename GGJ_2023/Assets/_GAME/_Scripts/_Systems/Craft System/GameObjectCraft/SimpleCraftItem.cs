using Ignix.CraftSystem;

public class SimpleCraftItem : ICraftResult
{
    public CraftItem Item { get; }
    public int Amount { get; set; }

    public SimpleCraftItem(CraftItem item, int amount)
    {
        Item = item;
        Amount = amount;
    }

    public object GetResult()
    {
        return new SimpleCraftItem(Item, Amount);
    }
}
