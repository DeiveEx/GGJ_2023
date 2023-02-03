using System;
using System.Collections.Generic;

public class InventoryArgs : EventArgs
{
    public CraftIngredient item;
}

public class Inventory
{
    private Dictionary<CraftIngredient, int> _currentItems = new();

    public IReadOnlyDictionary<CraftIngredient, int> CurrentItems => _currentItems;

    public event EventHandler<InventoryArgs> onItemAdded;
    public event EventHandler<InventoryArgs> onItemRemoved;

    public void AddItem(CraftIngredient item)
    {
        if(!_currentItems.ContainsKey(item))
            _currentItems.Add(item, 0);

        _currentItems[item] += 1;
        
        onItemAdded?.Invoke(this, new InventoryArgs() { item = item});
    }

    public void RemoveItem(CraftIngredient item)
    {
        if(!_currentItems.ContainsKey(item))
            return;

        _currentItems[item] -= 1;

        if (_currentItems[item] <= 0)
            _currentItems.Remove(item);
        
        onItemRemoved?.Invoke(this, new InventoryArgs() { item = item});
    }
}
