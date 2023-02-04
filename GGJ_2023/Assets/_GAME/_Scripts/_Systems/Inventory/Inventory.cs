using System;
using System.Collections.Generic;

public class InventoryArgs : EventArgs
{
    public InventoryItem itemEntry;
}

public class Inventory
{
    private Dictionary<object, InventoryItem> _currentItems = new();

    public IEnumerable<InventoryItem> CurrentItems => _currentItems.Values;

    public event EventHandler<InventoryArgs> onItemAdded;
    public event EventHandler<InventoryArgs> onItemRemoved;

    public void AddItem(object item)
    {
        if (!_currentItems.ContainsKey(item))
        {
            _currentItems.Add(item, new InventoryItem()
            {
                Item = item,
                Count = 0
            });
        }

        var entry = _currentItems[item];
        entry.Count += 1;
        
        onItemAdded?.Invoke(this, new InventoryArgs() { itemEntry = entry});
    }

    public void RemoveItem(object item)
    {
        if(!_currentItems.ContainsKey(item))
            return;

        var entry = _currentItems[item];
        entry.Count -= 1;

        if (entry.Count <= 0)
            _currentItems.Remove(item);
        
        onItemRemoved?.Invoke(this, new InventoryArgs() { itemEntry = entry});
    }
}
