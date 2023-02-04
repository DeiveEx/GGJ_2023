using System;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class Item
{
    [FormerlySerializedAs("_ingredientName")]
    [SerializeField] protected string _itemName;

    public string ItemName => _itemName;

    public Item(string itemName)
    {
        _itemName = itemName;
    }
}
